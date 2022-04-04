using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof(Rigidbody))]
public class PlanetaryOrbit : MonoBehaviour {

    [System.Serializable]
    public struct KeplerianParameter {
        public double element;
        public double rate;
    }

    /* 
     * Additional Terms used for computing Jupiter-Neptune Orbits
     * I have no idea where these arguments mean or come from, although they have to be used
     * [table 2b] https://ssd.jpl.nasa.gov/planets/approx_pos.html
     */
    [System.Serializable]
    public struct AdditionalTerms {
        public double b;
        public double c;
        public double s;
        public double f;
    }

    public const float radToDeg = 180 / Mathf.PI;
    public const float degToRad = Mathf.PI / 180;

    public double gravitationalParameter;

    public KeplerianParameter semiMajorAxis;                    // AU       |       AU  / Cy        
    public KeplerianParameter eccentricity;                     // rad      |       rad / Cy        
    public KeplerianParameter inclination;                      // deg      |       deg / Cy        
    public KeplerianParameter meanLongitude;                    // deg      |       deg / Cy        
    public KeplerianParameter perihelionLongitude;              // deg      |       deg / Cy        
    public KeplerianParameter ascendingNodeLongitude;           // deg      |       deg / Cy
    public AdditionalTerms additionalTerms;

    private Universe universe;
    private Vector3 position = new Vector3();
    private Vector3 velocity = new Vector3();
    private Rigidbody rb;
    private Quaternion rotation;


    void Awake() {
        rb = GetComponent<Rigidbody>();
        universe = FindObjectOfType<Universe>();
        rotation = rb.rotation;
    }

    void FixedUpdate() {
        CalculateCoordinates(ref position, ref velocity, universe.julianCenturiesSinceEpoch);
        rb.MovePosition(CalculatePosition(position, velocity));
        rotation = CalculateRotation(velocity, rotation);
        // rb.MoveRotation(rotation);
        //if (gameObject.name == "Earth")
        //{
        //    Debug.Log((position * universe.distanceScale) + velocity * Universe.physicsTimeStep);
        //    Debug.Log("transform: " + transform.position);
        //}
    }


    // https://ssd.jpl.nasa.gov/planets/approx_pos.html
    public void CalculateCoordinates(ref Vector3 position, ref Vector3 velocity, double julianCenturiesSinceEpoch) {
        position = new Vector3();
        
        // Compute values for the Kepler elements for the given (Julian) time
        float semiMajorAxis = (float)(this.semiMajorAxis.element + this.semiMajorAxis.rate * julianCenturiesSinceEpoch);
        float eccentricity = (float)(this.eccentricity.element + this.eccentricity.rate * julianCenturiesSinceEpoch);
        float inclination = (float)(this.inclination.element + this.inclination.rate * julianCenturiesSinceEpoch);
        float meanLongitude = (float)(this.meanLongitude.element + this.meanLongitude.rate * julianCenturiesSinceEpoch);
        float perihelionLongitude = (float)(this.perihelionLongitude.element + this.perihelionLongitude.rate * julianCenturiesSinceEpoch);
        float ascendingNodeLongitude = (float)(this.ascendingNodeLongitude.element + this.ascendingNodeLongitude.rate * julianCenturiesSinceEpoch);

        // Compute the argument of perihelion and the mean anomaly
        float argumentOfPerihelion = perihelionLongitude - ascendingNodeLongitude;
        float meanAnomaly = (float)(meanLongitude - perihelionLongitude
            + additionalTerms.b * julianCenturiesSinceEpoch * julianCenturiesSinceEpoch
            + additionalTerms.c * Mathf.Cos((float)(additionalTerms.f * julianCenturiesSinceEpoch))
            + additionalTerms.s * Mathf.Sin((float)(additionalTerms.f * julianCenturiesSinceEpoch))
            );

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

        position.x = radiusVector * (
                Mathf.Cos(ascendingNodeLongitude) * Mathf.Cos(trueAnomaly + argumentOfPerihelion)
                - Mathf.Sin(ascendingNodeLongitude) * Mathf.Sin(trueAnomaly + argumentOfPerihelion)
                * Mathf.Cos(inclination)
            );

        position.y = radiusVector * (
                Mathf.Sin(ascendingNodeLongitude) * Mathf.Cos(trueAnomaly + argumentOfPerihelion)
                + Mathf.Cos(ascendingNodeLongitude) * Mathf.Sin(trueAnomaly + argumentOfPerihelion)
                * Mathf.Cos(inclination)
            );

        position.z = radiusVector * Mathf.Sin(trueAnomaly + argumentOfPerihelion) * Mathf.Sin(inclination);

        // Compute angular momentum and calculate velocity vectors
        float p = semiMajorAxis * (1 - eccentricity * eccentricity);  // This will be used a lot - Calculate here to save computation power
        float angularMomentum = Mathf.Sqrt((float)(gravitationalParameter) * p);

        velocity = new Vector3();

        velocity.x = (position.x * angularMomentum * eccentricity / (radiusVector * p)) * Mathf.Sin(trueAnomaly)
                - (angularMomentum / radiusVector) * Mathf.Cos(ascendingNodeLongitude) * Mathf.Sin(argumentOfPerihelion + trueAnomaly)
                + Mathf.Sin(ascendingNodeLongitude) * Mathf.Cos(argumentOfPerihelion + trueAnomaly) * Mathf.Cos(inclination);

        velocity.y = (position.y * angularMomentum * eccentricity / (radiusVector * p)) * Mathf.Sin(trueAnomaly)
                - (angularMomentum / radiusVector) * Mathf.Sin(ascendingNodeLongitude) * Mathf.Sin(argumentOfPerihelion + trueAnomaly)
                - Mathf.Cos(argumentOfPerihelion) * Mathf.Cos(argumentOfPerihelion * trueAnomaly) * Mathf.Cos(inclination);

        velocity.z = (position.z * angularMomentum * eccentricity / (radiusVector * p )) * Mathf.Sin(trueAnomaly)
            + (angularMomentum * radiusVector) * Mathf.Cos(argumentOfPerihelion + trueAnomaly) * Mathf.Sin(inclination);
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

    public static Quaternion CalculateRotation(Vector3 angularVelocity, Quaternion currentRotation) {
        return currentRotation * Quaternion.Euler(angularVelocity * Universe.physicsTimeStep);
    }

    public Vector3 CalculatePosition(Vector3 position, Vector3 velocity) {
        return (position * universe.distanceScale) + velocity * Universe.physicsTimeStep;
    }
};

