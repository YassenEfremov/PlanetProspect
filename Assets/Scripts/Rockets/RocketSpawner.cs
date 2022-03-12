using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketSpawner : MonoBehaviour {

    public GameObject rocketPrefab;
    public Vector3 spawnPosition;
    public Vector3 initialVelocity;
    public float spawnSize;

    // Start is called before the first frame update
    void Start() {
        GameObject rocket = SpawnRocket(spawnPosition);
        LaunchRocket(rocket, initialVelocity);
    }

    // Update is called once per frame
    void Update() {
        
    }

    GameObject SpawnRocket(Vector3 spawnPosition) {
        GameObject newRocket = (GameObject)Instantiate(rocketPrefab, spawnPosition, Quaternion.Euler(Vector3.forward));
        newRocket.AddComponent<RocketOrbitDraw>();


        newRocket.AddComponent<GravityObject>();
        newRocket.transform.SetParent(gameObject.transform);
        GravityObject rocketGravity = newRocket.GetComponent<GravityObject>();
        rocketGravity.isGravityAffected = true;
        rocketGravity.radius = 0.05f;
        rocketGravity.surfaceGravity = 0.04f;
        rocketGravity.initialVelocity = Vector3.zero;

        newRocket.transform.localScale = Vector3.one * spawnSize;
        return newRocket;
    }

    void LaunchRocket(GameObject rocket, Vector3 initialVelocity) {
        rocket.GetComponent<GravityObject>().initialVelocity = initialVelocity; 
    }
}
