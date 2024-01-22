using System;
using UnityEngine;

public class EntryPointController : AppDelegate
{
    private EntryPointOutlet outlet;
    
    public override void ApplicationStarted()
    {
        Application.targetFrameRate = 60;        
        outlet = GameObject.Find(SceneOutlets.EntryPoint).GetComponent<EntryPointOutlet>();

#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
        SetupServicesAndProviders();
    }

    void SetupServicesAndProviders()
    {
        //setup and prewarm providers
        //SpriteProvider.Prewarm();
        ModalWindow.Instance.InitModal();
        LoadDefaultScene();
    }

    private void LoadDefaultScene()
    {
        var presenter = new LoadingScreenPresenter();
        presenter.HideLoadingScreen(animated: false);
        presenter.ShowLoadingScreen(() =>
        {
            var mainMenuVc = new MainMenuController();
            UNavigationController.SetRootViewController(mainMenuVc);
        });
    }

    public override void ApplicationWillGoToBackground()
    {
        base.ApplicationWillGoToBackground();
    }
    
    public override void ApplicationWillQuit()
    {
        base.ApplicationWillQuit();
    }

    public override void ApplicationIsRestoredFromBackground()
    {
        base.ApplicationIsRestoredFromBackground();
    }

}