using UnityEngine;

public class PlanetLabelManager : MonoBehaviour
{
    void Update()
    {
        foreach (Transform label in transform)
        {
            label.gameObject.SetActive(Vector3.Distance(label.gameObject.GetComponent<PlanetLabel>().targetPlanet.transform.position, Camera.main.transform.position)
            >
            (label.gameObject.GetComponent<PlanetLabel>().targetPlanet.transform.lossyScale.x * 20));
        }
    }
}
