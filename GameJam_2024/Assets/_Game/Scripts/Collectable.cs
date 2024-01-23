using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public string type;

    public Canvas canvas;

    public static Action<string> Collected;

    bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.gameObject.SetActive(true);
            isTriggered = true;
        }
    }

    private void Update()
    {
        if (isTriggered && Input.GetKeyDown(KeyCode.E))
        {
            Collected?.Invoke(type);
            Destroy(this.gameObject);
        }
    }
}
