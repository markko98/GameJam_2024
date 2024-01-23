using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Action OnBossTalking;
    public static Action OnBossFainted;

    public CinemachineVirtualCamera virtualCamera;
    public Animator animator;

    public bool isTalking = false;
    public bool canInteract = true;

    private void Start()
    {
        MatchTextByTyping.OnTextTypedCorrectly += StopTalking;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            StartTalking();
        }
    }

    public void StartTalking()
    {
        isTalking = true;
        virtualCamera.gameObject.SetActive(true);
        animator.SetBool("isTalking", isTalking);
        OnBossTalking?.Invoke();
    }

    public void StopTalking()
    {
        isTalking = false;
        animator.SetBool("isTalking", isTalking);
        virtualCamera.gameObject.SetActive(false);
        canInteract = false;
    }

    public void Faint()
    {
        animator.SetBool("isFainting", true);
        OnBossFainted?.Invoke();
        StopTalking();
    }
}
