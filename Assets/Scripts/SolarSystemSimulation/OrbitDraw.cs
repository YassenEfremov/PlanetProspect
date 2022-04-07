using System;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class OrbitDraw : MonoBehaviour {

    private PlanetaryOrbit planetaryOrbit;
    private Universe universe;
    private LineRenderer lineRenderer;

    //public float lineWidth;
    public Material lineMaterial;
    public double dayIncrement;
    public uint nodeAmount;

    float currentCameraPrevPosZ;
    MainCameraController mainCameraController;
    MapCameraController mapCameraController;


    void Start() {
        planetaryOrbit = gameObject.GetComponentInChildren<PlanetaryOrbit>();
        universe = FindObjectOfType<Universe>();

        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        lineRenderer.material = lineMaterial;
        lineRenderer.startColor = lineRenderer.material.color;
        lineRenderer.endColor = lineRenderer.material.color;
        lineRenderer.enabled = true;

        DrawOrbit();
    }

    void Update() {
        // Update lines width on camera movement
        if (Camera.main.transform.position.z != currentCameraPrevPosZ) {
            if (Camera.main.name == "MainCamera") {
                if (mainCameraController == null) {
                    mainCameraController = Camera.main.GetComponent<MainCameraController>();
                }
                lineRenderer.widthMultiplier = Vector3.Distance(mainCameraController.planetToFollow.transform.position, Camera.main.transform.position) / 400;
            }
            else if (Camera.main.name == "MapCamera") {
                if (mapCameraController == null) {
                    mapCameraController = Camera.main.GetComponent<MapCameraController>();
                }
                lineRenderer.widthMultiplier = Vector3.Distance(mapCameraController.planetToFollow.transform.position, Camera.main.transform.position) / 400;
            }
            currentCameraPrevPosZ = Camera.main.transform.position.z;
        }
    }


    public void DrawOrbit() {
        Vector3[] drawPoints = new Vector3[nodeAmount];
        DateTime georgianDate = universe.georgianDate;

        // calculate points
        for (uint i = 0; i < nodeAmount; i++) {
            // increment date and convert to Julian centuries since J2000
            georgianDate = georgianDate.AddDays(dayIncrement);
            double julianDate = Universe.ToJulianDate(georgianDate);
            double julianCenturiesSinceEpoch = Universe.ToJulianCenturiesSinceEpoch(julianDate);
            // compute coords
            Vector3 velocity = new Vector3();
            planetaryOrbit.CalculateCoordinates(ref drawPoints[i], ref velocity, julianCenturiesSinceEpoch);
            drawPoints[i] = planetaryOrbit.CalculatePosition(drawPoints[i], velocity);
        }

        // draw lines
        lineRenderer.positionCount = drawPoints.Length;
        lineRenderer.SetPositions(drawPoints);
    }
}
