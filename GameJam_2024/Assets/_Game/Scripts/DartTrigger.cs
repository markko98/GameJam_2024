using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartTrigger : MonoBehaviour
{
    public bool canSlip = true;
    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (!canSlip) return;

        if (other.gameObject.CompareTag("Player"))
        {
            canSlip = false;
            other.gameObject.GetComponent<Player>().LaunchPlayer();
            animator.Play("Throw");
        }
    }
}
