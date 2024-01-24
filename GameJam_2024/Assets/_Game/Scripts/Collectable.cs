using System;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public ObjectiveType type;
    public static Action<ObjectiveType> Collected;

    [SerializeField] private Canvas canvas;
    private bool isTriggered = false;

    private void Awake()
    {
        canvas = transform.GetComponentInChildren<Canvas>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        canvas.gameObject.SetActive(true);
        isTriggered = true;
    }

    private void OnTriggerExit(Collider other)
    {
        canvas.gameObject.SetActive(false);
        isTriggered = false;
    }

    private void Update()
    {
        if (!isTriggered || !Input.GetKeyDown(KeyCode.E)) return;
        
        Collected?.Invoke(type);
        Destroy(this.gameObject);
    }
}
