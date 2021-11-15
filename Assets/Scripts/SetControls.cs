using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetControls : MonoBehaviour
{
    private KeyCode key;
    private bool reading;
    private int keyIndex;
    public Text textField;
    public Text Warning;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (reading)
        {
            Warning.text = "Press any key to select";
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(vKey))
                {
                    if (GameManager.ControlsP1.Contains(vKey) ||
                        GameManager.ControlsP2.Contains(vKey) ||
                        GameManager.MovementP1.Contains(vKey) ||
                        GameManager.MovementP2.Contains(vKey))
                    {
                        Warning.text = "Select another key";
                    }
                    else if (textField.gameObject.transform.parent.name.Contains("1"))
                    {
                        textField.text = vKey.ToString();
                        GameManager.ControlsP1[keyIndex] = vKey;
                        Warning.text = "";
                    }
                    else if (textField.gameObject.transform.parent.name.Contains("2"))
                    {
                        textField.text = vKey.ToString();
                        GameManager.ControlsP2[keyIndex] = vKey;
                        Warning.text = "";
                    }
                    reading = false;
                }
            }
        }
    }

    public void SetControl(Text text)
    {
        int index = text.gameObject.transform.parent.transform.GetSiblingIndex();
        keyIndex = index;
        textField = text;
        reading = true;
    }
}
