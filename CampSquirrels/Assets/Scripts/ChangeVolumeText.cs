using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeText : MonoBehaviour
{   
    [SerializeField]
    AudioController.AudioType audioType;
    Slider slider;
    TextMeshProUGUI textMeshProUGUI;


    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        slider = transform.parent.GetComponentInChildren<Slider>();
        switch (audioType) {
            case AudioController.AudioType.SFX:
                slider.value = PlayerPrefs.GetFloat("SFXVolume");
            break;
            case AudioController.AudioType.Ambience:
                slider.value = PlayerPrefs.GetFloat("AmbienceVolume");
            break;
            case AudioController.AudioType.Music:
                slider.value = PlayerPrefs.GetFloat("MusicVolume");
            break;
        }
    }

    private void Start() {
        ChangeText(slider.value);
    }

    public void ChangeText(float f){
        string key = "";
        switch (audioType) {
            case AudioController.AudioType.SFX:
                key = "SFXVolume";
            break;
            case AudioController.AudioType.Ambience:
                key = "AmbienceVolume";
            break;
            case AudioController.AudioType.Music:
                key = "MusicVolume";
            break;
            default:
                Debug.LogError("Audio Type doesn't have a key");
            break;
        }
        PlayerPrefs.SetFloat(key, f);
        textMeshProUGUI.text = $"{f*100:0}%";
    }
}
