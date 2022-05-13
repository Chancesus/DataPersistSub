using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerName : MonoBehaviour
{
    public TextMeshProUGUI displayPlayerName;
    private void Awake()
    {
        displayPlayerName.text = ReadInput._ReadInput.InputField.text;
    }
}
