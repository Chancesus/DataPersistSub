using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadInput : MonoBehaviour
{
    public TMP_InputField InputField;
    public string input;
    public static ReadInput _ReadInput;

    public int score;

    private void Awake()
    {
        if (_ReadInput == null)
        {
            _ReadInput = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReadStringInput()
    {
        input = InputField.text;
    }
}
