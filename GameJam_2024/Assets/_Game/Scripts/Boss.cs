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

    public EventType eventType = EventType.MatchText;

    public bool isTalking = false;
    public bool canInteract = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            var player = other.gameObject.GetComponent<Player>();
            player.RegisterEvent(eventType);
            StartTalking();
        }
    }

    public void StartTalking()
    {
        RegisterEvent(eventType);
        isTalking = true;
        virtualCamera.gameObject.SetActive(true);
        animator.SetBool("isTalking", isTalking);
        OnBossTalking?.Invoke();
    }

    public void StopTalking()
    {
        DeregisterEvent(eventType);
        isTalking = false;
        animator.SetBool("isTalking", isTalking);
        virtualCamera.gameObject.SetActive(false);
        canInteract = false;
        
        DeregisterEvent(eventType);
    }

    public void Faint()
    {
        animator.SetBool("isFainting", true);
        OnBossFainted?.Invoke();
        StopTalking();
    }
    
    private IEnumerator WaitAndFaint()
    {
        yield return new WaitForSeconds(5f);
        Faint();
    }

    public void RegisterEvent(EventType type)
    {
        switch (type)
        {
            case EventType.MatchText:
                GameEvent.EventSuccessfull += StopTalking;
                GameEvent.EventFailed += () => StartCoroutine(WaitAndFaint());
                break;
            case EventType.ButtonMash:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        

    }

    public void DeregisterEvent(EventType type)
    {
        
        switch (type)
        {
            case EventType.MatchText:
                GameEvent.EventSuccessfull -= StopTalking;
                GameEvent.EventFailed -= () => StartCoroutine(WaitAndFaint());
                break;
            case EventType.ButtonMash:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
        
    }
}
