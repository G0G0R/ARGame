using System;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{
    [SerializeField]
    private List<Objectif> liste_arguments;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool EstValide()
    {
        bool res = true;
        foreach (Objectif obj in liste_arguments)
        {
            switch (obj.Id)
            {
                case 0:
                    if (GameManager.Instance.Nourriture < obj.Qte)
                    {
                        res = false;
                    }
                    break;
                case 1:
                    if (GameManager.Instance.Argent < obj.Qte)
                    {
                        res = false;
                    }
                    break;
                case 2:
                    if (GameManager.Instance.Habitants < obj.Qte)
                    {
                        res = false;
                    }
                    break;
                default:
                    break;
            }

        }
        return res;
    }

    public string GetText()
    {
        String chaine = "Quête: ";

        foreach (Objectif obj in liste_arguments)
        {
            switch (obj.Id)
            {
                case 0:
                    chaine += "Nourriture: " + GameManager.Instance.Nourriture + "/" + obj.Qte;
                    break;
                case 1:
                    chaine += "Argent: " + GameManager.Instance.Argent + "/" + obj.Qte;
                    break;
                case 2:
                    chaine += "Habitants: " + GameManager.Instance.Habitants + "/" + obj.Qte;
                    break;
                default:
                    break;

            }
            chaine += " ";
        }

        return chaine;
    }

}