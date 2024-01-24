using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchTextByTyping : GameEvent
{
    private int position = 0;
    public string expectedText;
    public float maxTime = 3;

    public bool isTyping;

    public TextMeshProUGUI typingText;
    public string writtenText = "";

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
        text.text = "Type " + expectedText + " to avoid boss!";
        Keyboard.current.onTextInput += OnTextInput;
        isTyping = true;
    }

    void StopTyping()
    {
        Debug.Log("Stop Typing!");
        text.text = "";
        Keyboard.current.onTextInput -= OnTextInput;
        isTyping = false;
        writtenText = "";
        typingText.text = "";
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
                text.text = "You could not avoid the boss!";
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
            writtenText += ch;
            typingText.text = writtenText;
            ++position;
            if (position == expectedText.Length)
            {
                text.text = "You avoided the boss!";
                StopTyping();
                EventSuccessfull?.Invoke();
                Debug.Log("You wrote correctly");
            }
        }
        else
        {
            expectedText = null;
            position = 0;
            text.text = "Wrong key is pressed!";

            StopTyping();
            // TODO
            Debug.Log("Wrong Key!");
            EventFailed?.Invoke();
        }
    }
}
