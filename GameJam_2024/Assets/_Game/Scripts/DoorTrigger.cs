using System;
using UnityEngine;
using DG.Tweening;
using FMOD.Studio;

public class DoorTrigger : MonoBehaviour
{

    public int angle;

    public bool shouldLaunch = false;

    private bool doorOpened;

    private EventInstance doorSlapSound;

    private void Start()
    {
        doorSlapSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.doorSlap, AudioSceneType.Gameplay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doorOpened) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            transform.DORotate(new Vector3(0, angle, 0), 0.5f, RotateMode.Fast);

            doorOpened = true;
            if (!shouldLaunch) return;

            doorSlapSound.start();
            other.gameObject.GetComponent<Player>().LaunchPlayer();
        }
    }
}
