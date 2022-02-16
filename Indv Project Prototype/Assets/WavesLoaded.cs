using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesLoaded : MonoBehaviour
{

    public Text loaded;
    public WaveSpawner waveSpawner;

    public void RefreshWavesLoaded()
    {
        loaded.text = waveSpawner.waves.Count + " Waves Loaded";
    }

    private void OnEnable()
    {
        RefreshWavesLoaded();
    }
}
