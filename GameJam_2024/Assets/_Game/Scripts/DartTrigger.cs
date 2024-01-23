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
            animator.SetBool("throw", true);
            StartCoroutine(WaitAndThrow(other));
        }
    }

    private IEnumerator WaitAndThrow(Collider other)
    {
        yield return new WaitForSeconds(0.5f);
        other.gameObject.GetComponent<Player>().LaunchPlayer();
    }
}
