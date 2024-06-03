using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioSource> sfxSources;
    public List<AudioSource> ambienceSources;
    public List<AudioSource> musicSources;
    public enum AudioType { SFX, Ambience, Music}

    private void Update() {
        UpdateAllAudioSources();
    }

    private void UpdateAllAudioSources() {
        foreach (AudioSource source in sfxSources) {
            source.volume = PlayerPrefs.GetFloat("SFXVolume");
        }
        foreach (AudioSource source in ambienceSources) {
            source.volume = PlayerPrefs.GetFloat("AmbienceVolume");
        }
        foreach (AudioSource source in musicSources) {
            source.volume = PlayerPrefs.GetFloat("MusicVolume");
        }
    }
}
