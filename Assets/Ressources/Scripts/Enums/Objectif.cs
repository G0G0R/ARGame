using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Objectif 
{
    [SerializeField]
    private string nom;

    [SerializeField]
    private int id;

    [SerializeField]
    private int qte;

    public int Id { get => id; }
    public int Qte { get => qte; }
    public String Nom { get => nom; }
}
