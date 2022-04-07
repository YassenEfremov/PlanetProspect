using UnityEngine;


public class CanvasSwitch : MonoBehaviour {
    //[SerializeField] Canvas globalCanvas;
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas mapCanvas;

    public RocketSpawner spawner;

    public void SwitchCanvas() {
        mainCanvas.gameObject.SetActive(!mainCanvas.gameObject.activeSelf);
        mapCanvas.gameObject.SetActive(!mapCanvas.gameObject.activeSelf);


        if (mapCanvas.gameObject.activeSelf) {
            Universe.isFrozen = true;
            spawner = FindObjectOfType<RocketSpawner>();
            spawner.SpawnRocket();
        } else {
            Universe.isFrozen = false;
            spawner.rocketPrefab.transform.parent = spawner.spawnLocation.transform;
        }
    }
}
