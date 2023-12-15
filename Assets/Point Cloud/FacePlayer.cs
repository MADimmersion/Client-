using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Transform playerTransform;

    private GameObject pointCloud;
    private Transform PCTransform;

    void Start()
    {
        // Find the player GameObject. Adjust this based on how you have your player set up in the scene.
        //playerTransform = GameObject.Find("OVRCameraRig").transform;
        playerTransform = GameObject.Find("Camera").transform;
        pointCloud = GameObject.Find("PointCloud");
        PCTransform = pointCloud.transform;
    }

    void Update()
    {
        // Use LookAt to rotate the cylinder's forward vector to face the player.
        // 'Vector3.up' specifies that the up direction of the cylinder should be aligned with the world's up direction (you can change this if needed).
        //transform.LookAt(playerTransform);

        PCTransform = playerTransform.transform;
        //Vector3 pcRadius = PCTransform.localScale * pointCloud.radius * 0.5f;
        //Debug.Log("RADISU"+ pcRadius);
        //transform.localScale = pcRadius;
    }
}
