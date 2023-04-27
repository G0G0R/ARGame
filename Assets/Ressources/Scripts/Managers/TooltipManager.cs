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
    private RectTransform rectTransform;

    private void Awake ()
    {
        if(_instance == null)
            {
            _instance = FindObjectOfType<TooltipManager>();
        }
        rectTransform = transform.GetComponent<RectTransform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.anchoredPosition3D = Camera.main.WorldToScreenPoint(batimentObject.transform.position);
    }

    public void SetAndShowTooltip(GameObject batiment)
    {
        gameObject.SetActive(true);
        gameObject.transform.parent.gameObject.SetActive(true);
        textComponent.text = batiment.GetComponent<Batiment>().Intitule;
        batimentObject = batiment;
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
        textComponent.text = string.Empty;
    }

    public void Ameliorer()
    {
        batimentObject.GetComponent<Batiment>().Ameliorer();
        HideTooltip();
        SetAndShowTooltip(batimentObject);
    }

    public void Supprimer()
    {
        Destroy(batimentObject);
        HideTooltip();
    }
}
