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
    public Transform playerArmeture;
    public bool shouldLook = false;

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
            playerArmeture.LookAt(boss);
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

    }

    public void LookAtBoss()
    {
        shouldLook = true;
    }

    public void StopLookingAtBoss()
    {
        shouldLook = false;
        OnFartEnd?.Invoke();
    }
}
