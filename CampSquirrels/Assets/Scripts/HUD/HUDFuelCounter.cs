using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDFuelCounter : MonoBehaviour
{
    TextMeshProUGUI counterText;
    PlayerObjectInteractions playerOI;
    private void Awake() {
        playerOI = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerObjectInteractions>();
        counterText = GetComponentInChildren<TextMeshProUGUI>();
        playerOI.OnChangeOfFuelCount += UpdateText;
    }

    private void UpdateText(int currentFuelCount)
    {
        counterText.text = currentFuelCount.ToString();
    }
}
