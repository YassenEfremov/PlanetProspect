using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryToggle : MonoBehaviour
{
    public void ToggleTrajectory()
    {
        LineRenderer trajectory = gameObject.GetComponentInChildren<LineRenderer>();
        trajectory.enabled = !trajectory.isVisible;
    }
}
