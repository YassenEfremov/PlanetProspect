using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class GravityObject : MonoBehaviour {
    public bool isRocket;
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    Transform meshHolder;

    public Vector3 velocity { get; private set; }
    public float mass { get; private set; }
    [System.NonSerialized]
    public Rigidbody rb;

    void Awake () {
        rb = GetComponent<Rigidbody> ();
        rb.mass = mass;
        velocity = initialVelocity;
    }

    public void UpdateVelocity (GravityObject[] allBodies, float timeStep) {
        foreach (var otherBody in allBodies) {
            if (otherBody != this) {
                float sqrDst = (otherBody.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (otherBody.rb.position - rb.position).normalized;
                Vector3 acceleration = forceDir * YavorUniverse.gravitationalConstant * otherBody.mass / sqrDst;
                velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity (Vector3 acceleration, float timeStep) {
        velocity += acceleration * timeStep;
        
    }

    public void UpdatePosition (float timeStep) {
        rb.MovePosition(rb.position + velocity * timeStep);
        Quaternion rotation = Quaternion.LookRotation(velocity);
        rb.MoveRotation(rotation);
    }

    void OnValidate () {
        mass = surfaceGravity * radius * radius / YavorUniverse.gravitationalConstant;
//        meshHolder = transform.GetChild (0);
//        meshHolder.localScale = Vector3.one * radius;
    }

    public Rigidbody Rigidbody {
        get {
            return rb;
        }
    }

    public Vector3 Position {
        get {
            return rb.position;
        }
    }

}
