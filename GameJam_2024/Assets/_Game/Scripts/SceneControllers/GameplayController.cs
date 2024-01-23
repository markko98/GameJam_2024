using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : USceneController
{
    public GameplayController() : base(SceneNames.Gameplay) { }
    private GameplayOutlet outlet;

    private EventInstance officeAmbient;
    private EventInstance ventillationAmbient;
    private EventInstance stomachGrowlLoop;
    private EventInstance stomachGrowl;
    private EventInstance fartSound;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.Gameplay).GetComponent<GameplayOutlet>();

        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen();

        outlet.gameProgressTracker.Setup();

        SetupSound();
        SetUIElements();
    }

    private void SetupSound()
    {
        officeAmbient = AudioManager.Instance.CreateInstance(AudioProvider.Instance.officeAmbient, AudioSceneType.Gameplay);
        ventillationAmbient = AudioManager.Instance.CreateInstance(AudioProvider.Instance.ventillationAmbient, AudioSceneType.Gameplay);
        stomachGrowlLoop = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSoundLoop, AudioSceneType.Gameplay);
        stomachGrowl = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSound, AudioSceneType.Gameplay);
        fartSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.fartSound, AudioSceneType.Gameplay);

        officeAmbient.start();
        ventillationAmbient.start();
        stomachGrowlLoop.start();
    }

    private void SetUIElements()
    {

    }
}
