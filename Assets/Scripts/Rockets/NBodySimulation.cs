using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NBodySimulation : MonoBehaviour {
    GravityObject[] bodies;
    static NBodySimulation instance;

    void Awake() {
        bodies = FindObjectsOfType<GravityObject> ();
        Time.fixedDeltaTime = Universe.physicsTimeStep;
        Debug.Log ("Setting fixedDeltaTime to: " + Universe.physicsTimeStep);
    }

    void FixedUpdate() {
        for (int i = 0; i < bodies.Length; i++) {
            if (bodies[i].isGravityAffected && bodies[i].isActive) {
                bodies[i].velocity = CalculateVelocity(bodies, bodies[i]);
            }
        }

        for (int i = 0; i < bodies.Length; i++) {
            if (bodies[i].isGravityAffected && bodies[i].isActive) {
                bodies[i].Position = CalculatePosition(bodies[i].Position, bodies[i].velocity);
            }
        }
    }

    public static GravityObject[] Bodies {
        get {
            return Instance.bodies;
        }
    }

    public static Vector3 CalculateVelocity(GravityObject[] allBodies, GravityObject target) {
        Vector3 velocity = target.velocity; 

        foreach (GravityObject body in allBodies) {
            if (body != target) {
                float sqrDst = (body.Position - target.Position).sqrMagnitude;
                Vector3 forceDir = (body.Position - target.Position).normalized;
                Vector3 acceleration = forceDir * Universe.gravitationalConstant * body.mass / sqrDst;
                velocity += acceleration * Universe.physicsTimeStep;
            }
        }

        return velocity;
    }

    public static Vector3 CalculatePosition(Vector3 position, Vector3 velocity) {
        return position + velocity * Universe.physicsTimeStep;
    }

    public static Quaternion CalculateRotation(Vector3 velocity) {
        return Quaternion.LookRotation(velocity);
    }

    static NBodySimulation Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<NBodySimulation> ();
            }
            return instance;
        }
    }
}
