using UnityEngine;
using DG.Tweening;
using System;

public class LoadingScreenPresenter
{
    public bool isShown;
    private float imageWidth;
    private float moveCounter
    {
        set
        {
            var leftTransform = leftImage.GetComponent<RectTransform>();
            var rightTransform = rightImage.GetComponent<RectTransform>();

            var delta = value - _moveCounter;
            leftTransform.position = new Vector3(leftTransform.position.x - delta, leftTransform.position.y, leftTransform.position.z);
            rightTransform.position = new Vector3(rightTransform.position.x + delta, rightTransform.position.y, rightTransform.position.z);
        
            _moveCounter = value;
        }

        get
        {
            return _moveCounter;
        }
    }

    private float _moveCounter;

    private GameObject leftImage
    {
        get
        {
            if (_leftImage == null)
            {
                _leftImage = GameObject.Instantiate(Resources.Load("UI/Splash_Left"), GameObject.FindObjectOfType<Canvas>().transform, false) as GameObject;
                imageWidth = _leftImage.GetComponent<RectTransform>().rect.width;
            }
            return _leftImage;
        }
    }
    private GameObject rightImage
    {
        get
        {
            if (_rightImage == null)
            {
                _rightImage = GameObject.Instantiate(Resources.Load("UI/Splash_Right"), GameObject.FindObjectOfType<Canvas>().transform, false) as GameObject;
            }
            return _rightImage;
        }
    }

    private GameObject _leftImage;
    private GameObject _rightImage;

    public void ShowLoadingScreen(Action block, bool animated = true)
    {
        leftImage.SetActive(true);
        rightImage.SetActive(true);
        if (!animated)
        {
            moveCounter = 0.0f;
            return;
        }
        DOTween.To(() => moveCounter, (x) => { moveCounter = x; }, 0.0f, 0.6f).SetEase(Ease.InOutExpo).OnComplete(() => {
            isShown = true;
            block?.Invoke();
        });
    }

    public void HideLoadingScreen(Action block = null, bool animated = true)
    {
        leftImage.SetActive(true);
        rightImage.SetActive(true);

        if (!animated)
        {
            moveCounter = imageWidth;
            return;
        }

        DOTween.To(() => moveCounter, (x) => { moveCounter = x; }, imageWidth, 0.8f).SetEase(Ease.InOutExpo).OnComplete(() => {
            isShown = false;
            leftImage.SetActive(false);
            rightImage.SetActive(false);
            block?.Invoke();
        });
    }
}

