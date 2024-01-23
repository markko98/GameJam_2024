using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonMashing : MonoBehaviour
{

    public static Action MashingFailed;
    public static Action MashingSuccessful;

    public Canvas canvas;
    public KeyCode keyCode;
    public float mashDelay = 0.5f;
    public float requiredTimes = 10;

    float mash;
    bool pressed = false;
    bool started = false;
    public int count;

    void Start()
    {
        Boss.BossTalking += StartMashing;
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
                MashingSuccessful?.Invoke();
            }

            if (mash <= 0)
            {
                StopMashing();
                MashingFailed?.Invoke();
            }
        }
    }

    private void StartMashing()
    {
        IEnumerator coroutine = WaitAndStart(2.0f);
        StartCoroutine(coroutine);
    }

    private void StopMashing()
    {
        started = false;
        canvas.gameObject.SetActive(false);
    }

    private IEnumerator WaitAndStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        started = true;
        canvas.gameObject.SetActive(true);
    }
}
