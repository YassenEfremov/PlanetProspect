using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class MapCameraController : MonoBehaviour {
    float MIN_ZOOM;
    float MAX_ZOOM = 10000f;
    //float MAX_DIST_AWAY = 200f;

    Plane plane;
    bool touchedUI = false;
    [HideInInspector] public bool focused = true;
    Vector3 planetPreviousPos = Vector3.zero;
    Vector3 cameraSavedPos;

    [SerializeField] bool rotate;
    public GameObject planetToFollow;

    /* Mouse Zoom Support */
    public Vector3 scrollZoomAxis;  // The axis that should be affect. Using Z is recommended (0, 0, z)
    public float scrollSpeed;  // if == 1, each scroll dist multiplies
    /**********************/

    void Awake() {
        Application.targetFrameRate = 60;       // Very important!

        MIN_ZOOM = planetToFollow.transform.lossyScale.x * 10;
    }

    void Start() {
        // TEMPORARY! UNTIL WE HAVE A BACKEND TO GET THE LAST POS FROM cameraSavedPos = new Vector3(planetToFollow.transform.position.x,
        cameraSavedPos = new Vector3(planetToFollow.transform.position.x,
                                    planetToFollow.transform.position.y,
                                    planetToFollow.transform.position.z - planetToFollow.transform.lossyScale.x * 5);

        gameObject.transform.position = cameraSavedPos;
    }

    public void Update() {
        // If the camera hasn't been moved by the player => move the camera along with the planetToFollow
        if (focused) {
            if (planetPreviousPos == Vector3.zero) {
                planetPreviousPos = planetToFollow.transform.position;
            }

            Vector3 planetOffset = planetToFollow.transform.position - planetPreviousPos;
            gameObject.transform.position += planetOffset;
            planetPreviousPos = planetToFollow.transform.position;
        }

        // If we click on an interactable UI element => don't move the camera
        if (Input.touchCount >= 1 && IsPointerOverUIObject() && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchedUI = true;
        }

        // Are there touchscreen controls?
        if (Input.touchSupported) {
            // Pinch
            if (Input.touchCount >= 2 && !touchedUI) {
                var pos1 = PlanePosition(Input.GetTouch(0).position);
                var pos2 = PlanePosition(Input.GetTouch(1).position);
                var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
                var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

                // Calculate the point between the two touches based on their movement speed => makes the zooming a lot nicer
                if ((Vector3.Distance(pos1, pos1b) + Vector3.Distance(pos2, pos2b)) == 0) {
                    return;
                }

                float posAsPercentOfTotalDistance = Vector3.Distance(pos1, pos1b) / (Vector3.Distance(pos1, pos1b) + Vector3.Distance(pos2, pos2b));
                Vector3 posToZoom = Vector3.Lerp(pos1b, pos2b, posAsPercentOfTotalDistance);

                // Calculate zoom
                var zoom = Vector3.Distance(pos1, pos2) / Vector3.Distance(pos1b, pos2b);

                // edge case
                if (zoom == 0 || zoom > 10) {
                    return;
                }

                // Limit camera zoom
                if (-gameObject.transform.position.z >= MAX_ZOOM) {
                    if (zoom > 1) {    // Allow only zooming in
                        gameObject.transform.position = Vector3.LerpUnclamped(posToZoom, gameObject.transform.position, 1 / zoom);
                    }
                } else if (Vector3.Distance(planetToFollow.transform.position, gameObject.transform.position) <= MIN_ZOOM) {
                    if (zoom < 1) {    // Allow only zooming out
                        gameObject.transform.position = Vector3.LerpUnclamped(posToZoom, gameObject.transform.position, 1 / zoom);
                    }
                } else  {
                    // Move the camera (1/zoom)% along the ray from pos1 to the camera
                    gameObject.transform.position = Vector3.LerpUnclamped(posToZoom, gameObject.transform.position, 1 / zoom);
                }

                // SHOULD BE REWOKED IF WE WANT TO USE IT!!
                if (rotate && pos2b != pos2) {
                    gameObject.transform.RotateAround(pos1, plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, plane.normal));
                }
            }

            /*
            Limit the camera movement within a circle around the current planet
            if (Vector2.Distance(planetToFollow.transform.position, gameObject.transform.position) > MAX_DIST_AWAY)
            {
                Vector2 limitedDistance = Vector2.Lerp(planetToFollow.transform.position, gameObject.transform.position,
                                                       MAX_DIST_AWAY / Vector2.Distance(planetToFollow.transform.position, gameObject.transform.position));
                gameObject.transform.position = new Vector3(limitedDistance.x, limitedDistance.y, gameObject.transform.position.z);
            }
            */

        } else { // PC zoom controls (mouse scroll wheel)
            /* 
             * 0 if no scroll, positive num if scroll, negative num if backwards scroll 
             * Scroll somewhat depends on mouse hardware
             * I get 0.5 for a single scroll and -0.5. Assuming it's the same for all mice, we could interpret it as 1 by multiplying by 2
             */
            float scroll = Input.GetAxis("Mouse ScrollWheel") * 2;

            // Don't want to change the initial scrollZoomAxis;
            Vector3 zoomAxis = scrollZoomAxis;
            zoomAxis *= GetZoomMultiplierOverDistance(planetToFollow.transform.position.z, gameObject.transform.position.z, scrollSpeed);

            // how much distance the camera should move 
            Vector3 zoomDist = scroll * zoomAxis;

            gameObject.transform.position = gameObject.transform.position + zoomDist;
        }

        // Scroll
        if (Input.touchCount >= 1 && !touchedUI)
        {
            plane.SetNormalAndPosition(Vector3.back, planetToFollow.transform.position);   // Update reference plane
            var delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (Input.touchCount >= 2)
                    gameObject.transform.Translate(delta1 / 5, Space.World);    // Scroll slower when zooming limit has been reached
                else
                    gameObject.transform.Translate(delta1, Space.World);

                focused = false;
                planetPreviousPos = Vector3.zero;
            }
        }

        if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            touchedUI = false;
        }
    }

    /*
     * By how much should zoom speed scale when away from center?
     * center is the body that we are following, usually planetToFollow 
     * centerOffset is really the dif from center to current pos
     * TODO: currently too simple -> more thought out coefficient?
     */
    float GetZoomMultiplierOverDistance(float center, float centerOffset, float speed) {
        return (centerOffset + center) * speed;
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

    Vector3 PlanePositionDelta(Touch touch)
    {
        // Didn't move
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        var rayBefore = Camera.main.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.main.ScreenPointToRay(touch.position);
        if (plane.Raycast(rayBefore, out var enterBefore) && plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        // Didn't hit the plane
        return Vector3.zero;
    }

    Vector3 PlanePosition(Vector2 screenPos)
    {
        var rayNow = Camera.main.ScreenPointToRay(screenPos);
        if (plane.Raycast(rayNow, out var enterNow))
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
            gameObject.transform.position = Vector3.LerpUnclamped(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                                                                  gameObject.transform.position, 0.8f);
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
            gameObject.transform.position = Vector3.LerpUnclamped(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0),
                                                                  gameObject.transform.position, 1.2f);
        }
    }
}