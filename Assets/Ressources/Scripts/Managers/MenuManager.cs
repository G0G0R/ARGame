using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MenuManager : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> buildButtons;

    private CanvasGroup activeCanvasgroup;

    private bool mainMenuShown = false;

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
}
