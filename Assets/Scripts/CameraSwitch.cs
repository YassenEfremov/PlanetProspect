using UnityEngine;


public class CameraSwitch : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera mapCamera;


    // Start is called before the first frame update
    void Start()
    {
        /*        mainCamera.gameObject.SetActive(true);
                topView.gameObject.SetActive(false);*/
    }


    public void SwitchCamera()
    {
        mainCamera.gameObject.SetActive(!mainCamera.gameObject.activeSelf);
        mapCamera.gameObject.SetActive(!mapCamera.gameObject.activeSelf);

        // This is needed because the camera's scripts aren't loaded on the first touch
        if (mainCamera.gameObject.activeSelf)
            mainCamera.GetComponent<MainCameraController>().Update();
        else
            mapCamera.GetComponent<MapCameraController>().Update();
    }
}
