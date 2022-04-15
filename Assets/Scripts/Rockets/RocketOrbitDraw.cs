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
                position = body.transform.position;
                velocity = body.velocity == Vector3.zero ? body.initialVelocity : body.velocity;
                mass = body.mass;
                radius = body.radius;
            }
        }
    }

    private LineRenderer lineRenderer;
    private List<GravityObject> planets;
    private GravityObject realRocket;
    // private VirtualRocket rocket;
    private VirtualRocket rocket;
    // private Vector3 rocketVelocity;

    public uint nodeAmount = 1000;
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

        realRocket = gameObject.GetComponentInChildren<GravityObject>();
        rocket = new VirtualRocket(realRocket);

        /* line renderer settings */
        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        // lineRenderer.enabled = true;
        lineRenderer.material = lineMaterial;
        lineRenderer.material.color = Color.white;
        lineRenderer.startColor = lineMaterial.color;
        lineRenderer.endColor = lineMaterial.color;

    }

    // Update is called once per frame
    void FixedUpdate() {
        // create a new virtual rocket
        rocket = new VirtualRocket(realRocket);
        DrawOrbit();
    }

    void Update() {
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

    void DrawOrbit() {
        Vector3[] drawPoints = new Vector3[nodeAmount];

        // Update velocity and position
        for (uint i = 0; i < nodeAmount; i++) {
            drawPoints[i] = rocket.position;
            UpdateVelocity();
            UpdatePosition();
            // check for colliision and exit if so
            if (hasCollided(rocket, planets)) {
                break;
            }
        }

        lineRenderer.positionCount = drawPoints.Length;
        lineRenderer.SetPositions(drawPoints);
        // lineRenderer.widthMultiplier = lineWidth;
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
            float dist = Vector3.Distance(rocket.position, planet.transform.position);
            // take in account planet radius
            dist -= planet.radius + rocket.radius;
            // check for collision
            if (dist <= 0.01) {
                return true;
            }
        }

        return false;
    }
}
