using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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
    
    public static Action OnFart;
    public static Action OnFartEnd;

    public ParticleSystem fart;
    public Transform boss;
    public Transform playerArmature;
    public bool shouldLook = false;

    private EventType currentEvent;

    void Start()
    {
        rigibodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        characterController.detectCollisions = true;

        ToggleRagdoll(true);
        
        Boss.OnBossTalking += LookAtBoss;
        Boss.OnBossFainted += StopLookingAtBoss;
        
        RegisterEvent(EventType.ButtonMash);
    }
    
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
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LaunchPlayer();
        }

        if (animator.IsAnimationPlaying("Get Up")) return;
        if (AnimationDone == null) return;
        
        AnimationDone.Invoke();
        AnimationDone = null;
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
                GameEvent.EventSuccessfull += GetBackUp;
                GameEvent.EventFailed += GetBackUpWithTimer;
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
                GameEvent.EventSuccessfull -= GetBackUp;
                GameEvent.EventFailed -= GetBackUpWithTimer;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
