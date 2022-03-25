using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelection : MonoBehaviour
{
    static GameObject selectedObject = null;

    Ray cameraRay;
    RaycastHit hitObject;

    public GameObject panel;
    public GameObject[] buttons;


    private void Start()
    {
        gameObject.GetComponent<Outline>().enabled = false;
    }

    private void Update()
    {
        if (Input.touchCount >= 1)
        {
/*            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(cameraRay, out hitObject))
                {
                    // If we hit the current game object => set the touched flag
                    if (hitObject.transform == gameObject.transform)
                        touched = true;
                }
            }*/

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

                if (Physics.Raycast(cameraRay, out hitObject))
                {
                    // If we hit the current game object => enable it's outline
                    if (hitObject.transform == gameObject.transform)
                    {
                        if(selectedObject != null)
                        {
                            if(selectedObject != gameObject)
                            {
                                // Selected the same object => the object should be deselected
                                selectedObject.GetComponent<Outline>().enabled = false;
                                foreach (Transform button in panel.transform)
                                    Destroy(button.gameObject);
                            }
                        }

                        if(gameObject.GetComponent<Outline>().enabled == false)
                        {
                            // The object is being selected => instantiate all of it's buttons
                            foreach(GameObject button in buttons) {
                                GameObject newButton = Instantiate(button);
                                newButton.transform.SetParent(panel.transform, false);
                            }
                        }
                        else
                        {
                            // The object is being deselected
                            foreach (Transform button in panel.transform)
                                Destroy(button.gameObject);
                        }
                        selectedObject = gameObject;
                        gameObject.GetComponent<Outline>().enabled = !gameObject.GetComponent<Outline>().enabled;
                    }
                }
            }
        }
    }

/*    private void OnMouseDrag()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance = 0;

        // if the ray hits the plane...
        if (hPlane.Raycast(cameraRay, out distance))
        {
            transform.position = cameraRay.GetPoint(distance);
        }
    }*/
}
