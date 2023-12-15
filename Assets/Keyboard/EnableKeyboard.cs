using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EnableKeyboard : MonoBehaviour
{

    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";
    public InputField button;

    // Start is called before the first frame update
    void Start()
    {

    }

// Update is called once per frame
void Update()
    {
        
    }

    void OnPointerClick()
    {
         button.Select();
         overlayKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
         if (overlayKeyboard != null)
            inputText = overlayKeyboard.text;
    }
}
