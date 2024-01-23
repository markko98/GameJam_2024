using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveView : MonoBehaviour
{
    public bool isCompleted = false;
    [SerializeField] Image checkboxImage;
    [SerializeField] Sprite checkBoxCompleted;
    [SerializeField] Sprite checkBoxNotCompleted;
    [SerializeField] TextMeshProUGUI objectiveText;

    public void Setup(string objective)
    {
        isCompleted = false;
        checkboxImage.DOColor(ColorProvider.Instance.redColor, 0f);
        objectiveText.SetText(objective);
        UpdateView();
    }
    public void SetDone()
    {
        isCompleted = true;
        checkboxImage.DOColor(ColorProvider.Instance.greenColor, 0.25f);
        UpdateView();
    }

    private void UpdateView()
    {
        checkboxImage.sprite = isCompleted ? checkBoxCompleted : checkBoxNotCompleted;
    }
}
