using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainCameraController : MonoBehaviour
{
    float MIN_ZOOM;
    float MAX_ZOOM = 10000f;

    Plane referencePlane;
    bool touchedUI = false;
    Vector3 planetPreviousPos = Vector3.zero;
    Vector3 cameraSavedPos;

    [SerializeField] bool rotate;
    public GameObject planetToFollow;


    void Awake()
    {
        Application.targetFrameRate = 60;       // Very important!
    }

    void Start()
    {
        //// TEMPORARY! UNTIL WE HAVE A BACKEND TO GET THE LAST POS FROM
        //cameraSavedPos = new Vector3(planetToFollow.transform.position.x,
        //                            planetToFollow.transform.position.y - planetToFollow.transform.lossyScale.x * 5,
        //                            planetToFollow.transform.position.z - planetToFollow.transform.lossyScale.x * 5);

        cameraSavedPos = new Vector3(planetToFollow.transform.position.x,
                            planetToFollow.transform.position.y - planetToFollow.transform.lossyScale.x * 5,
                            planetToFollow.transform.position.z - planetToFollow.transform.lossyScale.x * 5);
        gameObject.transform.position = cameraSavedPos;

        focusPlanet();
    }

    public void Update()
    {
        // Move the camera along with the planetToFollow
        if (planetPreviousPos == Vector3.zero)
            planetPreviousPos = planetToFollow.transform.position;

        Vector3 planetOffset = planetToFollow.transform.position - planetPreviousPos;
        gameObject.transform.position += planetOffset;
        planetPreviousPos = planetToFollow.transform.position;

        referencePlane.SetNormalAndPosition(gameObject.transform.forward, planetToFollow.transform.position);   // Update reference plane

        // If we click on an interactable UI element => don't move the camera
        if (Input.touchCount >= 1 && IsPointerOverUIObject() && Input.GetTouch(0).phase == TouchPhase.Began)
            touchedUI = true;

        // Rotate
        if(Input.touchCount >= 1 && !touchedUI)
        {
            var touch1 = PlanePosition(Input.GetTouch(0).position);
            var touch1Delta = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            //var touch3rd = PlanePosition(new Vector2(Input.GetTouch(0).position.x, (Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition).y));

            if (rotate && touch1Delta != touch1)
            {
                // Rotate around the vertical axis
                gameObject.transform.RotateAround(planetToFollow.transform.position, Vector3.back, Vector3.SignedAngle(
                    touch1Delta - planetToFollow.transform.position, touch1 - planetToFollow.transform.position, referencePlane.normal
                ));
                //// Rotate around the camera's horizontal axis
                //gameObject.transform.RotateAround(planetToFollow.transform.position, gameObject.transform.right, Vector3.SignedAngle(
                //    touch1 - planetToFollow.transform.position, touch3rd - planetToFollow.transform.position, referencePlane.normal
                //));
            }
        }

        // Pinch
        if (Input.touchCount >= 2 && !touchedUI)
        {
            var touch1 = PlanePosition(Input.GetTouch(0).position);
            var touch2 = PlanePosition(Input.GetTouch(1).position);
            var touch1Delta = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var touch2Delta = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            // Calculate zoom
            var zoom = Vector3.Distance(touch1, touch2) / Vector3.Distance(touch1Delta, touch2Delta);

            // edge case
            if (zoom == 0 || zoom > 10)
                return;

            // Limit camera zoom
            if (-gameObject.transform.position.z >= MAX_ZOOM)
            {
                if (zoom > 1)    // Allow only zooming in
                    gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1 / zoom);
            }
            else if (Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position) <= MIN_ZOOM)
            {
                if (zoom < 1)    // Allow only zooming out
                    gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1 / zoom);
            }
            else
            {
                gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1 / zoom);
            }
        }

        if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            touchedUI = false;
    }


    public void focusPlanet()
    {
        //Ray cameraRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        //plane.SetNormalAndPosition(Vector3.back, planetToFollow.transform.position);   // Update reference plane
        //plane.Raycast(cameraRay, out var rayEnter);
        //Vector3 offset = planetToFollow.transform.position - cameraRay.GetPoint(rayEnter);
        ////Debug.Log(rayEnter);
        //gameObject.transform.position += offset;

        //plane.SetNormalAndPosition(Vector3.back, planetToFollow.transform.position);   // Update reference plane
        MIN_ZOOM = planetToFollow.transform.lossyScale.x;   // Update minimum zoom distance
        float distance = Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position);
        gameObject.transform.position = Vector3.Lerp(planetToFollow.transform.position, gameObject.transform.position,
                                                     planetToFollow.transform.lossyScale.x * 5 / distance);
    }

    bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        if (results.Count > 0)
        {
            // Go through all of the parent transforms of the UI element until we find one that is tagged as "Interactable"
            Transform interactable = results[0].gameObject.transform;
            while (interactable != null)
            {
                if (interactable.tag.Equals("Interactable"))
                    return true;

                interactable = interactable.parent;
            }
        }
        return false;
    }

    Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = Camera.main.ScreenPointToRay(screenPos);
        if (referencePlane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        // Didn't hit the plane
        return Vector3.zero;
    }

    public void zoomIn()
    {
        if (-gameObject.transform.position.z >= MAX_ZOOM)
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 0.99f);
        }
        else if (Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position) <= MIN_ZOOM)
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1.01f);
        }
        else
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 0.8f);
        }
    }

    public void zoomOut()
    {
        if (-gameObject.transform.position.z >= MAX_ZOOM)
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 0.99f);
        }
        else if (Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position) <= MIN_ZOOM)
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1.01f);
        }
        else
        {
            gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, 1.2f);
        }
    }
}