using UnityEngine;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{

    public int angle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.DORotate(new Vector3(0, angle, 0), 0.5f, RotateMode.Fast);
        }
    }
}
