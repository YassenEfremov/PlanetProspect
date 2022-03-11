using System.Collections;
using System.Collections.Generic;
using UnityEngine; 


public class PlanetaryOrbit : MonoBehaviour {

    [System.Serializable]
    public struct KeplerianParameter {
        public double element;
        public double rate;
    }

    public const float radToDeg = 180 / Mathf.PI;
    public const float degToRad = Mathf.PI / 180;

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
       transform.position = CalculateCoordinates(universe.julianCenturiesSinceEpoch) * universe.distanceScale;
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

        // Mathf.Sin and Cos functions take in radians. Convert some keplerian elements from deg to rad to save computation power
        argumentOfPerihelion *= degToRad;
        eccentricAnomaly *= degToRad;
        inclination *= degToRad;
        ascendingNodeLongitude *= degToRad;

        // calculate true anomaly
        float trueAnomaly = TrueAnomaly(eccentricity, eccentricAnomaly);

        // https://space.stackexchange.com/questions/19322/converting-orbital-elements-to-cartesian-state-vectors
        // calculate the radius vector and compute the position components X,Y,Z
        float radiusVector = semiMajorAxis * (1 - eccentricity * eccentricity) / (1 + eccentricity * Mathf.Cos(trueAnomaly));

        coords.x = radiusVector * (
                Mathf.Cos(ascendingNodeLongitude) * Mathf.Cos(trueAnomaly + argumentOfPerihelion)
                - Mathf.Sin(ascendingNodeLongitude) * Mathf.Sin(trueAnomaly + argumentOfPerihelion)
                * Mathf.Cos(inclination)
            );

        coords.y = radiusVector * (
                Mathf.Sin(ascendingNodeLongitude) * Mathf.Cos(trueAnomaly + argumentOfPerihelion)
                + Mathf.Cos(ascendingNodeLongitude) * Mathf.Sin(trueAnomaly + argumentOfPerihelion)
                * Mathf.Cos(inclination)
            );

        coords.z = radiusVector * Mathf.Sin(trueAnomaly + argumentOfPerihelion) * Mathf.Sin(inclination);

        return coords;
    }

    /*
     * Calculate the eccentric anomaly given the mean anomaly and the eccentricity
     * meanAnomaly = eccentricAnomaly - eccentricity * sin(eccentricAnomaly)
     */
    public static float KeplerEquation(float meanAnomaly, float eccentricity, uint precision) {

        float eccentricityInDeg = eccentricity * radToDeg;
        //E0
        float eccentricAnomaly = meanAnomaly + eccentricity * Mathf.Sin(meanAnomaly);

        for (uint i = 0; i < precision; i++) {
            float eccentricAnomalyInRad = eccentricAnomaly * degToRad;
            float deltaMeanAnomaly = meanAnomaly - (eccentricAnomaly - eccentricityInDeg * Mathf.Sin(eccentricAnomalyInRad));
            float deltaEccentricAnomaly = deltaMeanAnomaly / (1 - eccentricity * Mathf.Cos(eccentricAnomalyInRad));
            eccentricAnomaly += deltaEccentricAnomaly;
        }

        return eccentricAnomaly;

    }

    // Compute the true anomaly -> eccentricAnomaly should be in radians
    // jgiesen.de/Kepler
    // tan(TA) = (sqrt(1-e*e) * sin(E)) / (cos(E) - e)
    public static float TrueAnomaly(float eccentricity, float eccentricAnomaly) {
        float numerator = Mathf.Sqrt(1f - eccentricity * eccentricity) * Mathf.Sin(eccentricAnomaly);
        float denominator = Mathf.Cos(eccentricAnomaly) - eccentricity;

        return Mathf.Atan2(numerator, denominator);


    }
};

