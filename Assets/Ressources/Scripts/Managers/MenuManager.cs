using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> buildButtons;

    [SerializeField]
    private List<Button> liste_bouttons_1;

    [SerializeField]
    private List<Button> liste_bouttons_2;

    [SerializeField]
    private List<Button> liste_bouttons_3;

    private CanvasGroup activeCanvasgroup;

    private bool mainMenuShown = false;

    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
            }

            return instance;
        }
    }

    public void AfficherConstructionMenu()
    {
        if (mainMenuShown)
        {
            DisableActiveCG();
        }
        mainMenuShown = !mainMenuShown;
        
        foreach (GameObject button in buildButtons)
        {
            button.SetActive(!button.activeSelf);
        }
    }

    private void DisableActiveCG()
    {
        if (activeCanvasgroup != null)
        {
            activeCanvasgroup.alpha = 0;
            activeCanvasgroup.blocksRaycasts = false;
            activeCanvasgroup = null;
        }
    
    }
    public void ShowMenu(CanvasGroup c)
    {
        if (c != activeCanvasgroup)
        {
            DisableActiveCG();
        }

        if (c != null)
        {

            c.alpha = (c.alpha == 1) ? 0 : 1;
            c.blocksRaycasts = !c.blocksRaycasts;

            if (c.alpha == 1)
            {
                activeCanvasgroup = c;
            }

        }
    }

    public void ActiverBoutonsConstruction(int niveau)
    {
        switch(niveau)
        {
            case 1:
                foreach(Button b in liste_bouttons_1)
                {
                    b.interactable = true;
                }
                break;
            case 2:
                foreach (Button b in liste_bouttons_2)
                {
                    b.interactable = true;
                }
                break;
            case 3:
                foreach (Button b in liste_bouttons_3)
                {
                    b.interactable = true;
                }
                break;
            default: 
                break;
        }
    }
}
