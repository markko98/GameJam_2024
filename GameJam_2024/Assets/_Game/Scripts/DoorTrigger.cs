using UnityEngine;
using DG.Tweening;

public class DoorTrigger : MonoBehaviour
{

    public int angle;

    public bool shouldLaunch = false;

    private bool doorOpened;

    private void OnTriggerEnter(Collider other)
    {
        if (doorOpened) return;
        
        if (other.gameObject.CompareTag("Player"))
        {
            transform.DORotate(new Vector3(0, angle, 0), 0.5f, RotateMode.Fast);

            doorOpened = true;
            if (!shouldLaunch) return;
            
            other.gameObject.GetComponent<Player>().LaunchPlayer();
        }
    }
}
