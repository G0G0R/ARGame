using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private int argent;

    [SerializeField]
    private TextMeshProUGUI affichage_argent;

    private static GameManager instance;

    public static GameManager Instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {

        UpdateTextArgent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTextArgent()
    {
        affichage_argent.text = this.argent.ToString();
    }

    public bool UpdateMonnaie(int monnaie)
    {
        bool res = true;
        if(this.argent >= -monnaie)
        {
            this.argent += monnaie;
            UpdateTextArgent();
        }
        else
        {
            res = false;
        }
        return res;
    }
}
