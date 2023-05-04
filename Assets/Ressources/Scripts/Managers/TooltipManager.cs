using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class TooltipManager : MonoBehaviour
{

    public static TooltipManager _instance;
    public TextMeshProUGUI infoText;
    public GameObject batimentObject;
    private RectTransform tooltipTransform;
    private GameObject infoCanva;

    private void Awake ()
    {
        if(_instance == null)
            {
            _instance = FindObjectOfType<TooltipManager>();
        }
        tooltipTransform = transform.Find("TooltipBox").GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        infoCanva = gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
        infoCanva.SetActive(false);
        Debug.Log(infoCanva);
        Debug.Log(gameObject.transform.GetChild(0).gameObject.name);
        Debug.Log(GameObject.Find("UIelements").name);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(batimentObject.transform.position);
        float scaleFactor = gameObject.transform.GetChild(0).GetComponent<Canvas>().scaleFactor;
        //float scaleFactor = GameObject.Find("UIelements").transform.GetComponent<Canvas>().scaleFactor;
        Vector2 finalPosition = new Vector2(screenPosition.x / scaleFactor, screenPosition.y / scaleFactor);
        tooltipTransform.anchoredPosition = finalPosition;
    }

    public void SetAndShowTooltip(GameObject batiment, Vector3 pos)
    {
        gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        batimentObject = batiment;
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(batiment.transform.position);
        float scaleFactor = gameObject.transform.GetChild(0).GetComponent<Canvas>().scaleFactor;
        //float scaleFactor = GameObject.Find("UIelements").transform.GetComponent<Canvas>().scaleFactor;
        Vector2 finalPosition = new Vector2(screenPosition.x / scaleFactor, screenPosition.y / scaleFactor);
        tooltipTransform.anchoredPosition = finalPosition;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        CloseInformation();

    }

    public void Ameliorer()
    {
        batimentObject.GetComponent<Batiment>().Ameliorer();
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        CloseInformation();
    }

    public void Supprimer()
    {
        Destroy(batimentObject);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);
        CloseInformation();

    }

    public void ShowInformation()
    {
        infoCanva.SetActive(true);
        if (batimentObject.GetComponent<Batiment>().Capacite > 0)
        {
            infoText.text = batimentObject.GetComponent<Batiment>().Intitule + "\nCapacité : " + batimentObject.GetComponent<Batiment>().Capacite;
        }
        if (batimentObject.GetComponent<Batiment>().getProductionArgent() > 0)
        {
            infoText.text = batimentObject.GetComponent<Batiment>().Intitule + "\nProduction d'argent : " + batimentObject.GetComponent<Batiment>().getProductionArgent();
        }
        if (batimentObject.GetComponent<Batiment>().getProductionNourriture() > 0)
        {
            infoText.text = batimentObject.GetComponent<Batiment>().Intitule + "\nProduction de nourriture : " + batimentObject.GetComponent<Batiment>().getProductionNourriture();
        }

    }

    public void CloseInformation()
    {
        infoCanva.SetActive(false);
    }

}
