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
    // [System.NonSerialized]
    public float angleIncrement;

    private float yAngle = 0;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();

        rotationPeriodHours += rotationPeriodDays * 24;
        // get minutes required for a full rotation
        rotationPeriodMinutes += rotationPeriodHours * 60;

        universe = FindObjectOfType<Universe>();
        // how many full rotations happen every time step
        fullRotationsPerTimeStep = rotationPeriodMinutes / universe.minuteTimeStep;

        // angle increase per time step where a full rotation happens every 360 degrees
        angleIncrement = 360 / fullRotationsPerTimeStep;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Rotate();
    }

    void Rotate() {
        // increment angle TODO: if yAngle > 360, set to 0 + leftover
        yAngle += angleIncrement * Universe.physicsTimeStep;
        rb.MoveRotation(Quaternion.Euler(new Vector3(0, yAngle, 0)));
    }
}
