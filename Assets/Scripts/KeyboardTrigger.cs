using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTrigger : MonoBehaviour
{

    [SerializeField] private Update_IP m_UpdateIP;


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            m_UpdateIP.UpdateIP();
        }
    }
}
