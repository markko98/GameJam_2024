using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Animator animator;
    public Transform player;

    private EventInstance clappingSound;
    private EventInstance voiceline;

    void Start()
    {
        clappingSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.clapping, AudioSceneType.Gameplay);
        voiceline = AudioManager.Instance.CreateInstance(AudioProvider.Instance.girlShitDude, AudioSceneType.Gameplay);

        animator = GetComponent<Animator>();
        Player.OnFart += StartClapping;
        Player.OnFartEnd += StopClapping;
    }

    private void StartClapping()
    {
        animator.SetBool("isClapping", true);
        transform.LookAt(player);
        
        clappingSound.start();
        voiceline.start();
    }

    private void StopClapping()
    {
        animator.SetBool("isClapping", false);
    }
}
