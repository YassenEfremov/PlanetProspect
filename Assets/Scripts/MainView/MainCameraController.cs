using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MainCameraController : MonoBehaviour
{
    Plane plane;
    bool touchedUI = false;
    //float cameraDistance = -1;
    //Vector3 zoomedCameraPos;
    Vector3 planetPreviousPos = Vector3.zero;
    Vector3 planetOffset = Vector3.zero;

    [SerializeField] bool rotate;
    public GameObject planetToFollow;

    float MIN_ZOOM;
    float MAX_ZOOM = 10000f;


    void Awake()
    {
        Application.targetFrameRate = 60;       // Very important!

        MIN_ZOOM = planetToFollow.transform.lossyScale.x * 2;
    }

    void Start()
    {
        //gameObject.transform.position = new Vector3(0, -5, -5);     // Default position
        //if (PlayerPrefs.HasKey("x"))
        //    // Load the last camera position
        //    gameObject.transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"), PlayerPrefs.GetFloat("z"));

        //gameObject.transform.parent.position = planetToFollow.transform.position;
        //gameObject.transform.position = new Vector3(planetToFollow.transform.position.x, planetToFollow.transform.position.y - 1, planetToFollow.transform.position.z - 1);
        StartCoroutine(waitThenFocusPlanet());
    }

    public void Update()
    {
        //float distance = Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position);
        //if (cameraDistance == -1)
        //    cameraDistance = distance;
        //gameObject.transform.position = Vector3.LerpUnclamped(planetToFollow.transform.position, gameObject.transform.position, cameraDistance / distance);
        if(planetPreviousPos == Vector3.zero)
        {
            planetPreviousPos = planetToFollow.transform.position;
        }
        planetOffset = planetToFollow.transform.position - planetPreviousPos;
        gameObject.transform.position += planetOffset;
        planetPreviousPos = planetToFollow.transform.position;

        // If we click on an interactable UI element => don't move the camera
        if (Input.touchCount >= 1 && IsPointerOverUIObject() && Input.GetTouch(0).phase == TouchPhase.Began)
            touchedUI = true;

        // Pinch
        if (Input.touchCount >= 2 && !touchedUI)
        {
            plane.SetNormalAndPosition(Vector3.back, Vector3.zero);   // Create reference plane
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
            //cameraDistance = Vector3.Distance(planetToFollow.transform.position, zoomedCameraPos);

            // SHOULD BE REWOKED IF WE WANT TO USE IT!!
            if (rotate && touch2Delta != touch2)
                Camera.main.transform.RotateAround(planetToFollow.transform.position, plane.normal, Vector3.SignedAngle(touch2 - touch1, touch2Delta - touch1Delta, plane.normal));
        }

        if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
            touchedUI = false;
    }

    //void OnApplicationPause(bool pauseStatus)
    //{
    //    if (pauseStatus)
    //    {
    //        // Save the camera position
    //        PlayerPrefs.SetFloat("x", gameObject.transform.position.x);
    //        PlayerPrefs.SetFloat("y", gameObject.transform.position.y);
    //        PlayerPrefs.SetFloat("z", gameObject.transform.position.z);
    //        PlayerPrefs.Save();
    //    }
    //}

    IEnumerator waitThenFocusPlanet()
    {
        yield return new WaitForSeconds(0.01f);
        gameObject.transform.position = new Vector3(planetToFollow.transform.position.x, planetToFollow.transform.position.y - 1, planetToFollow.transform.position.z - 1);
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
        if (plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        // Didn't hit the plane
        return Vector3.zero;
    }
}