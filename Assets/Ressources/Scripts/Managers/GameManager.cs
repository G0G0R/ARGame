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
    private int nourriture;

    [SerializeField]
    private int habitants;
    private int habitants_dispo;

    [SerializeField]
    private TextMeshProUGUI affichage_argent;

    [SerializeField]
    private TextMeshProUGUI affichage_nourriture;

    [SerializeField]
    private TextMeshProUGUI affichage_habitants;

    [SerializeField]
    private EnumGameModes gameMode;

    private int variation_argent;
    private int variation_nourriture;
    private int variation_habitants;

    private List<Batiment> batiments_list;



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
        habitants_dispo = habitants;
        batiments_list = new List<Batiment>();

        UpdateTextArgent();
        UpdateTextHabitants();
        UpdateTextNourriture();
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
    public void UpdateTextNourriture()
    {
        affichage_nourriture.text = this.nourriture.ToString();
    }
    public void UpdateTextHabitants()
    {
        affichage_habitants.text = this.habitants_dispo.ToString() + '/' + this.habitants.ToString();
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

    public bool UpdateNourriture(int food)
    {
        bool res = true;
        if (this.nourriture >= -food)
        {
            this.nourriture += food;
        }
        else
        {
            this.nourriture = 0;
            res = false;
        }
        UpdateTextNourriture();
        return res;
    }

    public bool UpdateHabitants(int people)
    {
        bool res = true;
        if (this.habitants >= -people)
        {
            this.habitants += people;
        }
        else
        {
            this.habitants = 0;
            res = false;
        }
        UpdateTextHabitants();
        return res;
    }

    public void MAJRessources()
    {
        int majnourriture = 0;
        int majargent = 0;
        foreach(Batiment b in this.batiments_list)
        {
            majnourriture += b.getProductionNourriture();
            majargent += b.getProductionArgent();
        }
        UpdateNourriture(majnourriture);
        UpdateMonnaie(majargent);
    }

    public void AttribuerTravailleurs(Batiment batiment)
    {
        if (batiment.NBTravailleursTotal - batiment.NBTravailleurs > this.habitants_dispo )
        {
            batiment.NBTravailleurs += this.habitants_dispo;
            this.habitants_dispo = 0;
        }
        else
        {
            this.habitants_dispo -= batiment.NBTravailleursTotal - batiment.NBTravailleurs;
            batiment.NBTravailleurs = batiment.NBTravailleursTotal;
        }
        UpdateTextHabitants();
    }

    public void UpdateHabitantsDisponibles(int capacite)
    {
        if (this.habitants_dispo >= -capacite)
        {
            this.habitants_dispo += capacite;
        }
        else
        {
            RetirerTravailleursAll(-capacite - this.habitants_dispo);
            this.habitants_dispo = 0;
        }
        UpdateTextHabitants();
    }

    private void RetirerTravailleursAll(int nombre)
    {
        int i = 0;
        int save;
        while (nombre > 0)
        {
            if (batiments_list[i].NBTravailleurs > 0)
            {
                save = batiments_list[i].NBTravailleurs;
                RetirerTravailleurs(batiments_list[i], nombre);
                nombre = nombre - save;
            }
            i++;
        }
    }

    private void RetirerTravailleurs(Batiment batiment, int nombre)
    {
        batiment.NBTravailleurs = Math.Max(0, batiment.NBTravailleurs - nombre);
    }

    internal void AjouterBatiment(Batiment batiment)
    {
        this.batiments_list.Add(batiment);
    }

    internal void AttribuerTravailleursAll()
    {
        int i = 0;
        while( i <= this.batiments_list.Count && this.habitants_dispo > 0)
        {
            if (!(batiments_list[i].NBTravailleurs == batiments_list[i].NBTravailleursTotal))
            {
                AttribuerTravailleurs(batiments_list[i]);
            }
            i++;
        }
    }

    public void RetirerBatiment(Batiment batiment)
    {
        this.batiments_list.Remove(batiment);
    }
     
    public List<Batiment> getBatiments_list()
    {
        return batiments_list;
    }


}
