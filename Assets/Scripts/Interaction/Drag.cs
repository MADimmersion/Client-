using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform pointer;

    [Header("Select to include drag")]

    public bool x;
    public bool y;
    public bool z;

    public void DragUI()
    {
        float newX = x ? pointer.position.x : transform.position.x;
        float newY = y ? pointer.position.y : transform.position.y;
        float newZ = x ? pointer.position.z : transform.position.z;
    }
}
