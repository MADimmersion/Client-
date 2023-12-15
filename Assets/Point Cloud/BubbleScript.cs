using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    private Vector3 center = Vector3.zero;
    private float radius = 1f;

    public GameObject pointCloud;
    public PointCloud PCScript;
    // Start is called before the first frame update
    void Awake()
    {
        pointCloud = GameObject.Find("PointCloud");
        // Get a reference to the ScriptA component attached to the same GameObject
        PCScript = pointCloud.GetComponent<PointCloud>();

        center = PCScript.center;
        radius = PCScript.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (PCScript.center != center)
        {
            center = PCScript.center;
            Debug.Log("center : " + center);

            transform.position = center;

        }
        if (PCScript.radius != radius)
        {
            radius = PCScript.radius;
            transform.localScale = new Vector3(2*radius,2*radius,2*radius);
        }
    }
}
