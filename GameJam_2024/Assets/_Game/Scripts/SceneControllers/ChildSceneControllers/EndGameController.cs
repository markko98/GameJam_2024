using System;
using UnityEngine;

public class EndGameController: USceneController
{
    public EndGameController(EndGameDetails details) : base(SceneNames.EndGame)
    {
        this.details = details;
    }

    private EndGameOutlet outlet;
    private EndGameDetails details;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.EndGameOutlet).GetComponent<EndGameOutlet>();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SetupUIElements();
    }

    private void SetupUIElements()
    {
        outlet.titleText.SetText(details.isVictory ? "Congratulations" : "You haven't made it in time :(");
        
        var seconds = (int)details.timeRemaining % 60;
        var minutes = (int)details.timeRemaining / 60;
        outlet.timeRemainingText.SetText("Time remaining: " + string.Format("{0:00}:{0:00}", minutes, seconds));
        
        
        outlet.quitButton.onClick.AddListener(UNavigationController.PopToRootViewController);
    }
}

public struct EndGameDetails
{
    public float timeRemaining;
    public bool isVictory;
}