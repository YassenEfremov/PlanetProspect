using UnityEngine;
using UnityEngine.UI;


public class PanelOpener : MonoBehaviour
{
    static GameObject openPanel = null;

    [SerializeField] GameObject backgroundDimmer;



    void Start()
    {
        foreach (Button button in GetComponentsInChildren<Button>(true))
        {
            //Debug.Log(button.GetComponent<Image>().color);
            //button.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
            //button.gameObject.GetComponent<Animator>().keepAnimatorControllerStateOnDisable = true;
        }
    }

    //void OnDisable()
    //{
    //    foreach (Button button in GetComponentsInChildren<Button>(true))
    //    {
    //        Debug.Log("AAA");
    //        button.gameObject.GetComponent<Animator>().Play("Normal", 0, 0f);
    //    }
    //}


    public void togglePanel()
    {
        // There is no active panel => set the given one as active
        if (openPanel == null)
        {
            openPanel = gameObject;
            gameObject.SetActive(true);
            backgroundDimmer.SetActive(true);
            // Disable all buttons except for the ones that are part of the given panel
            foreach (Button button in FindObjectsOfType<Button>())
            {
                if (!button.transform.IsChildOf(gameObject.transform))
                    button.interactable = false;
            }
        }
        // There already is an active panel => deactivate it
        else
        {
            if (gameObject == openPanel)
            {
                openPanel = null;
                gameObject.SetActive(false);
                backgroundDimmer.SetActive(false);
                // Enable all buttons that are part of the given panel
                foreach (Button button in FindObjectsOfType<Button>())
                {
                    if (!button.transform.IsChildOf(gameObject.transform))
                        button.interactable = true;
                }
            }
        }
    }
}
