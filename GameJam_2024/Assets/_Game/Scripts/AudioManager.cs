using FMOD.Studio;
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum AudioSceneType { MainMenu, Gameplay, Global }
public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;
    [Range(0, 1)]
    public float ambienceVolume = 1;
    [Range(0, 1)]
    public float SFXVolume = 1;

    private Bus masterBus;
    private Bus musicBus;
    private Bus ambienceBus;
    private Bus sfxBus;

    private Dictionary<AudioSceneType, List<EventInstance>> eventInstances;
    private Dictionary<AudioSceneType, List<StudioEventEmitter>> eventEmitters;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Setup();
            }

            return _instance;
        }
    }
    public static void Setup()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = Resources.Load<AudioManager>("AudioManager");
        _instance.SetupForSounds();
        DontDestroyOnLoad(_instance);
    }

    private void SetupForSounds()
    {
        eventInstances = new Dictionary<AudioSceneType, List<EventInstance>>();
        eventEmitters = new Dictionary<AudioSceneType, List<StudioEventEmitter>>();

        //masterBus = RuntimeManager.GetBus("bus:/");
        //musicBus = RuntimeManager.GetBus("bus:/Music");
        //ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        //sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Update()
    {
        //masterBus.setVolume(masterVolume);
        //musicBus.setVolume(musicVolume);
        //ambienceBus.setVolume(ambienceVolume);
        //sfxBus.setVolume(SFXVolume);
    }

    public void SetGameplayAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference, AudioSceneType sceneType)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        if (eventInstances.ContainsKey(sceneType))
        {
            eventInstances[sceneType].Add(eventInstance);
        }
        else
        {
            eventInstances[sceneType] = new List<EventInstance>();
        }
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject, AudioSceneType sceneType)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        if (eventEmitters.ContainsKey(sceneType))
        {
            eventEmitters[sceneType].Add(emitter);
        }
        else
        {
            eventEmitters[sceneType] = new List<StudioEventEmitter>();
        }
        return emitter;
    }

    public void CleanUp(AudioSceneType sceneType)
    {
        var sceneTypeInstances = eventInstances.Where(x=>x.Key == sceneType);
        var sceneTypeEmitters = eventEmitters.Where(x => x.Key == sceneType);

        foreach (var eventInstance in sceneTypeInstances)
        {
            foreach (var val in eventInstance.Value)
            {
                val.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                val.release();
            }
        }
        foreach (var eventEmitter in sceneTypeEmitters)
        {
            foreach (var val in eventEmitter.Value)
            {
                val.Stop();
            }
        }
    }
}
