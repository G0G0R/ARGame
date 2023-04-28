using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.ARFoundation;

public class TooltipManager : MonoBehaviour
{

    public static TooltipManager _instance;
    public TextMeshProUGUI textComponent;
    public GameObject batimentObject;
    private RectTransform tooltipTransform;

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

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(batimentObject.transform.position);
        float scaleFactor = gameObject.transform.GetChild(0).GetComponent<Canvas>().scaleFactor;
        Vector2 finalPosition = new Vector2(screenPosition.x / scaleFactor, screenPosition.y / scaleFactor);
        tooltipTransform.anchoredPosition = finalPosition;
    }

    public void SetAndShowTooltip(GameObject batiment, Vector3 pos)
    {
        gameObject.SetActive(true);
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        textComponent.text = batiment.GetComponent<Batiment>().Intitule;
        batimentObject = batiment;
        //print("Position de l'objet spawn :");
        //print(batiment.transform.position);
        //print("position du batiment traduite en coord 2D :");
        //print(Camera.main.WorldToScreenPoint(batiment.transform.position));
        //print("position du tooltip avant changement:");
        //print(gameObject.transform.GetChild(0).position);
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(batiment.transform.position);
        float scaleFactor = gameObject.transform.GetChild(0).GetComponent<Canvas>().scaleFactor;
        Vector2 finalPosition = new Vector2(screenPosition.x / scaleFactor, screenPosition.y / scaleFactor);
        tooltipTransform.anchoredPosition = finalPosition;
        //gameObject.transform.GetChild(0).position = batiment.transform.position;
        //print("position du tooltip après changement:");
        //print(gameObject.transform.GetChild(0).position);


    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }

    public void Ameliorer()
    {
        batimentObject.GetComponent<Batiment>().Ameliorer();
        this.textComponent.text = batimentObject.GetComponent<Batiment>().Intitule;
    }

    public void Supprimer()
    {
        Destroy(batimentObject);
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.SetActive(false);

    }

}
