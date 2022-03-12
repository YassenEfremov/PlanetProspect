using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class testsrc : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject go1 = new GameObject();
        go1.name = "go1";
        go1.AddComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
