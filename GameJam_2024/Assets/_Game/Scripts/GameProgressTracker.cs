using DG.Tweening;
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
            OnTimeChange?.Invoke(currentTime, initialTime);
            OnTimeChangePercentageLeft?.Invoke(1f - (currentTime / initialTime));
        }

    }

    [SerializeField] Transform timerHolder;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image timerSliderFillImage;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] Color fullColor;
    [SerializeField] Color emptyColor;

    bool didGameStart = false;

    public Action OnTimeRunOut;
    public Action<float, float> OnTimeChange;
    public Action<float> OnTimeChangePercentageLeft;

    bool didTimeRunOut = false;

    public void Setup()
    {
        didGameStart = true;
        CurrentTime = initialTime;
    }
    private void Update()
    {
        if (!didGameStart) return;

        DecreaseTime(GameTicker.DeltaTime);
    }
    private void UpdateView()
    {
        timerText.SetText(Utils.FormatTime(CurrentTime));
        timerSlider.DOValue(CurrentTime / initialTime, 0.25f);
        var endColor = Color.Lerp(emptyColor, fullColor, CurrentTime/initialTime);
        timerSliderFillImage.DOColor(endColor, 0.25f);
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
