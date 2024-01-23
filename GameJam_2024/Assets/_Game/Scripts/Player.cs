using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Player : MonoBehaviour
{
    public ParticleSystem fart;
    public Transform boss;
    public Transform playerArmeture;
    public bool shouldLook = false;

    void Start()
    {
        Boss.BossTalking += LookAtBoss;
        Boss.BossFainted += StopLookingAtBoss;
        ButtonMashing.MashingSuccessful += StopLookingAtBoss;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLook)
        {
            playerArmeture.LookAt(boss);
        }
    }

    public void Fart()
    {
        Debug.Log("Fart Sound!");
        fart.Play();
        ParticleSystem.EmissionModule em = fart.emission;
        em.enabled = true;
    }

    private void LookAtBoss()
    {
        shouldLook = true;
    }

    private void StopLookingAtBoss()
    {
        shouldLook = false;
    }
}
