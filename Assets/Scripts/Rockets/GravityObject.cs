using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class GravityObject : MonoBehaviour {
    public bool isGravityAffected;
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;

    public Vector3 velocity {get; set;}
    public float mass { get; private set; }
    
    // [System.NonSerialized]
    private Rigidbody rb;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        velocity = initialVelocity;
    }

    void OnValidate () {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
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

        set {
            rb.position = value;
            rb.MovePosition(rb.position);
            Quaternion rotation = Quaternion.LookRotation(velocity);
            rb.MoveRotation(rotation);
        }
    }
}
