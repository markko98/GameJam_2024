using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipperyTrigger : MonoBehaviour
{
    public float waitSeconds = 4;
    public bool canSlip = true;

    private void OnTriggerEnter(Collider other)
    {
        if (!canSlip) return;

        if (other.gameObject.CompareTag("Player"))
        {
            canSlip = false;
            other.gameObject.GetComponent<Player>().LaunchPlayer();
            StartCoroutine(WaitAndReset());
        }
    }

    IEnumerator WaitAndReset()
    {
        yield return new WaitForSeconds(waitSeconds);
        canSlip = true;
    }
}
