using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerOutlet : MonoBehaviour
{
    public Canvas canvas;
    public bool shouldResetApp;

    private void Awake()
    {
        if (shouldResetApp && !AppDelegate.appStarted)
        {
            SceneManager.LoadScene(SceneNames.EntryPoint);
        }
    }
}
