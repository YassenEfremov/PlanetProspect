using UnityEngine;


public class CameraSwitch : MonoBehaviour
{
    [SerializeField] Camera closeView;
    [SerializeField] Camera mapView;


    // Start is called before the first frame update
    void Start()
    {
        /*        closeView.gameObject.SetActive(true);
                topView.gameObject.SetActive(false);*/
    }


    public void SwitchCamera()
    {
        closeView.gameObject.SetActive(!closeView.gameObject.activeSelf);
        mapView.gameObject.SetActive(!mapView.gameObject.activeSelf);

        // This is needed because the camera's scripts aren't loaded on the first touch
        if (closeView.gameObject.activeSelf)
            closeView.GetComponent<MainCameraController>().Update();
        else
            mapView.GetComponent<MapCameraController>().Update();
    }
}
