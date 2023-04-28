using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaneManager : MonoBehaviour
{
    [SerializeField] private ARPlaneManager arPlaneManager;
    [SerializeField] private float minPlaneSize = 1.0f; // Définissez la taille minimale des plans à conserver.

    private List<ARPlane> planesToMerge = new List<ARPlane>();

    private void Awake()
    {
        print("awake");
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDestroy()
    {
        print("Destroy");
        arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        MergePlanes(args.updated);
        RemoveSmallPlanes(args.updated);
        //RemoveOverlappingPlanes(args.updated);
    }

    private void MergePlanes(List<ARPlane> updatedPlanes)
    {
        // Remove any null plane objects from the list
        //updatedPlanes.RemoveAll(p => p == null);

        for (int i = 0; i < updatedPlanes.Count; i++)
        {
            ARPlane plane1 = updatedPlanes[i];

            if (plane1 == null || plane1.gameObject.activeSelf == false)
            {
                continue;
            }

            for (int j = i + 1; j < updatedPlanes.Count; j++)
            {
                ARPlane plane2 = updatedPlanes[j];

                if (plane2 == null)
                {
                    continue;
                }

                if (plane1 == plane2) continue;

                if (plane2.gameObject.activeSelf == false) continue;


                if (ArePlanesNeighbours(plane1, plane2))
                {
                    // Combine the boundary points of both planes
                    List<Vector3> combinedBoundary = new List<Vector3>(plane1.boundary.Length);
                    for (int k = 0; k < plane1.boundary.Length; k++)
                    {
                        combinedBoundary.Add(plane1.boundary[k]);
                    }
                    for (int k = 0; k < plane2.boundary.Length; k++)
                    {
                        combinedBoundary.Add(plane2.boundary[k]);
                    }

                    // Create a new mesh from the combined boundary points
                    Mesh newMesh = new Mesh();
                    newMesh.SetVertices(combinedBoundary);
                    List<int> triangles = new List<int>();
                    for (int t = 0; t < combinedBoundary.Count - 2; t++)
                    {
                        triangles.Add(0);
                        triangles.Add(t + 1);
                        triangles.Add(t + 2);
                    }
                    newMesh.SetTriangles(triangles, 0);
                  
                  

                    // Update the first plane's meshs
                    plane1.GetComponent<MeshFilter>().mesh = newMesh;

                    // Destroy the second plane's GameObject
                    //plane2.gameObject.SetActive(false);
                    print("DESTROY MERGE");
                    // Remove the second plane from the updatedPlanes list
                    updatedPlanes.RemoveAt(j);
                    j--;
                }
            }
        }
    }








    private List<Vector2> CombineBoundaries(List<Vector2> boundary1, List<Vector2> boundary2)
    {
        List<Vector2> combinedBoundary = new List<Vector2>();
        HashSet<Vector2> uniquePoints = new HashSet<Vector2>();

        foreach (Vector2 point in boundary1)
        {
            combinedBoundary.Add(point);
            uniquePoints.Add(point);
        }

        foreach (Vector2 point in boundary2)
        {
            if (!uniquePoints.Contains(point))
            {
                combinedBoundary.Add(point);
                uniquePoints.Add(point);
            }
        }

        return combinedBoundary;
    }


    private bool ArePlanesNeighbours(ARPlane plane1, ARPlane plane2)
    {
        // Calculate the distance and angle thresholds
        float distanceThreshold = 0.5f;
        float angleThreshold = 10.0f;

        // Calculate the distance in 2D space (excluding the Y axis)
        Vector2 center1 = new Vector2(plane1.center.x, plane1.center.z);
        Vector2 center2 = new Vector2(plane2.center.x, plane2.center.z);
        float distance = Vector2.Distance(center1, center2);

        // Check if the distance in 2D space is less than the threshold
        if (distance > distanceThreshold) return false;

        // Calculate the angle between the two planes
        float angle = Vector3.Angle(plane1.normal, plane2.normal);

        // Check if the angle is less than the threshold
        if (angle > angleThreshold) return false;

        // Calculate the distance threshold on the Y axis with a margin of 20%
        float yThreshold = distanceThreshold * 0.2f;
        float yDiff = Mathf.Abs(plane1.center.y - plane2.center.y);
        if (yDiff >= yThreshold) return false;

        // The two planes are neighbours
        return true;
    }


    private void RemoveSmallPlanes(List<ARPlane> updatedPlanes)
    {
        foreach (var plane in updatedPlanes)
        {
            if (plane == null || plane.gameObject.activeSelf == false)
            {
                continue;
            }

            if (plane.size.x * plane.size.y < minPlaneSize)
            {
                print(plane.size.x * plane.size.y);
                plane.gameObject.SetActive(false);
            }
        }
    }


    private void RemoveOverlappingPlanes(List<ARPlane> updatedPlanes)
    {

        // Check for overlapping planes and remove the smaller ones
        for (int i = 0; i < updatedPlanes.Count; i++)
        {
            ARPlane plane1 = updatedPlanes[i];

            if (plane1 == null || plane1.gameObject.activeSelf == false)
            {
                continue;
            }

            for (int j = i + 1; j < updatedPlanes.Count; j++)
            {
                ARPlane plane2 = updatedPlanes[j];

                if (plane2 == null)
                {
                    continue;
                }

                if (plane1 == plane2) continue;

                if (plane2.gameObject.activeSelf == false) continue;

                if (ArePlanesOverlapping(plane1, plane2))
                {
                    // Remove the smaller plane
                    if (plane1.size.x * plane1.size.y < plane2.size.x * plane2.size.y)
                    {
                        plane1.gameObject.SetActive(false);
                    }
                    else
                    {
                        plane2.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    private bool ArePlanesOverlapping(ARPlane plane1, ARPlane plane2)
    {
        // Calculate the distance threshold in 2D space (excluding the Y axis)
        Vector2 center1 = new Vector2(plane1.center.x, plane1.center.z);
        Vector2 center2 = new Vector2(plane2.center.x, plane2.center.z);
        float distance2D = Vector2.Distance(center1, center2);

        // Check if the distance in 2D space is less than the threshold
        if (distance2D >= minPlaneSize) return false;

        // Calculate the distance threshold on the Z axis with a margin of 20%
        float yDiff = Mathf.Abs(plane1.center.y - plane2.center.y);
        float yThreshold = Mathf.Abs(plane1.center.y) * 0.2f;
        if (yDiff >= yThreshold) return false;

        return true;
    }




}
