using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class OrbitDraw : MonoBehaviour {

    private PlanetaryOrbit planetaryOrbit;
    private Universe universe;
    private LineRenderer lineRenderer;

    public float lineWidth;
    public double dayIncrement;
    public uint nodeAmount;

    void Start() {
        planetaryOrbit = gameObject.GetComponentInChildren<PlanetaryOrbit>();
        universe = FindObjectOfType<Universe>();

        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        lineRenderer.enabled = true;
    }

    void Update() {
        DrawOrbit();
    }

    void DrawOrbit() {
        Vector3[] drawPoints = new Vector3[nodeAmount];
        DateTime georgianDate = universe.georgianDate;

        // calculate points
        for (uint i = 0; i < nodeAmount; i++) {
            // increment date and convert to Julian centuries since J2000
            georgianDate = georgianDate.AddDays(dayIncrement);
            double julianDate = Universe.ToJulianDate(georgianDate);
            double julianCenturiesSinceEpoch = Universe.ToJulianCenturiesSinceEpoch(julianDate);
            // compute coords
            drawPoints[i] = planetaryOrbit.CalculateCoordinates(julianCenturiesSinceEpoch) * universe.distanceScale;
        }

        // draw lines
        lineRenderer.positionCount = drawPoints.Length;
        lineRenderer.SetPositions(drawPoints);
        lineRenderer.widthMultiplier = lineWidth;
    }


}