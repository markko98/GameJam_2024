using System;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : USceneController
{
    public MainMenuController() : base(SceneNames.MainMenu) { }
    private MainMenuOutlet outlet;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.MainMenu).GetComponent<MainMenuOutlet>();

        var presenter = new LoadingScreenPresenter();
        presenter.HideLoadingScreen(() => {

            var modalData = new ModalData()
            {
                title = "Out of Energy",
                message = "Take a look at some of the options",
                option1 = "Watch an Add",
                option2 = "Buy refill",
                option1Callback = () => Debug.Log("CONFIRM"),
                option2Callback = () => Debug.Log("DENY"),
                hideCallback = () => Debug.Log("CLOSE"),
            };
            ModalWindow.Instance.ShowModal(modalData);
        });

        SetUIElements();
    }
    
    private void SetUIElements()
    {
    }
    private void OpenNextScene()
    {
        //var nextScene = new NextSceneController();
        //PushSceneController(nextScene);
    }
}
