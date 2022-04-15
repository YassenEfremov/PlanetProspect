using UnityEngine;
using UnityEngine.UI;

public class ObjectSelection : MonoBehaviour
{
    static GameObject selectedObject = null;

    Ray cameraRay;
    bool touchedObject;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] panels;


    void Start()
    {
        gameObject.GetComponent<Outline>().enabled = false;
    }

    void Update()
    {
        if (Input.touchCount >= 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(cameraRay, out var hitObject))
                {
                    // If we hit the current game object => set the touchedObject flag
                    if (hitObject.transform == gameObject.transform)
                        touchedObject = true;
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && touchedObject)
            {
                cameraRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(cameraRay, out var hitObject))
                {
                    // If we hit the current game object => enable it's outline
                    if (hitObject.transform == gameObject.transform)
                    {
                        if (selectedObject != null)
                        {
                            if (selectedObject != gameObject)
                            {
                                // Selected the same object => the object should be deselected
                                selectedObject.GetComponent<Outline>().enabled = false;
                                foreach (Transform button in panel.transform)
                                    Destroy(button.gameObject);
                            }
                        }

                        if (gameObject.GetComponent<Outline>().enabled == false)
                        {
                            // The object is being selected => instantiate all of it's buttons

                            // TEMPORARY ----------------------------------------------------------------------------
                            if (gameObject.tag == "Rocket")
                            {
                                panel.SetActive(true);
                            }
                            else
                            // --------------------------------------------------------------------------------------
                            {
                                foreach (GameObject button in buttons)
                                {
                                    GameObject newButton = Instantiate(button);
                                    newButton.transform.SetParent(panel.transform, false);

                                    switch (button.name)
                                    {
                                        case "InfoButton":
                                            newButton.GetComponent<Button>().onClick.AddListener(() => panels[0].GetComponent<PanelOpener>().togglePanel());
                                            break;

                                        case "BuildButton":
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            // TEMPORARY ----------------------------------------------------------------------------
                            if (gameObject.tag == "Rocket")
                            {
                                panel.SetActive(false);
                            }
                            else
                            // --------------------------------------------------------------------------------------
                            {
                                // The object is being deselected
                                foreach (Transform button in panel.transform)
                                    Destroy(button.gameObject);
                            }
                        }
                        selectedObject = gameObject;
                        gameObject.GetComponent<Outline>().enabled = !gameObject.GetComponent<Outline>().enabled;
                        touchedObject = false;
                    }
                }
            }
        }
    }
}
