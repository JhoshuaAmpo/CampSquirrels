using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    List<AudioSource> musicSources;
    List<AudioSource> sfxSources;
    List<AudioSource> ambienceSources;

    private void Awake() {
        musicSources = new();
        sfxSources = new();
        ambienceSources = new();

        
    }
}
