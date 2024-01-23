using System;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public Action GoalReached;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GoalReached?.Invoke();
        }
    }
}
