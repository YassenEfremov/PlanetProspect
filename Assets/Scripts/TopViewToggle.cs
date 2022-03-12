using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopViewToggle : MonoBehaviour
{
    public void ToggleTopView()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
