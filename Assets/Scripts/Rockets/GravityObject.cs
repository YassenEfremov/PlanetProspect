using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Rigidbody))]
public class GravityObject : MonoBehaviour {
    public bool isActive;
    public bool isGravityAffected;
    public float radius;
    public float surfaceGravity;
    public Slider sliderX;
    public Slider sliderY;
    public Vector3 initialVelocity;

    public Vector3 velocity {get; set;}
    public float mass { get; private set; }
    
    // [System.NonSerialized]
    private Rigidbody rb;

    void Awake () {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        //velocity = initialVelocity;
    }

    private void Update()
    {
        if(!isActive)
        {
            if(sliderX != null || sliderY != null)
                velocity = new Vector3(sliderX.value, sliderY.value, 0);
        }
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
            if (isActive) {
                rb.position = value;
                rb.MovePosition(rb.position);
                Quaternion rotation = Quaternion.LookRotation(velocity);
                //rb.MoveRotation(rotation);
            }
        }
    }

    public void Launch()
    {
        isActive = true;
    }
}
