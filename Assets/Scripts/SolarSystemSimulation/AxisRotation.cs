using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {

    private Universe universe;

    public float rotationPeriodDays;
    public float rotationPeriodHours = 24;
    public float rotationPeriodMinutes;

    // TODO: Don't serialize
    // [System.NonSerialized]
    public float fullRotationsPerTimeStep;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rotationPeriodHours += rotationPeriodDays * 24;
        rotationPeriodMinutes += rotationPeriodHours * 60;

        universe = FindObjectOfType<Universe>();
        fullRotationsPerTimeStep = universe.minuteTimeStep / rotationPeriodMinutes;

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        Rotate();
    }

    void Rotate() {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * fullRotationsPerTimeStep * Universe.physicsTimeStep));
    }
}
