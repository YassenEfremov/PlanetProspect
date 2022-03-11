using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
#if UNITY_IOS || UNITY_ANDROID

    //public bool Rotate;
    Plane Plane;


    private void Start()
    {
        Application.targetFrameRate = 60;       // Very important!
    }

    private void Update()
    {
        // Scroll
        if (Input.touchCount >= 1)
        {
            Plane.SetNormalAndPosition(Vector3.up, Vector3.zero);   // Update Plane
            var Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (Input.touchCount >= 2)
                {
                    // Scroll slower when zooming limit has been reached
                    gameObject.transform.Translate(Delta1 / 3, Space.World);
                }
                else
                {
                    gameObject.transform.Translate(Delta1, Space.World);
                }
            }
        }

        // Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            // Calculate zoom
            var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

            // edge case
            if (zoom == 0 || zoom > 10)
                return;

            // Limit camera zoom
            if (gameObject.transform.position.y >= 10)
            {
                if (zoom > 1)    // Allow only zooming in
                    gameObject.transform.position = Vector3.LerpUnclamped(Vector3.Lerp(pos1, pos2, 0.5f), gameObject.transform.position, 1 / zoom);
                return;
            }
            if (gameObject.transform.position.y <= 2)
            {
                if (zoom < 1)    // Allow only zooming out
                    gameObject.transform.position = Vector3.LerpUnclamped(Vector3.Lerp(pos1, pos2, 0.5f), gameObject.transform.position, 1 / zoom);
                return;
            }

            // Move the camera (1/zoom)% along the ray from pos1 to the camera
            gameObject.transform.position = Vector3.LerpUnclamped(Vector3.Lerp(pos1, pos2, 0.5f), gameObject.transform.position, 1 / zoom);

            //if (Rotate && pos2b != pos2)
            //    Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));
        }
    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        // Didn't moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = Camera.main.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.main.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        // Didn't hit the plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = Camera.main.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        // Didn't hit the plane
        return Vector3.zero;
    }

#endif
}
