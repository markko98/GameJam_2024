using UnityEngine;
using System.Collections;
using FMODUnity;
using Unity.VisualScripting;
using FMOD.Studio;

public class FmodSoundTemplate : MonoBehaviour {

    public FMODUnity.EventReference soundClip = new EventReference() { Path = "event:/TestEvent" };
    private FMOD.Studio.EventInstance soundEvent;

    [FMODUnity.ParamRef]
    [SerializeField] string paramTest;
    [FMODUnity.BankRef]
    [SerializeField] string bankTest;

	// for settings
	private Bus masterBus;
    private Bus UIBus;
	[Range(0, 1)] [SerializeField] float masterVolume = 1;
    [Range(0, 1)] [SerializeField] float UIVolume = 1;

    private void Awake()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        UIBus = RuntimeManager.GetBus("bus:/UI");
    }
    private void Update()
    {
		masterBus.setVolume(masterVolume);
		UIBus.setVolume(UIVolume);
    }
    void Start () {
		// Continous with parameter
		soundEvent = FMODUnity.RuntimeManager.CreateInstance(soundClip);
		FMODUnity.RuntimeManager.AttachInstanceToGameObject (soundEvent, transform, GetComponent<Rigidbody> ());
		soundEvent.getParameterByName("Name", out float val);
		soundEvent.setParameterByName("Name", 1);
		soundEvent.start();

		// Oneshot
		FMODUnity.RuntimeManager.PlayOneShot (soundClip, transform.position);

		// Oneshot
		FMODUnity.RuntimeManager.PlayOneShotAttached (soundClip, gameObject);
	}

	// For continous
	void OnDestroy(){
		if(soundEvent.IsUnityNull())
        {
			soundEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
			soundEvent.release();
		}
	}
}
