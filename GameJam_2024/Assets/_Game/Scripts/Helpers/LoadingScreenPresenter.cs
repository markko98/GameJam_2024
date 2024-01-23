using UnityEngine;
using DG.Tweening;
using System;

public enum LoadingScreenDirection { Vertical, Horizontal };
public class LoadingScreenPresenter
{
    public LoadingScreenPresenter()
    {
    }
    public LoadingScreenPresenter(LoadingScreenDirection loadingScreenDirection)
    {
        this.loadingScreenDirection = loadingScreenDirection;
    }

    public bool isShown;
    private LoadingScreenDirection loadingScreenDirection = LoadingScreenDirection.Horizontal;
    private float imageWidth;
    private float imageHeight;
    private float moveCounter
    {
        set
        {
            var leftTransform = leftImage.GetComponent<RectTransform>();
            var rightTransform = rightImage.GetComponent<RectTransform>();

            var delta = value - _moveCounter;
            if (loadingScreenDirection == LoadingScreenDirection.Horizontal)
            {
                leftTransform.position = new Vector3(leftTransform.position.x - delta, leftTransform.position.y, leftTransform.position.z);
                rightTransform.position = new Vector3(rightTransform.position.x + delta, rightTransform.position.y, rightTransform.position.z);
            }
            else if (loadingScreenDirection == LoadingScreenDirection.Vertical)
            {
                leftTransform.position = new Vector3(leftTransform.position.x, leftTransform.position.y - delta, leftTransform.position.z);
                rightTransform.position = new Vector3(rightTransform.position.x, rightTransform.position.y + delta, rightTransform.position.z);
            }

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
                imageHeight = _leftImage.GetComponent<RectTransform>().rect.height;
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
        if (loadingScreenDirection == LoadingScreenDirection.Horizontal)
        {
            DOTween.To(() => moveCounter, (x) => { moveCounter = x; }, 0.0f, 0.6f).SetEase(Ease.InOutExpo).OnComplete(() =>
            {
                isShown = true;
                block?.Invoke();
            });
        }
        else if (loadingScreenDirection == LoadingScreenDirection.Vertical)
        {
            DOTween.To(() => moveCounter, (y) => { moveCounter = y; }, 0.0f, 0.6f).SetEase(Ease.InOutExpo).OnComplete(() =>
            {
                isShown = true;
                block?.Invoke();
            });
        }
    }

    public void HideLoadingScreen(Action block = null, bool animated = true)
    {
        leftImage.SetActive(true);
        rightImage.SetActive(true);

        if (!animated)
        {
            moveCounter = loadingScreenDirection == LoadingScreenDirection.Horizontal ? imageWidth : imageHeight;
            return;
        }

        if (loadingScreenDirection == LoadingScreenDirection.Horizontal)
        {
            DOTween.To(() => moveCounter, (x) => { moveCounter = x; }, imageWidth, 0.8f).SetEase(Ease.InOutExpo).OnComplete(() => {
                isShown = false;
                leftImage.SetActive(false);
                rightImage.SetActive(false);
                block?.Invoke();
            });
        }
        else if (loadingScreenDirection == LoadingScreenDirection.Vertical)
        {
            DOTween.To(() => moveCounter, (y) => { moveCounter = y; }, imageHeight, 0.8f).SetEase(Ease.InOutExpo).OnComplete(() => {
                isShown = false;
                leftImage.SetActive(false);
                rightImage.SetActive(false);
                block?.Invoke();
            });
        }
    }
}

