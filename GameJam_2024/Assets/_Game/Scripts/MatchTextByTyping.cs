using System;
using System.Collections;
using FMOD.Studio;
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

    public Transform ourTarget;
    
    private EventInstance mashSuccess;
    private EventInstance mashFail;
    private EventInstance keystroke;


    private void Start()
    {
        Boss.OnBossTalking += StartTyping;
        mashSuccess = AudioManager.Instance.CreateInstance(AudioProvider.Instance.mashSuccess, AudioSceneType.Gameplay);
        mashFail = AudioManager.Instance.CreateInstance(AudioProvider.Instance.mashFail, AudioSceneType.Gameplay);
        keystroke = AudioManager.Instance.CreateInstance(AudioProvider.Instance.keystroke, AudioSceneType.Gameplay);
    }

    void StartTyping(Transform target)
    {

        if (ourTarget != target) return;
        
        StartCoroutine(WaitAndStart());
    }

    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start Typing!");
        text.text = "Type " + expectedText + " to avoid colleague!";
        
        Keyboard.current.onTextInput += OnTextInput;
        isTyping = true;
    }

    void StopTyping()
    {
        Debug.Log("Stop Typing!");
        
        Keyboard.current.onTextInput -= OnTextInput;
        isTyping = false;
        writtenText = "";
        typingText.text = "";
        DelayedExecutionManager.ExecuteActionAfterDelay(1000, () => text.text = "");
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
                text.text = "You could not avoid the colleague!";
                StopTyping();
                mashFail.start();
                EventFailed?.Invoke();
            }
        }
    }

    private void OnTextInput(char ch)
    {
        if (expectedText == null || position >= expectedText.Length)
            return;

        keystroke.start();

        if (expectedText[position] == ch)
        {
            writtenText += ch;
            typingText.text = writtenText;
            ++position;
            if (position == expectedText.Length)
            {
                text.text = "You avoided the colleague!";
                StopTyping();
                EventSuccessfull?.Invoke();
                mashSuccess.start();
                Debug.Log("You wrote correctly");
            }
        }
        else
        {
            position = 0;
            text.text = "Wrong key is pressed!";

            StopTyping();
            // TODO
            Debug.Log("Wrong Key!");
            mashFail.start();
            EventFailed?.Invoke();
        }
    }
}
