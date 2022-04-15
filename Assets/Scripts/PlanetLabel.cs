using UnityEngine;


public class PlanetLabel : MonoBehaviour
{
    public GameObject label;
    MainCameraController mainCameraController;
    MapCameraController mapCameraController;


    void Update()
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        screenPoint.y += 100;
        label.transform.position = screenPoint;

        // Deactivate the lable if we get too close to the planet
        label.gameObject.SetActive(Vector3.Distance(gameObject.transform.position, Camera.main.transform.position) > (gameObject.transform.lossyScale.x * 30));
    }


    public void focusPlanet()
    {
        if (Camera.main.name == "MainCamera")
        {
            if (mainCameraController == null)
                mainCameraController = Camera.main.GetComponent<MainCameraController>();
            mainCameraController.planetToFollow = gameObject;
            mainCameraController.Update();
            mainCameraController.focusPlanet();
        }
        else if (Camera.main.name == "MapCamera")
        {
            if (mapCameraController == null)
                mapCameraController = Camera.main.GetComponent<MapCameraController>();
            mapCameraController.planetToFollow = gameObject;
            Camera.main.transform.position = new Vector3(gameObject.transform.position.x,
                                                         gameObject.transform.position.y,
                                                         gameObject.transform.position.z - gameObject.transform.lossyScale.x * 5);
            mapCameraController.focused = true;
        }
    }
}
