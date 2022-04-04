using UnityEngine;


public class PlanetLabel : MonoBehaviour
{
    public GameObject targetPlanet;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(targetPlanet.transform.position);
        screenPoint.y += 100;
        gameObject.transform.position = screenPoint;

        //gameObject.SetActive(Vector3.Distance(targetPlanet.transform.position, Camera.main.transform.position) > (targetPlanet.transform.lossyScale.x * 5));
    }

    public void focusPlanet()
    {
        //if (Camera.main.GetComponent<MainCameraController>().planetToFollow != targetPlanet)
        Camera.main.GetComponent<MainCameraController>().planetToFollow = targetPlanet;
        float distance = Vector3.Distance(targetPlanet.transform.position, Camera.main.transform.position);
        Camera.main.transform.position = Vector3.Lerp(targetPlanet.transform.position, Camera.main.transform.position, targetPlanet.transform.lossyScale.x * 5 / distance);
    }
}
