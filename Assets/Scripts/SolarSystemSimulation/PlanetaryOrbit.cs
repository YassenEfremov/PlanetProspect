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
        
        // Compute values for the Kepler elements for the given (Julian) time
        float semiMajorAxis = (float)(this.semiMajorAxis.element + this.semiMajorAxis.rate * julianCenturiesSinceEpoch);
        float eccentricity = (float)(this.eccentricity.element + this.eccentricity.rate * julianCenturiesSinceEpoch);
        float inclination = (float)(this.inclination.element + this.inclination.rate * julianCenturiesSinceEpoch);
        float meanLongitude = (float)(this.meanLongitude.element + this.meanLongitude.rate * julianCenturiesSinceEpoch);
        float perihelionLongitude = (float)(this.perihelionLongitude.element + this.perihelionLongitude.rate * julianCenturiesSinceEpoch);
        float ascendingNodeLongitude = (float)(this.ascendingNodeLongitude.element + this.ascendingNodeLongitude.rate * julianCenturiesSinceEpoch);

        // Compute the argument of perihelion and the mean anomaly
        float argumentOfPerihelion = perihelionLongitude - ascendingNodeLongitude;
        float meanAnomaly = meanLongitude - perihelionLongitude;

        // Modulus the mean anomaly so that -180 <= M <= +180
        while (meanAnomaly > 180) {
            meanAnomaly -= 360;
        }

        // Solve Kepler's equation (Newton's way) to get the eccentricity anomaly
        float eccentricAnomaly = KeplerEquation(meanAnomaly, eccentricity, 5);

        return coords;
    }

    /*
     * Calculate the eccentric anomaly given the mean anomaly and the eccentricity
     * meanAnomaly = eccentricAnomaly - eccentricity * sin(eccentricAnomaly)
     */
    public static float KeplerEquation(float meanAnomaly, float eccentricity, uint precision) {
        //E0
        float eccentricAnomaly = meanAnomaly + eccentricity * Mathf.Sin(meanAnomaly);

        for (uint i = 0; i < precision; i++) {
            float deltaMeanAnomaly = meanAnomaly - (eccentricAnomaly - eccentricity * Mathf.Sin(eccentricAnomaly));
            float deltaEccentricAnomaly = deltaMeanAnomaly / (1 - eccentricity * Mathf.Cos(eccentricAnomaly));
            eccentricAnomaly += deltaEccentricAnomaly;
        }

        return eccentricAnomaly;

    }
};

