using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderSkybox: MonoBehaviour
{
    public Material SkyBox_Mat;
    private WebCamTexture tex;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        for (int i = 0; i < devices.Length; i++)
        {
            print("Webcam available: " + devices[i].name);
        }

        //Renderer rend = this.GetComponentInChildren<Renderer>();

        tex = new WebCamTexture("OBS Virtual Camera");

        //rend.material.mainTexture = tex;
        tex.Play();

        SkyBox_Mat.mainTexture = tex;
        RenderSettings.skybox = SkyBox_Mat;

        transform.position = new Vector3(0, 0, 0);
    }

    private void Update()
    {
    }
}
