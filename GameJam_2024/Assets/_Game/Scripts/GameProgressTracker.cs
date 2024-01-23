using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressTracker : MonoBehaviour
{
    [SerializeField] float initialTime = 10;
    [SerializeField] float currentTime;
    public float CurrentTime
    {
        get { return currentTime; }
        private set {
            currentTime = value;
            if (currentTime <= 0)
            {
                currentTime = 0;
                didTimeRunOut = true;
                OnTimeRunOut?.Invoke();
            }
            UpdateView();
            OnTimeChange?.Invoke(currentTime);
        }

    }

    [SerializeField] Transform timerHolder;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image timerSliderFillImage;
    [SerializeField] TextMeshProUGUI timerText;

    public static System.Action OnTimeRunOut;
    public static System.Action<float> OnTimeChange;

    bool didTimeRunOut = false;

    public void Setup()
    {
        CurrentTime = initialTime;
    }
    private void Update()
    {
        DecreaseTime(Time.deltaTime);
    }
    private void UpdateView()
    {
        timerText.SetText(Utils.FormatTime(CurrentTime));
        timerSlider.value = CurrentTime / initialTime;
    }

    public void AddTime(float amount)
    {
        if (didTimeRunOut) { return; }

        CurrentTime += amount;
    }

    public void DecreaseTime(float amount)
    {
        if (didTimeRunOut) { return; }

        CurrentTime -= amount;
    }
}
