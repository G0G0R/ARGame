using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Linq;

public class SpawnableManager : MonoBehaviour
{

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
   
    
    [SerializeField]
    private GameObject spawnablePrefab;

    [SerializeField]
    private List<GameObject> spawnablePrefabs;

    Camera arCam;
    GameObject spawnedObject;

    [SerializeField]
    private EnumGameModes mode;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0)
        {
            return;
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

        if (m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began && spawnedObject == null)
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag == "Spawnable")
                    {
                        if (mode == EnumGameModes.DELETION)
                        {
                            Destroy(hit.collider.gameObject);
                        }
                        else if (mode == EnumGameModes.AMELIORATION)
                        {
                            hit.collider.gameObject.GetComponent<Batiment>().Ameliorer();
                        }
                        else
                        {
                            spawnedObject = hit.collider.gameObject;
                        }
                    }
                    else if(mode == EnumGameModes.CREATION)
                    {
                        SpawnPrefab(m_Hits[0].pose.position);
                    }
                }
            }

            else if (Input.GetTouch(0).phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = m_Hits[00].pose.position;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnedObject = null;
            }
        }
    }

    private void SpawnPrefab(Vector3 spawnPosition)
    {
        if (GameManager.Instance.UpdateMonnaie(-spawnablePrefab.GetComponent<Batiment>().Prix)) {
            spawnedObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
            GameManager.Instance.AjouterBatiment(spawnedObject.GetComponent<Batiment>());
            if (spawnablePrefab.GetComponent<Batiment>().Capacite > 0)
            {
                GameManager.Instance.UpdateHabitants(spawnedObject.GetComponent<Batiment>().Capacite);
                GameManager.Instance.UpdateHabitantsDisponibles(spawnedObject.GetComponent<Batiment>().Capacite);
                GameManager.Instance.AttribuerTravailleursAll();
            }
            else
            {
                GameManager.Instance.AttribuerTravailleurs(spawnedObject.GetComponent<Batiment>());
            }
        }
    }

    public void ModeCreation()
    {
        mode = EnumGameModes.CREATION;
    }

    public void ModeDelation()
    {
        mode = EnumGameModes.DELETION;
    }

    public void ModeAmelioration()
    {
        mode = EnumGameModes.AMELIORATION;
    
    }

    public void Construction(int index)
    {
        spawnablePrefab = spawnablePrefabs.Find(item => item.GetComponent<Batiment>().Index == index);
    }
}
