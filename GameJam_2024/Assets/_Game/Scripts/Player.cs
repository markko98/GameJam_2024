using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public static Action OnFart;
    public static Action OnFartEnd;

    public ParticleSystem fart;
    public Transform boss;
    public Transform playerArmature;
    public bool shouldLook = false;

    private EventType currentEvent;

    void Start()
    {
        Boss.OnBossTalking += LookAtBoss;
        Boss.OnBossFainted += StopLookingAtBoss;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLook)
        {
            playerArmature.LookAt(boss);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Fart();
        }
    }

    public void Fart()
    {
        // TODO
        Debug.Log("Fart Sound!");
        OnFart?.Invoke();
        fart.Play();
        ParticleSystem.EmissionModule em = fart.emission;
        em.enabled = true;
        
        DeregisterEvent(currentEvent);
    }

    public void LookAtBoss()
    {
        shouldLook = true;
    }

    public void StopLookingAtBoss()
    {
        shouldLook = false;
        OnFartEnd?.Invoke();
        
        DeregisterEvent(currentEvent);
    }
    
    private IEnumerator WaitAndFart()
    {
        yield return new WaitForSeconds(3f);
        Fart();
    }

    public void RegisterEvent(EventType type)
    {
        currentEvent = type;
        switch (type)
        {
            case EventType.MatchText:
                GameEvent.EventSuccessfull += StopLookingAtBoss;
                GameEvent.EventFailed += () => StartCoroutine(WaitAndFart());
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
                GameEvent.EventSuccessfull -= StopLookingAtBoss;
                GameEvent.EventFailed -= () => StartCoroutine(WaitAndFart());
                break;
            case EventType.ButtonMash:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}


