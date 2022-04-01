using UnityEngine;


public class CanvasSwitch : MonoBehaviour
{
    [SerializeField] Canvas closeCanvas;
    [SerializeField] Canvas mapCanvas;


    public void SwitchCanvas()
    {
        closeCanvas.gameObject.SetActive(!closeCanvas.gameObject.activeSelf);
        mapCanvas.gameObject.SetActive(!mapCanvas.gameObject.activeSelf);
    }
}
