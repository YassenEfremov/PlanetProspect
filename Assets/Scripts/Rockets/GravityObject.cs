using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Rigidbody))]
public class GravityObject : MonoBehaviour {
    public bool isActive;
    public bool isGravityAffected;
    public float radius;
// public float surfaceGravity; -> can be calculated from radius, mass and gravitational constant
    public float mass; // mass in earth masses

    [SerializeField] FixedJoystick joystick;
    [SerializeField] Slider slider;
    public Vector3 initialVelocity;

    public Vector3 velocity { get; set; }

    // [System.NonSerialized]
    private Rigidbody rb;
    private Universe universe;


    void Awake() {
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        try {
            Mesh mesh = GetComponent<MeshFilter>().mesh;
            Bounds bounds = mesh.bounds;
            radius = bounds.extents.x;
        } catch (MissingComponentException) {  // Rockets don't have a mesh filter
            radius = 0;
        }
        //velocity = initialVelocity;
        universe = FindObjectOfType<Universe>();
    }

    void Update() {
        if (!isActive && joystick != null) {
            velocity = new Vector3(joystick.Horizontal * slider.value, joystick.Vertical * slider.value, 0);
        }
    }

/*
    void OnValidate() {
        mass = surfaceGravity * radius * radius / Universe.gravitationalConstant;
    }
*/

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
                rb.MoveRotation(rotation);
            }
        }
    }

    public void Launch() {
        isActive = true;
    }
}
