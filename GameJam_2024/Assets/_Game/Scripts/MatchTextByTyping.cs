using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchTextByTyping : GameEvent
{
    private int position = 0;
    public string expectedText;
    public float maxTime = 3;

    public bool isTyping;

    private void Start()
    {
        Boss.OnBossTalking += StartTyping;
    }

    void StartTyping()
    {
        StartCoroutine(WaitAndStart());
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start Typing!");
        canvas.gameObject.SetActive(true);
        Keyboard.current.onTextInput += OnTextInput;
        isTyping = true;
    }

    void StopTyping()
    {
        Debug.Log("Stop Typing!");
        canvas.gameObject.SetActive(false);
        Keyboard.current.onTextInput -= OnTextInput;
        isTyping = false;
    }

    private void Update()
    {
        if (isTyping)
        {
            maxTime -= Time.deltaTime;
            if (maxTime < 0)
            {
                // TODO - inform player
                Debug.Log("Time is up!");
                StopTyping();
                EventFailed?.Invoke();
            }
        }
    }

    private void OnTextInput(char ch)
    {
        if (expectedText == null || position >= expectedText.Length)
            return;

        if (expectedText[position] == ch)
        {
            ++position;
            if (position == expectedText.Length)
            {
                StopTyping();
                EventSuccessfull?.Invoke();
                Debug.Log("You wrote correctly");
            }
        }
        else
        {
            expectedText = null;
            position = 0;
            StopTyping();
            // TODO
            Debug.Log("Wrong Key!");

            EventFailed?.Invoke();
        }
    }
}
