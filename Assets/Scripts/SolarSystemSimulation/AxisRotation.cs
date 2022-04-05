using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {

    private Universe universe;

    public float rotationPeriodDays;
    public float rotationPeriodHours;
    public float rotationPeriodMinutes;

    public float axisAngleOffset;
    public bool counterClockwiseRotation = true;

    // TODO: Don't serialize
    [System.NonSerialized]
    public float fullRotationsPerTimeStep;
    [System.NonSerialized]
    public float angleIncrementX;
    [System.NonSerialized]
    public float angleIncrementY;

    private float yAngle = 0;
    private float xAngle = 0;

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
        angleIncrementY = (360 / fullRotationsPerTimeStep) * Mathf.Sin(axisAngleOffset);
        angleIncrementX = (360 / fullRotationsPerTimeStep) * Mathf.Cos(axisAngleOffset);
    }

    // Update is called once per frame
    void FixedUpdate() {
        Rotate();
    }

    void Rotate() {
        // increment angle TODO: if yAngle > 360, set to 0 + leftover
        if (counterClockwiseRotation) {
            yAngle -= angleIncrementY * Universe.physicsTimeStep;
            xAngle -= angleIncrementX * Universe.physicsTimeStep;
        } else {
            yAngle += angleIncrementY * Universe.physicsTimeStep;
            xAngle += angleIncrementX * Universe.physicsTimeStep;
        }
        rb.MoveRotation(Quaternion.Euler(new Vector3(xAngle, yAngle, 0)));
    }
}
