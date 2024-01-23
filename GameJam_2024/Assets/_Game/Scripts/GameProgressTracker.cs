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
            OnTimeChange?.Invoke(currentTime);
        }

    }

    [SerializeField] Transform timerHolder;
    [SerializeField] Slider timerSlider;
    [SerializeField] Image timerSliderFillImage;
    [SerializeField] TextMeshProUGUI timerText;

    [SerializeField] Color fullColor;
    [SerializeField] Color emptyColor;

    public Action OnTimeRunOut;
    public Action<float> OnTimeChange;

    bool didTimeRunOut = false;

    public void Setup()
    {
        CurrentTime = initialTime;
    }
    private void Update()
    {
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
