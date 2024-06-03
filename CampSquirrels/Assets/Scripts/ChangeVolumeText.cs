using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeText : MonoBehaviour
{   
    [SerializeField]
    AudioType audioType;
    public enum AudioType { SFX, Ambience, Music}
    Slider slider;
    TextMeshProUGUI textMeshProUGUI;

    private void Awake() {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        slider = transform.parent.GetComponentInChildren<Slider>();
        switch (audioType) {
            case AudioType.SFX:
                slider.value = PlayerPrefs.GetFloat("SFXVolume");
            break;
            case AudioType.Ambience:
                slider.value = PlayerPrefs.GetFloat("AmbienceVolume");
            break;
            case AudioType.Music:
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
            case AudioType.SFX:
                key = "SFXVolume";
            break;
            case AudioType.Ambience:
                key = "AmbienceVolume";
            break;
            case AudioType.Music:
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
