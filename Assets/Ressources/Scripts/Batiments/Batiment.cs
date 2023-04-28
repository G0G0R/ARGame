using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Batiment : MonoBehaviour
{

    [SerializeField]
    private GameObject batimentToUpdate;

    [SerializeField]
    private int prix;

    [SerializeField]
    private string intitule;

    [SerializeField]
    private int capacite;

    [SerializeField]
    private int production_nourriture;

    [SerializeField]
    private int production_argent;

    [SerializeField]
    private int nb_travailleurs_total;
    private int nb_travailleurs;

    public string Intitule { get => intitule; }


    [SerializeField]
    private int index;

    public int Index { get => index; }


    public int Prix { get => prix; }
    public int Capacite { get => capacite; }
    public int ProductionNourriture { get => production_nourriture; }
    public int ProductionArgent { get => production_argent; }
    public int NBTravailleurs { get => nb_travailleurs; set { nb_travailleurs = value; } }
    public int NBTravailleursTotal { get => nb_travailleurs_total; }



    // Start is called before the first frame update
    void Start()
    {
        nb_travailleurs = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ameliorer()
    {
        if (batimentToUpdate != null && GameManager.Instance.UpdateMonnaie(-batimentToUpdate.GetComponent<Batiment>().Prix))
        {
            Instantiate(batimentToUpdate, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void OnDestroy()
    {
        GameManager.Instance.UpdateHabitants(-this.capacite);
        GameManager.Instance.UpdateHabitantsDisponibles(-this.capacite);
        GameManager.Instance.UpdateHabitantsDisponibles(this.nb_travailleurs);
        GameManager.Instance.RetirerBatiment(this);
    }

    public int getProductionNourriture()
    {
        if (this.nb_travailleurs_total > 0)
        {
            return this.production_nourriture * (this.nb_travailleurs / this.nb_travailleurs_total);
        }
        else if(this.capacite > 0)
        {
            return -this.capacite;
        }
        else
        {
            return 0;
        }
    }

    public int getProductionArgent()
    {
        if (this.nb_travailleurs_total > 0)
        {
            return this.production_argent * (this.nb_travailleurs / this.nb_travailleurs_total);
        }
        else
        {
            return 0;
        }
    }
}
