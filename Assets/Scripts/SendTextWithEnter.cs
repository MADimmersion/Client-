using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputTextWithEnter : MonoBehaviour
{
    // Start is called before the first frame update
    public InputField Field;
    public Text TextBox;

    public void CopyText()
    {
        TextBox.text = Field.text;
    }
}
