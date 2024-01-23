using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    Animator animator;
    public Transform player;

    void Start()
    {
        animator = GetComponent<Animator>();
        Player.OnFart += StartClapping;
        Player.OnFartEnd += StopClapping;
    }

    private void StartClapping()
    {
        animator.SetBool("isClapping", true);
        transform.LookAt(player);
        // TODO
        Debug.Log("Clapping Sound");
    }

    private void StopClapping()
    {
        animator.SetBool("isClapping", false);
    }
}
