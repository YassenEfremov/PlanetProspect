using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APODButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    public void showAPOD()
    {
        GameObject Content = GameObject.Find("Content");
        ContentGatheringScript scr;
        scr = Content.GetComponent<ContentGatheringScript>();
        scr.DeleteClones();
        scr.Awake();
        scr.Start();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
