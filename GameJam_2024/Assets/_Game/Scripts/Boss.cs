using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Animator animator;
    public static Action BossTalking;
    public bool isTalking = false;
    public bool canInteract = true;

    private void Start()
    {
        ButtonMashing.MashingSuccessful += StopTalking;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            StartTalking();
        }
    }

    private void StartTalking()
    {
        isTalking = true;
        virtualCamera.gameObject.SetActive(true);
        animator.SetBool("isTalking", isTalking);
        BossTalking?.Invoke();
    }

    private void StopTalking()
    {
        isTalking = false;
        animator.SetBool("isTalking", isTalking);
        virtualCamera.gameObject.SetActive(false);
        canInteract = false;
    }
}
