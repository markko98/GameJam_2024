using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ModalWindow
{
    static ModalWindow _instance;
    public static ModalWindow Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new ModalWindow();
            }
            return _instance;
        }
    }
    private ModalWindowView modalWindowPrefab;
    private Canvas ParentCanvas;

    ModalWindowView currentModal;
    ModalWindowView lastModal;

    Tween modalTweener;
    UnityEvent onEscapePressed = new UnityEvent();
    bool isEscapeCalled = false;

    public void InitModal()
    {
        modalWindowPrefab = Resources.Load<ModalWindowView>("ModalWindowView");
    }

    void CustomUpdate()
    {
        if (currentModal != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                onEscapePressed?.Invoke();
            }
        }
    }

    ModalWindowView CreateModalWindow()
    {
        ParentCanvas = GameObject.FindObjectOfType<Canvas>();
        if(ParentCanvas == null)
        {
            Debug.LogWarning("CANVAS MISSING FOR MODAL WINDOW");
        }
        var tempParent = ParentCanvas.transform;
        return GameObject.Instantiate(modalWindowPrefab, tempParent);
    }

    public void ShowModal(ModalData modalData)
    {
        showModal(modalData.title, modalData.message, modalData.option1, modalData.option2, modalData.option1Callback, modalData.option2Callback, modalData.hideCallback);
    }

    public void HideCurrentModal()
    {
        Hide(null);
    }

    void showModal(string title, string message, string option1, string option2, UnityAction option1Callback, UnityAction option2Callback, UnityAction hideCallback = null)
    {
        var modal = CreateModalWindow();
        currentModal = modal;
        currentModal.Modal.gameObject.SetActive(false);
        SetHeader(title);
        SetContent(message);
        SetFooter(option1, option2);
        var showTween = Show();
        // set button listeners after window pops up
        showTween.OnComplete(() => { SetCallbacks(option1Callback, option2Callback, hideCallback); });
    }

    #region Setup functions
    void SetHeader(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            currentModal.Modal_Header.gameObject.SetActive(false);
            return;
        }
        currentModal.Modal_Header_HeaderTXT.text = title;
    }
    void SetContent(string message)
    {
        if (string.IsNullOrEmpty(message))
        {
            currentModal.Modal_Content_Horizontal.gameObject.SetActive(false);
            return;
        }

        currentModal.Modal_Content_Horizontal.gameObject.SetActive(true);
        currentModal.Modal_Content_Horizontal_ContentHorizontalTXT.text = message;

    }
    void SetFooter(string option1, string option2)
    {
        currentModal.Modal_Footer_Option1BTN_Option1TXT.text = option1;
        currentModal.Modal_Footer_Option2BTN_Option2TXT.text = option2;
        currentModal.Modal_Footer_Option1BTN.gameObject.SetActive(option1 != "");
        currentModal.Modal_Footer_Option2BTN.gameObject.SetActive(option2 != "");
    }
    void SetCallbacks(UnityAction option1Callback, UnityAction option2Callback, UnityAction hideCallback)
    {
        currentModal.Modal_Header_ExitBTN.onClick.AddListener(() => { Hide(hideCallback); });
        currentModal.Modal_Footer_Option1BTN.onClick.AddListener(() => { option1Callback?.Invoke(); Hide(hideCallback); });
        currentModal.Modal_Footer_Option2BTN.onClick.AddListener(() => { option2Callback?.Invoke(); Hide(hideCallback); });
        onEscapePressed.AddListener(() => {
            EscapeCallback();
        });
    }

    private void EscapeCallback()
    {
        if (!isEscapeCalled)
        {
            isEscapeCalled = true;
            currentModal.Modal_Header_ExitBTN.onClick?.Invoke();
        }
    }
    #endregion

    #region Toggle functions
    Tween Show()
    {
        currentModal.Modal.rectTransform.transform.localScale = Vector3.zero;
        currentModal.Modal.gameObject.SetActive(true);
        var t = currentModal.Modal.rectTransform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        return t;
    }

    //TODO - fix logic for showing another window in the same time 
    void Hide(UnityAction hideCallback)
    {
        if (currentModal != null)
        {
            lastModal = currentModal;

            var t = currentModal.Modal.rectTransform.DOScale(0.25f, 0.35f).SetEase(Ease.InBack).OnComplete(()=> {
                isEscapeCalled = false;
                hideCallback?.Invoke();
                onEscapePressed.RemoveAllListeners();
                currentModal.Modal.gameObject.SetActive(false);
                GameObject.Destroy(currentModal.gameObject);
                currentModal = null;
            });
        }
    }
    #endregion

}
[System.Serializable]
public class ModalData
{
    public string title;
    public string message;
    public string option1 = "Option 1";
    public string option2 = "Option 2";
    public UnityAction option1Callback;
    public UnityAction option2Callback;
    public UnityAction hideCallback;
    public ModalData()
    {
    }
    public ModalData(string title, string message, UnityAction option1Callback, UnityAction option2Callback, UnityAction hideCallback)
    {
        this.title = title;
        this.message = message;
        this.option1Callback = option1Callback;
        this.option2Callback = option2Callback;
        this.hideCallback = hideCallback;
    }
    public ModalData(string title, string message)
    {
        this.title = title;
        this.message = message;
    }
}
