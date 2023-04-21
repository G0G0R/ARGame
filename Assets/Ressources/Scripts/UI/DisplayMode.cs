using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayMode : MonoBehaviour
{

    [SerializeField] 
    private TextMeshProUGUI textMode;

    [SerializeField]
    private Image imageMode;

    [SerializeField]
    private GameObject displayArea;

    private static DisplayMode instance;

    public static DisplayMode Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DisplayMode>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
   
    }

   
    public void ChangeGameModeDisplay(EnumGameModes mode)
    {
        if (mode == EnumGameModes.NONE)
        {
            displayArea.gameObject.SetActive(false);
        }
        else
        {
            displayArea.gameObject.SetActive(true);
            textMode.text = "Mode " + mode.ToString();

        }
    }
}
