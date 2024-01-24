using System;
using System.Collections;
using FMOD.Studio;
using UnityEngine;

public class ButtonMashing : GameEvent
{
    public KeyCode keyCode;
    public float mashDelay = 0.5f;
    public float requiredTimes = 10;

    float mash;
    bool pressed = false;
    bool started = false;
    public int count;

    private EventInstance mashSuccess;
    private EventInstance mashFail;

    void Start()
    {
        mashSuccess = AudioManager.Instance.CreateInstance(AudioProvider.Instance.mashSuccess, AudioSceneType.Gameplay);
        mashFail = AudioManager.Instance.CreateInstance(AudioProvider.Instance.mashFail, AudioSceneType.Gameplay);

        mash = mashDelay;
    }

    void Update()
    {
        if (started)
        {
            mash -= Time.deltaTime;
            if (Input.GetKeyDown(keyCode) && !pressed)
            {
                pressed = true;
                count++;
                mash = mashDelay;
            }
            else if (Input.GetKeyUp(keyCode))
            {
                pressed = false;
            }

            if (count >= requiredTimes)
            {
                StopMashing();
                EventSuccessfull?.Invoke();
                mashSuccess.start();
                Debug.Log("Mashing successful!");
            }

            if (mash <= 0)
            {
                StopMashing();
                EventFailed?.Invoke();
                mashFail.start();
                Debug.Log("Mashing failed!");
            }
        }
    }

    public void StartMashing()
    {
        IEnumerator coroutine = WaitAndStart(1f);
        StartCoroutine(coroutine);
    }

    public void StopMashing()
    {
        text.text = "";

        count = 0;
        mash = mashDelay;
        started = false;
        Debug.Log("Mashing stopped!");
    }

    private IEnumerator WaitAndStart(float waitTime)
    {
        text.text = "Press Q repeatedly to get up quickly!";

        yield return new WaitForSeconds(waitTime);
        started = true;
        // TODO - Inform player
        Debug.Log("Mashing started!");
    }
}
