using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MainMenuController : USceneController
{    public MainMenuController() : base(SceneNames.MainMenu) { }
    private MainMenuOutlet outlet;
    private EventInstance mainMenuMusic;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true; 
        outlet = GameObject.Find(SceneOutlets.MainMenu).GetComponent<MainMenuOutlet>();

        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen();
        SetupSound();
        SetUIElements();
    }

    private void SetupSound()
    {
        //mainMenuMusic = AudioManager.Instance.CreateInstance(AudioProvider.Instance.mainMenuMusic, AudioSceneType.MainMenu);
        //mainMenuMusic.start();
        //TODO - future play button
    }

    private void SetUIElements()
    {
        outlet.startButton.onClick.AddListener(() =>
        {
            OpenGameplayScene();
        });

        outlet.quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void OpenGameplayScene()
    {
        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen(animated: false);
        presenter.ShowLoadingScreen(() =>
        {
            AudioManager.Instance.CleanUp(AudioSceneType.MainMenu);
            var gameplayScene = new GameplayController();
            PushSceneController(gameplayScene);
        });
    }
}
