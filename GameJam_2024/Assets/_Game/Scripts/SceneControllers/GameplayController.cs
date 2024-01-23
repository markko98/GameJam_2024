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
    }

    public void OnObjectiveComplete(ObjectiveType type)
    {
        objectiveController.CompleteObjective(type);    
    }

    public void OnGoalTriggerReached()
    {
        if (objectiveController.CanFinishGame())
        {
            // SHow same success screen;
            Debug.Log("Done");
            return;
        }

        Debug.Log("Still playing!!");
        // Highlight wanted objects?
    }

    public void OnTimerRanOut()
    {
        Debug.Log("Game Over");
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

    public override void SceneWillAppear()
    {
        base.SceneWillAppear();

        Collectable.Collected -= OnObjectiveComplete;
        outlet.goalTrigger.GoalReached -= OnGoalTriggerReached;
        outlet.gameProgressTracker.OnTimeRunOut -= OnTimerRanOut;


    }
}
