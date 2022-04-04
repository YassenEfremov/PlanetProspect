using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLabelManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform label in transform)
        {
            label.gameObject.SetActive(Vector3.Distance(label.gameObject.GetComponent<PlanetLabel>().targetPlanet.transform.position, Camera.main.transform.position) > (label.gameObject.GetComponent<PlanetLabel>().targetPlanet.transform.lossyScale.x * 6));
        }
    }
}
