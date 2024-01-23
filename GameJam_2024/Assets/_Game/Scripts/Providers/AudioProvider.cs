using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioProvider : MonoBehaviour
{
    [Header("Ambient music")]
    public FMODUnity.EventReference mainMenuMusic;
    public FMODUnity.EventReference gameplayMusic;

    [Header("SFX")]
    public FMODUnity.EventReference stomachSoundLoop;
    public FMODUnity.EventReference stomachSound; // couple of them randomly
    public FMODUnity.EventReference fartSound; // couple of them randomly
    public FMODUnity.EventReference fartSoundHuge;

    [Header("UI")]
    public FMODUnity.EventReference hoverUI;
    public FMODUnity.EventReference clickUI;

    private static AudioProvider _instance;
    public static AudioProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                Prewarm();
            }

            return _instance;
        }
    }
    public static void Prewarm()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = Resources.Load<AudioProvider>(Strings.AssetProvidersPath + "AudioProvider");
        DontDestroyOnLoad(_instance);
    }
}
