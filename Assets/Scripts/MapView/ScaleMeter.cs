using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleMeter : MonoBehaviour
{
    Plane plane;

    [SerializeField] GameObject edge1;
    [SerializeField] GameObject edge2;
    [SerializeField] GameObject distance;

    // Start is called before the first frame update
    void Start()
    {
        plane.SetNormalAndPosition(Vector3.back, Vector3.zero);   // Update reference plane
    }

    // Update is called once per frame
    void Update()
    {
        var rayEdge1 = Camera.main.ScreenPointToRay(edge1.transform.position);
        var rayEdge2 = Camera.main.ScreenPointToRay(edge2.transform.position);

        if (plane.Raycast(rayEdge1, out var enterEdge1) && plane.Raycast(rayEdge2, out var enterEdge2))
        {
            string distanceAsText = (Vector3.Distance(rayEdge1.GetPoint(enterEdge1), rayEdge2.GetPoint(enterEdge2)) * 127420).ToString() + " km";
            distance.GetComponent<Text>().text = distanceAsText;
        }
    }
}
