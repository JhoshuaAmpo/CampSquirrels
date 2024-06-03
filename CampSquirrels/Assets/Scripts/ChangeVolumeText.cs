using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeText : MonoBehaviour
{
    [SerializeField]
    [Tooltip("True = SFX, False = Ambience")]
    bool isSFX;
    Slider slider;
    TextMeshProUGUI textMeshProUGUI;

    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        slider = transform.parent.GetComponentInChildren<Slider>();
        slider.value = isSFX ? PlayerPrefs.GetFloat("SFXVolume") : PlayerPrefs.GetFloat("AmbienceVolume");
    }

    private void Start() {
        ChangeText(slider.value);
    }

    public void ChangeText(float f){
        string key = isSFX ? "SFXVolume" : "AmbienceVolume";
        PlayerPrefs.SetFloat(key, f);
        textMeshProUGUI.text = $"{f*100:0}%";
    }
}
