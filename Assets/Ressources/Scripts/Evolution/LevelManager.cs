using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField]
    private List<Condition> liste_conditions;

    private int niveau_actuel;

    [SerializeField]
    private int niveau_max;

    [SerializeField]
    private TextMeshProUGUI textmesh;



    private static LevelManager instance;

    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            return instance;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        niveau_actuel = 0;
        UpdateObjectif();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Check()
    {
        if (niveau_actuel <= niveau_max && liste_conditions[niveau_actuel].EstValide())
        {
            niveau_actuel++;
            MenuManager.Instance.ActiverBoutonsConstruction(niveau_actuel);
            UpdateObjectif();
        }
    }

    public void UpdateObjectif()
    {
        if (niveau_actuel >= niveau_max && liste_conditions[niveau_actuel - 1].EstValide())
        {
            textmesh.SetText("Félicitations, vous avez accompli toutes les missions!");
        }
        else
        {
            textmesh.SetText(liste_conditions[niveau_actuel].GetText());
        }
    }

}
