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
        
        SetupUIElements();
    }

    private void SetupUIElements()
    {
        outlet.titleText.SetText(details.isVictory ? "Congratulations" : "You haven't made it in time :(");
        outlet.timeRemainingText.SetText("Time remaining: " + details.timeRemaining);
        
        
        outlet.quitButton.onClick.AddListener(UNavigationController.PopToRootViewController);
    }
}

public struct EndGameDetails
{
    public float timeRemaining;
    public bool isVictory;
}