
using TMPro;
using FMOD.Studio;
using UnityEngine;

public class DoorLockedTrigger : MonoBehaviour
{

    public TextMeshProUGUI text;

    private EventInstance lockedSound;

    private void Start()
    {
        lockedSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.doorLocked, AudioSceneType.Gameplay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.text = "Ohh shit!! It's locked.";
            lockedSound.start();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            text.text = "";
        }
    }
}
