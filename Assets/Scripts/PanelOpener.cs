using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOpener : MonoBehaviour
{
    static GameObject openPanel = null;

    public GameObject panel;


    public void TogglePanel()
    {
        if (openPanel == null)
        {
            panel.SetActive(true);
            openPanel = panel;
            foreach (Button button in FindObjectsOfType<Button>())
            {
                if (button != gameObject.GetComponent<Button>() && !button.transform.IsChildOf(panel.transform))
                    button.interactable = false;
            }
        }
        else
        {
            if (panel == openPanel)
            {
                panel.SetActive(false);
                openPanel = null;
                foreach (Button button in FindObjectsOfType<Button>())
                {
                    if (button != gameObject.GetComponent<Button>() && !button.transform.IsChildOf(panel.transform))
                        button.interactable = true;
                }
            }
        }
    }
}
