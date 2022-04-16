using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RocketSpawner : MonoBehaviour {

    public GameObject rocketPrefab;
    public GameObject spawnLocation;

/*
    [SerializeField]
    public FixedJoystick joystick;
    [SerializeField]
    public Slider slider;
*/

    private GameObject newRocket;

    // temporary var, while only one rocket exists
    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start() {
        // GameObject rocket = SpawnRocket(spawnPosition);
        // LaunchRocket(rocket, initialVelocity);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void SpawnRocket(bool respawnSig) {
        // TODO: Instanticate a new rocket which is an exact copy of the prefab

        /*
         * This is required, because if the rocket spawns inside the object, gravity calculations don't work as expected
         */
        GravityObject spawnGravity = spawnLocation.GetComponentInChildren<GravityObject>();
        Rigidbody spawnRigidbody = spawnLocation.GetComponent<Rigidbody>();
        
        spawnGravity.enabled = false;
        spawnRigidbody.Sleep();

        // if Rocket hasn't spawned
        if (!hasSpawned || respawnSig) {
            rocketPrefab.GetComponentInChildren<GravityObject>().isActive = false;
            hasSpawned = true;
            rocketPrefab.transform.position = new Vector3(spawnLocation.transform.position.x, spawnLocation.transform.position.y + spawnGravity.radius, spawnLocation.transform.position.z);
        } 
        
        rocketPrefab.transform.parent = null;

        /*
        newRocket = (GameObject)Instantiate(rocketPrefab, spawnLocation.transform.localPosition, Quaternion.Euler(Vector3.forward));

        newRocket.AddComponent<GravityObject>();
        GravityObject rocketGravity = newRocket.GetComponent<GravityObject>();
        rocketGravity.isActive = false;
        rocketGravity.isGravityAffected = true;
        rocketGravity.mass = 0;
        newRocket.AddComponent<RocketOrbitDraw>();
        /*
        GameObject newRocket = (GameObject)Instantiate(rocketPrefab, spawnPosition, Quaternion.Euler(Vector3.forward));
        newRocket.AddComponent<RocketOrbitDraw>();


        newRocket.AddComponent<GravityObject>();
        newRocket.transform.SetParent(gameObject.transform);
        GravityObject rocketGravity = newRocket.GetComponent<GravityObject>();
        rocketGravity.isGravityAffected = true;
        rocketGravity.radius = 0.05f;
        // rocketGravity.surfaceGravity = 0.04f;
        rocketGravity.initialVelocity = Vector3.zero;

        newRocket.transform.localScale = Vector3.one * spawnSize;
        return newRocket;
        */
    }

    void LaunchRocket(GameObject rocket, Vector3 initialVelocity) {
        rocket.GetComponent<GravityObject>().initialVelocity = initialVelocity; 
    }
}
