using DG.Tweening;
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
        objectiveText.SetText(objective);
        UpdateView();
    }
    public void SetDone()
    {
        isCompleted = true;
        UpdateView();

        var sequence = DOTween.Sequence();
        sequence
            .Append(transform.DOScale(1.2f, 0.2f))
            .Append(transform.DOScale(1, 0.2f))
            .Append(transform.DOMove(new Vector3(-100f, transform.position.y, transform.position.z), 0.2f))
            .OnComplete(() => gameObject.transform.SetParent(null));
    }

    private void UpdateView()
    {
        checkboxImage.sprite = isCompleted ? checkBoxCompleted : checkBoxNotCompleted;
    }
}
