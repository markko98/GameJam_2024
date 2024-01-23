using FMOD.Studio;
using System;
using UnityEngine;

public class GameplayController : USceneController
{
    public GameplayController() : base(SceneNames.Gameplay) { }
    private GameplayOutlet outlet;

    private EventInstance officeAmbient;
    private EventInstance stomachGrowlLoop;
    private EventInstance stomachGrowl;
    private EventInstance fartSound;

    private ObjectiveController objectiveController = new ObjectiveController();

    private bool pauseMenuCalled;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.Gameplay).GetComponent<GameplayOutlet>();

        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen();

        outlet.gameProgressTracker.Setup();

        outlet.gameProgressTracker.OnTimeRunOut += OnTimerRanOut;
        outlet.goalTrigger.GoalReached += OnGoalTriggerReached;
        
        SetupSound();
        SetUIElements();

        GameTicker.SharedInstance.Update += Update;
    }

    private void Update()
    {
        if (pauseMenuCalled || !Input.GetKeyDown(KeyCode.Escape)) return;
        
        pauseMenuCalled = true;
        var pauseMenuController = new PauseMenuController();
        AddChildSceneController(pauseMenuController);
    }

    public void OnObjectiveComplete(ObjectiveType type)
    {
        objectiveController.CompleteObjective(type);

        var addedTime = ObjectiveController.GetTimeAddValueForObjective(type);
        outlet.gameProgressTracker.AddTime(addedTime);
    }

    public void OnGoalTriggerReached()
    {
        if (objectiveController.CanFinishGame())
        {
            // SHow same success screen;
            var endGameDetails = new EndGameDetails()
            {
                isVictory = true,
                timeRemaining = outlet.gameProgressTracker.CurrentTime
            };
            var endGameController = new EndGameController(endGameDetails);
            AddChildSceneController(endGameController);
            return;
        }

        Debug.Log("Still playing!!");
        // Highlight wanted objects?
    }

    public void OnTimerRanOut()
    {
        var endGameDetails = new EndGameDetails()
        {
            isVictory = false,
            timeRemaining = outlet.gameProgressTracker.CurrentTime
        };
        var endGameController = new EndGameController(endGameDetails);
        PushSceneController(endGameController);
    }

    private void SetupObjectives()
    {
        foreach (var objective in Enum.GetValues(typeof(ObjectiveType)))
        {
            objectiveController.CreateObjective((ObjectiveType)objective, outlet.objectiveHolder);
        }

        Collectable.Collected += OnObjectiveComplete;
    }

    private void SetupSound()
    {
        officeAmbient = AudioManager.Instance.CreateInstance(AudioProvider.Instance.officeAmbient, AudioSceneType.Gameplay);
        stomachGrowlLoop = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSoundLoop, AudioSceneType.Gameplay);
        stomachGrowl = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSound, AudioSceneType.Gameplay);
        fartSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.fartSound, AudioSceneType.Gameplay);

        officeAmbient.start();
        stomachGrowlLoop.start();
    }

    private void SetUIElements()
    {
        SetupObjectives();
    }

    public override void SceneWillDisappear()
    {
        base.SceneWillDisappear();
        Collectable.Collected -= OnObjectiveComplete;
        outlet.goalTrigger.GoalReached -= OnGoalTriggerReached;
        outlet.gameProgressTracker.OnTimeRunOut -= OnTimerRanOut;
        GameTicker.SharedInstance.Update -= Update;

    }
}
