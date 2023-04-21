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

    public string Intitule { get => intitule; }


    [SerializeField]
    private int index;

    public int Index { get => index; }


    public int Prix { get => prix; }



    // Start is called before the first frame update
    void Start()
    {
        
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
}
