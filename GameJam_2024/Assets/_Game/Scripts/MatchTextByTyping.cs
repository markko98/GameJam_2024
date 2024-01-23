using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchTextByTyping : MonoBehaviour
{
    public static Action OnTextTypedCorrectly;
    public static Action OnTextTypedIncorrectly;

    private int position = 0;
    public string expectedText;
    public float maxTime = 3;

    public Canvas canvas;
    public bool isTyping;

    private void Start()
    {
        Boss.OnBossTalking += StartTyping;
        Boss.OnBossFainted += StopTyping;
    }

    void StartTyping()
    {
        StartCoroutine(WaitAndStart());
    }

    IEnumerator WaitAndStart()
    {

        yield return new WaitForSeconds(1);
        canvas.gameObject.SetActive(true);
        Keyboard.current.onTextInput += OnTextInput;
        isTyping = true;
    }

    void StopTyping()
    {
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
                // TODO
                Debug.Log("Time is up!");
                StopTyping();
                OnTextTypedIncorrectly?.Invoke();
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
                isTyping = false;
                StopTyping();
                OnTextTypedCorrectly?.Invoke();
            }

        }
        else
        {
            expectedText = null;
            position = 0;
            isTyping = false;
            // TODO
            Debug.Log("Wrong Key!");

            OnTextTypedIncorrectly?.Invoke();
        }
    }
}
