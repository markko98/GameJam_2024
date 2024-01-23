using System;
using UnityEngine;

public class PauseMenuController: USceneController
{
    public PauseMenuController() : base(SceneNames.PauseMenu) { }
    private PauseMenuOutlet outlet;

    private float maxTime = 3f;
    private float currentTime;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.PauseMenuOutlet).GetComponent<PauseMenuOutlet>();
        
        SetupUIElements();

        GameTicker.TimeScale = 0;

        currentTime = maxTime;
        GameTicker.SharedInstance.Update += Update;
    }

    private void SetupUIElements()
    {
        outlet.warningText.SetText($"DID YOU REALLY PAUSE THE GAME IN TIMES LIKE THIS??? \n FINE I'LL GIVE YOU {0} SECONDS TO REST", maxTime);
        outlet.timeRemainingText.SetText(Math.Round(maxTime).ToString());
    }

    private void Update()
    {
        currentTime -= Time.deltaTime;
        outlet.timeRemainingText.SetText(Math.Round(currentTime).ToString());
        
        if (currentTime > 0) return;
        
        currentTime = 0;
        RemoveFromParentSceneController();
    }

    public override void SceneWillDisappear()
    {
        base.SceneWillDisappear();
        GameTicker.SharedInstance.Update -= Update;
        GameTicker.TimeScale = 1;

    }
}