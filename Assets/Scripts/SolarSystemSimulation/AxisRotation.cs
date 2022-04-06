using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {

    private Universe universe;

    public float rotationPeriodDays;
    public float rotationPeriodHours;
    public float rotationPeriodMinutes;

    public float axisAngle;
    private float angleIncrement;

    public bool counterClockwiseRotation = true;

    // TODO: Don't serialize
    [System.NonSerialized]
    public float fullRotationsPerTimeStep;
    [System.NonSerialized]
    public Vector3 axis;
    private Rigidbody rb;
    private Quaternion deltaRotation;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        // get minutes required for a full rotation
        rotationPeriodHours += rotationPeriodDays * 24;
        rotationPeriodMinutes += rotationPeriodHours * 60;

        // how many full rotations happen every time step
        universe = FindObjectOfType<Universe>();
        fullRotationsPerTimeStep = rotationPeriodMinutes / universe.minuteTimeStep;

        // angle increase per time step where a full rotation happens every 360 degrees
        angleIncrement = (360 / fullRotationsPerTimeStep);
        // calculate axis of rotation
        axis = new Vector3(0, Mathf.Cos(Mathf.Deg2Rad * axisAngle), Mathf.Sin(Mathf.Deg2Rad * axisAngle));

        if (counterClockwiseRotation) {
            angleIncrement *= -1;
        }

        deltaRotation = Quaternion.AngleAxis(angleIncrement * Universe.physicsTimeStep, axis);
    }

    void FixedUpdate() {
        Rotate();
    }

    public void Rotate() {
        // transform.Rotate(axis, angleIncrement * Universe.physicsTimeStep, Space.World);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
