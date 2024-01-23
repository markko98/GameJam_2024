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
    string tutorialText = "1.Limber up with a quick office stretch, but not for too long.\r\n\r\n2. Gather essential items on your way. A clever pooper knows what's needed for a successful bathroom mission.\r\n\r\n3. Celebrate with a well-deserved relief!";

    private bool pauseMenuCalled;

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.Gameplay).GetComponent<GameplayOutlet>();

        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen(animated: false);
        presenter.ShowLoadingScreen(() =>
        { 
            var modalData = new ModalData()
            {
                title = "Tutorial",
                message = tutorialText,
                option1 = "Okay",
                option1Callback = () => {
                    StartGame();
                },
            };
            ModalWindow.Instance.ShowModal(modalData);
        });
        

        outlet.gameProgressTracker.OnTimeRunOut += OnTimerRanOut;
        outlet.goalTrigger.GoalReached += OnGoalTriggerReached;
        
        SetupSound();
    }

    private void StartGame()
    {
        outlet.gameProgressTracker.Setup();
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
