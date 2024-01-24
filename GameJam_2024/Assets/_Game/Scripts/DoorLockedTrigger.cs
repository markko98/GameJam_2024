using System;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class DoorLockedTrigger : MonoBehaviour
{
    private EventInstance lockedSound;

    private void Start()
    {
        lockedSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.doorLocked, AudioSceneType.Gameplay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lockedSound.start();
        }
    }
}
