using System;
using System.Collections;
using UnityEngine;

public class ButtonMashing : MonoBehaviour
{

    public static Action OnMashingFailed;
    public static Action OnMashingSuccessful;

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
                OnMashingSuccessful?.Invoke();
            }

            if (mash <= 0)
            {
                StopMashing();
                OnMashingFailed?.Invoke();
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
        started = false;
        Debug.Log("Mashing stopped!");
    }

    private IEnumerator WaitAndStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        started = true;
        // TODO - Inform player
        Debug.Log("Mashing started!");
    }
}
