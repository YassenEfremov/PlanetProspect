using UnityEngine;


public class CanvasSwitch : MonoBehaviour
{
    //[SerializeField] Canvas globalCanvas;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas mapCanvas;


    public void SwitchCanvas()
    {
        mainCanvas.gameObject.SetActive(!mainCanvas.gameObject.activeSelf);
        mapCanvas.gameObject.SetActive(!mapCanvas.gameObject.activeSelf);
    }
}
