using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


public class PlanetaryOrbit : MonoBehaviour {

    [System.Serializable]
    public struct KeplerianParameter {
        public double element;
        public double rate;
    }

    public KeplerianParameter semiMajorAxis;                    // AU       |       AU  / Cy        
    public KeplerianParameter eccentricity;                     // rad      |       rad / Cy        
    public KeplerianParameter inclination;                      // deg      |       deg / Cy        
    public KeplerianParameter meanLongitude;                    // deg      |       deg / Cy        
    public KeplerianParameter perihelionLongitude;              // deg      |       deg / Cy        
    public KeplerianParameter ascendingNodeLongitude;           // deg      |       deg / Cy

    private Universe universe;

    void Awake() {
        universe = FindObjectOfType<Universe> ();
    }

    void FixedUpdate() {
       transform.position = CalculateCoordinates(universe.julianCenturiesSinceEpoch);
    }

    // https://ssd.jpl.nasa.gov/planets/approx_pos.html
    public Vector3 CalculateCoordinates(double julianCenturiesSinceEpoch) {
        Vector3 coords = new Vector3();
        
        // TODO

        return coords;
    }
};

