using System;
using System.Collections;
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

    void Start()
    {
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
                Debug.Log("Mashing successful!");
            }

            if (mash <= 0)
            {
                StopMashing();
                EventFailed?.Invoke();
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
        count = 0;
        mash = mashDelay;
        started = false;
        canvas.gameObject.SetActive(false);
        Debug.Log("Mashing stopped!");
    }

    private IEnumerator WaitAndStart(float waitTime)
    {
        canvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(waitTime);
        started = true;
        // TODO - Inform player
        Debug.Log("Mashing started!");
    }
}
