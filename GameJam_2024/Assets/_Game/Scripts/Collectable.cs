using System;
using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public ObjectiveType type;
    public static Action<ObjectiveType> Collected;

    [SerializeField] TextMeshProUGUI text;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        text.text = "Press 'E' to take!";
        isTriggered = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        text.text = "Press 'E' to take!";
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        text.text = "";
        isTriggered = false;
    }

    private void Update()
    {
        if (!isTriggered || !Input.GetKeyDown(KeyCode.E)) return;

        text.text = "";
        Collected?.Invoke(type);
        Destroy(this.gameObject);
    }
}
