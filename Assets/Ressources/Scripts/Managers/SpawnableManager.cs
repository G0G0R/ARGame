using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

public class SpawnableManager : MonoBehaviour
{

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();
   
    
    [SerializeField]
    private GameObject spawnablePrefab;

    [SerializeField]
    private List<GameObject> spawnablePrefabs;

    [SerializeField]
    private GameObject uiPanel;

    [SerializeField]
    private GameObject smallBoys;

    Camera arCam;
    GameObject spawnedObject;

    [SerializeField]
    private EnumGameModes mode;

    public float smallGuySpeed = 1.0f;

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
                bool isOverUI = IsPointOverUIObject(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit) && !isOverUI)
                {
                    if(hit.collider.gameObject.tag == "Spawnable" && hit.collider.gameObject.tag!= "UI")
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
                            TooltipManager._instance.SetAndShowTooltip(hit.collider.gameObject, m_Hits[0].pose.position);
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

            GameObject nearestBuilding = FindNearestBuildingWithCapacity(spawnPosition);
            if (nearestBuilding != null)
            {
                Vector3 startPosition = nearestBuilding.transform.position;
                GameObject smallGuy = Instantiate(smallBoys, startPosition, Quaternion.identity);

                // Tourner le smallGuy vers la destination dès son apparition
                Vector3 direction = (spawnedObject.transform.position - startPosition).normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                smallGuy.transform.rotation = targetRotation;

                StartCoroutine(MoveToTarget(smallGuy, spawnedObject.transform.position));
            }

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

    public bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;

    }

    private GameObject FindNearestBuildingWithCapacity(Vector3 currentPosition)
    {
        GameObject nearestBuilding = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Batiment building in GameManager.Instance.getBatiments_list())
        {
            if (building.Capacite > 0)
            {
                float distance = Vector3.Distance(currentPosition, building.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestBuilding = building.gameObject;
                }
            }
        }

        return nearestBuilding;
    }


    private IEnumerator MoveToTarget(GameObject smallGuy, Vector3 targetPosition)
    {
        while (Vector3.Distance(smallGuy.transform.position, targetPosition) > 0.1f)
        {
            smallGuy.transform.position = Vector3.MoveTowards(smallGuy.transform.position, targetPosition, smallGuySpeed * Time.deltaTime);
            yield return null;
        }

        // Arrivé à la destination
        smallGuy.transform.position = targetPosition;
    }
}
