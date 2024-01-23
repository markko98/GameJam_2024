using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToAnimated : MonoBehaviour
{
    public ButtonMashing buttonMashing;
    public Collider collider;
    public float respawnTime = 30f;

    private Rigidbody[] rigibodies;
    public bool isRagdoll;
    private Animator animator;

    private static readonly int Idle = Animator.StringToHash("Idle");

    private CharacterController characterController;
    private float speed = 5f;
    private float turnSpeed = 20f;

    private Action AnimationDone;

    void Start()
    {
        GameEvent.EventSuccessfull += GetBackUp;
        GameEvent.EventFailed += GetBackUpWithTimer;


        rigibodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = true;

        ToggleRagdoll(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isRagdoll && other.gameObject.CompareTag("Door"))
        {
            LaunchPlayer();
        }
    }

    public void LaunchPlayer()
    {
        ToggleRagdoll(false);
        buttonMashing.StartMashing();
    }

    private void GetBackUpWithTimer()
    {
        StartCoroutine(GetBackUp(respawnTime));
    }

    private void GetBackUp()
    {
        ToggleRagdoll(true, true);
    }

    private IEnumerator GetBackUp(float time)
    {
        yield return new WaitForSeconds(time);
        ToggleRagdoll(true, true);
    }

    private void ToggleRagdoll(bool isAnimating, bool getUp = false)
    {
        isRagdoll = !isAnimating;

        collider.enabled = isAnimating;
        foreach (var bone in rigibodies)
        {
            bone.isKinematic = isAnimating;
        }

        animator.enabled = isAnimating;

        if (!getUp) return;
        isRagdoll = true;
        AnimationDone = AnimatorDoneCallback;
        animator.Play("Get Up");
    }


    private void AnimatorDoneCallback()
    {
        isRagdoll = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchPlayer();
        }

        if (animator.IsAnimationPlaying("Get Up")) return;
        if (AnimationDone == null) return;
        
        AnimationDone.Invoke();
        AnimationDone = null;
    }
}


public static class AnimatorExtensions
{
    public static bool IsAnimationPlaying(this Animator animator, string stateName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1 && animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    } 
}
