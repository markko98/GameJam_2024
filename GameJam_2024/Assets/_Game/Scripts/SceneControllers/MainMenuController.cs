using FMOD.Studio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : USceneController
{    public MainMenuController() : base(SceneNames.MainMenu) { }
    private MainMenuOutlet outlet;
    private EventInstance mainMenuMusic;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
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
//() => {

//    var modalData = new ModalData()
//    {
//        title = "Out of Energy",
//        message = "Take a look at some of the options",
//        option1 = "Watch an Add",
//        option2 = "Buy refill",
//        option1Callback = () => Debug.Log("CONFIRM"),
//        option2Callback = () => Debug.Log("DENY"),
//        hideCallback = () => Debug.Log("CLOSE"),
//    };
//    ModalWindow.Instance.ShowModal(modalData);
//}
