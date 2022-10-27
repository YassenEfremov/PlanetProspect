using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class RocketOrbitDraw : MonoBehaviour {

    // Class used to simulate rocket positions in advance
    class VirtualRocket {
        public Vector3 position;
        public Vector3 velocity;
        public float mass;
        public float radius;

        // Rockets shouldn't affect each other gravity?
        // public float surfaceGravity;

        public VirtualRocket(GravityObject body) {
            if (body.isGravityAffected) {
                position = body.Position;
                velocity = body.velocity == Vector3.zero ? body.initialVelocity : body.velocity;
                mass = body.mass;
                radius = body.radius;
            }
        }
    }

    private Universe universe;
    private LineRenderer lineRendererStart lineRendererEnd;
    private List<GravityObject> planets;
    private GravityObject realRocket;
    // private VirtualRocket rocket;
    private VirtualRocket rocket;
    // private Vector3 rocketVelocity;

    public uint nodeAmount = 1000;

    private int orbitArrStart = 0;
    private int orbitArrEnd = nodeAmount - 1;
    private DateTime = lastOrbitPredDate; 

    private PlanetaryOrbit;

    private Vector3[] drawPoints = new Vector3[nodeAmount];

    public float timeStep = 0.1f;
    // public float lineWidth = 0.5f;
    public Material lineMaterial;

    float currentCameraPrevPosZ;
    MainCameraController mainCameraController;
    MapCameraController mapCameraController;


    // Start is called before the first frame update
    void Start() {
        // list of objects that will affect the rocket's gravity
        planets = new List<GravityObject>();
        foreach (GravityObject body in FindObjectsOfType<GravityObject>()) {
            // only planets aren't affected by gravity, because their orbit is calculated without it
            if(!body.isGravityAffected) {
                planets.Add(body);
            }
        }

        universe = FindObjectOfType<Universe>();
        lastOrbitPredDate = universe.georgianDate;
        planetaryOrbit = gameObject.GetComponentInChildren<PlanetaryOrbit>();
        realRocket = gameObject.GetComponentInChildren<GravityObject>();
        rocket = new VirtualRocket(realRocket);

        /* line renderer settings */
        lineRendererStart = gameObject.GetComponentInChildren<LineRenderer>();
        // lineRendererStart.enabled = true;
        lineRendererStart.material = lineMaterial;
        lineRendererStart.material.color = Color.white;
        lineRendererStart.startColor = lineMaterial.color;
        lineRendererStart.endColor = lineMaterial.color;

        lineRendererEnd = Instantiate(lineRendererStart);
    }

    // Update is called once per frame
    void FixedUpdate() {
        DrawOrbit();
    }

    void Update() {
        if (Camera.main.transform.position.z != currentCameraPrevPosZ) {
            if (Camera.main.name == "MainCamera") {
                if (mainCameraController == null) {
                    mainCameraController = Camera.main.GetComponent<MainCameraController>();
                }
                lineRendererStart.widthMultiplier = Vector3.Distance(mainCameraController.planetToFollow.transform.position, Camera.main.transform.position) / 400;
            }
            else if (Camera.main.name == "MapCamera") {
                if (mapCameraController == null) {
                    mapCameraController = Camera.main.GetComponent<MapCameraController>();
                }
                lineRendererStart.widthMultiplier = Vector3.Distance(mapCameraController.planetToFollow.transform.position, Camera.main.transform.position) / 400;
            }
            currentCameraPrevPosZ = Camera.main.transform.position.z;
        }
    }

    void DrawOrbit() {
        // find current rocket pos index
        while (drawPoints[i] != DrawOrbit[orbitArrStart]) {
            orbitArrStart = (orbitArrStart + 1) % 1000;
        }

        // add new rocket positions
        while (orbitArrStart + orbitArrEnd != nodeAmount - 1) {
            if (hasCollided(rocket, planets)) {
                break;
            }

            Universe.UpdateGeorgianDate(ref georgianDate);
            UpdateVelocity();
            UpdatePosition();
            orbitArrEnd = (orbitArrEnd + 1) % 1000;
            drawPoints[orbitArrEnd] = rocket.position;
        }

        lineRendererStart.positionCount = drawPoints.Length - orbitArrStart;
        lineRendererStart.SetPositions(drawPoints[orbitArrStart]);
        lineRendererStart.widthMultiplier = lineWidth;

        lineRendererEnd.positionCount = drawPoints.Length - lineRendererStart.positionCount;
        lineRendererEnd.SetPositions(drawPoints[orbitArrEnd]);
        lineRendererEnd.widthMultiplier = lineWidth;

    }

    void UpdateVelocity(DateTime georgianDate) {
        double julianDateSinceEpoch = Universe.ToJulianCenturiesSinceEpoch(Universe.ToJulianDate(georgianDate));
        Vector3 planetPos;
        Vector3 planetVel;

        foreach (GravityObject planet in planets) {
            planetaryOrbit.CalculateCoordinates(ref planetPos, ref planetVel, julianDateSinceEpoch);


            float sqrDst = (planetPos - rocket.position).sqrMagnitude;

            Vector3 forceDir = (planetPos - rocket.position).normalized;
            Vector3 acceleration = forceDir * Universe.gravitationalConstant * planet.mass / sqrDst;
            rocket.velocity += acceleration * timeStep;
        }
    }

    void UpdateVelocity() {
        foreach (GravityObject planet in planets) {
            float sqrDst = (planet.Position - rocket.position).sqrMagnitude;
            Vector3 forceDir = (planet.Position - rocket.position).normalized;
            Vector3 acceleration = forceDir * Universe.gravitationalConstant * planet.mass / sqrDst;
            rocket.velocity += acceleration * timeStep;
        }
    }

    void UpdatePosition() {
        rocket.position += rocket.velocity * timeStep;
    }

    /*
     * Check if a virtual rocket is in an object
     * TODO: Include other rockets to collisionObjects
     */
    bool hasCollided(VirtualRocket rocket, List<GravityObject> collisionObjects) {
        // iterate over all objects
        foreach (GravityObject planet in collisionObjects) {
            if (planet.name == "Earth") {
                continue;
            }
            // distance between rocket and planet
            float dist = Vector3.Distance(rocket.position, planet.Position);
            // take in account planet radius
            dist -= (planet.radius * 15) + rocket.radius;  // Multiply by 15 because planets are extremely small and with current physicTimeStep control it's virtually impossible to hit a planet
            // check for collision
            if (dist <= 0.01) {
                return true;
            }
        }

        return false;
    }
}
