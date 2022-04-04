using System.Collections;
using UnityEngine;


public class PlanetLabel : MonoBehaviour
{
    public GameObject targetPlanet;
    MainCameraController mainCameraController;
    MapCameraController mapCameraController;


    void Update()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(targetPlanet.transform.position);
        screenPoint.y += 100;
        gameObject.transform.position = screenPoint;
    }

    public void focusPlanet()
    {
        if(Camera.main.name == "MainCamera")
        {
            if(mainCameraController == null)
                mainCameraController = Camera.main.GetComponent<MainCameraController>();
            mainCameraController.planetToFollow = targetPlanet;
            mainCameraController.Update();
            mainCameraController.focusPlanet();
        }
        else if(Camera.main.name == "MapCamera")
        {
            if (mapCameraController == null)
                mapCameraController = Camera.main.GetComponent<MapCameraController>();
            mapCameraController.planetToFollow = targetPlanet;
            Camera.main.transform.position = new Vector3(targetPlanet.transform.position.x,
                                                         targetPlanet.transform.position.y,
                                                         targetPlanet.transform.position.z - targetPlanet.transform.lossyScale.x * 5);
        }
    }
}
