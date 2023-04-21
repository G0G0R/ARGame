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

    [SerializeField]
    private EnumGameModes gameMode;

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

    public EnumGameModes GameModes { get => gameMode;  }

    
    // Start is called before the first frame update
    void Start()
    {

        UpdateTextArgent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeGameMode(int gm)
    {
        gameMode = (EnumGameModes)Enum.ToObject(typeof(EnumGameModes), gm);
        DisplayMode.Instance.ChangeGameModeDisplay(gameMode);
        //todo, make other change relatives to game mode
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
