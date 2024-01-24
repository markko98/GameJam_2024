using Cinemachine;
using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Action<Transform> OnBossTalking;
    public static Action OnBossFainted;

    public CinemachineVirtualCamera virtualCamera;
    public Animator animator;

    public EventType eventType = EventType.MatchText;

    public bool isTalking = false;
    public bool canInteract = true;

    private Player player;

    private EventInstance npcTalking;

    private void Start()
    {
        npcTalking = AudioManager.Instance.CreateInstance(AudioProvider.Instance.bossTalking, AudioSceneType.Gameplay);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canInteract)
        {
            player = other.gameObject.GetComponent<Player>();
            player.SetBossTarget(animator.transform);
            animator.transform.LookAt(player.transform);
            player.RegisterEvent(eventType);
            StartTalking();
        }
    }

    public void StartTalking()
    {
        npcTalking.start();
        isTalking = true;
        virtualCamera.gameObject.SetActive(true);
        animator.SetBool("isTalking", isTalking);
        OnBossTalking?.Invoke(animator.transform);
        RegisterEvent(eventType);
    }

    public void StopTalking()
    {
        npcTalking.stop(STOP_MODE.IMMEDIATE);
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
        Debug.Log("Boss Register " + type);

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
        Debug.Log("Boss DeRegister " + type);

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
