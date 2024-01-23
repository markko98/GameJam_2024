using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UNavigationController
{
    private static readonly Stack<USceneController> _controllersStack = new Stack<USceneController>();
    public static USceneController ActiveController => _controllersStack.Peek();

    public static USceneController RootController { get; private set; }

    /// <summary>
    /// Open scene and add it to stack
    /// </summary>
    /// <param name="controller"></param>
    public static void PresentViewController(USceneController controller)
    {
        if (_controllersStack.Count == 0)
        {
            RootController = controller;
        }

        _controllersStack.Push(controller);

        SceneManager.LoadScene(controller.SceneName);
    }

    /// <summary>
    /// Close current scene and remove it from stack
    /// </summary>
    public static void RemoveViewController()
    {
        ActiveController.SceneWillDisappear();
        if (_controllersStack.Count > 0)
        {
            _controllersStack.Pop();
            ActiveController.RegisterLoad();
            SceneManager.LoadScene(ActiveController.SceneName);
        }
        else
        {
            //Debug.LogWarning("Navigational Stack is EMPTY! Loading Root VC!");
            SceneManager.LoadScene(RootController.SceneName);
        }
    }

    /// <summary>
    /// Set root scene
    /// </summary>
    /// <param name="controller"></param>
    public static void SetRootViewController(USceneController controller)
    {
        _controllersStack.Clear();

        controller.RegisterLoad();
        controller.SceneWillAppear();
        PresentViewController(controller);
    }

    /// <summary>
    /// Load root scene
    /// </summary>
    public static void PopToRootViewController()
    {
        ActiveController.SceneWillDisappear();
        _controllersStack.Clear();
        RootController.SceneWillAppear();
        RootController.RegisterLoad();
        PresentViewController(RootController);
    }
}
