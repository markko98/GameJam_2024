using FMOD.Studio;
using System;
using UnityEngine;

public class GameplayController : USceneController
{
    public GameplayController() : base(SceneNames.Gameplay) { }
    private GameplayOutlet outlet;
    private EventInstance officeIntensityMusic;
    private EventInstance officeAmbient;
    private EventInstance stomachGrowlLoop;
    private EventInstance stomachGrowl;
    private EventInstance fartSound;
    
    private EventInstance winSound;
    private EventInstance failSound;
    private EventInstance clockAmbient;
    private EventInstance collectablePickup;


    private ObjectiveController objectiveController = new ObjectiveController();
    string tutorialText = "1.Limber up with a quick office stretch, but not for too long.\r\n\r\n2. Gather essential items on your way. A clever pooper knows what's needed for a successful bathroom mission.\r\n\r\n3. Celebrate with a well-deserved relief!";
    string storyText = "In the midst of a Zoom meeting, Dave's stomach churned ominously as he realized he'd eaten something disagreeable. Panic set in, cramps seizing his legs, turning the dash to the bathroom into a survival mission.\n\nDiscomfort escalated with each passing minute as colleagues droned on, oblivious. In a code red situation, Dave discreetly excused himself, earning curious glances from coworkers.";
    private bool pauseMenuCalled;
    private Vector2 randomFartDelay = new Vector2(3, 8);
    private float timerFart = 0;
    private float timeForFart;
    private float timerGrowl;
    private float timeForGrowl;

    private DisposeBag disposeBag = new DisposeBag();

    public override void SceneDidLoad()
    {
        base.SceneDidLoad();
        outlet = GameObject.Find(SceneOutlets.Gameplay).GetComponent<GameplayOutlet>();

        outlet.playerController.CanWalk = false;

        var presenter = new LoadingScreenPresenter(LoadingScreenDirection.Vertical);
        presenter.HideLoadingScreen();

        ShowStoryModal();

        outlet.gameProgressTracker.OnTimeChangePercentageLeft += TimeChangedPercentage;
        outlet.gameProgressTracker.OnTimeRunOut += OnTimerRanOut;
        outlet.goalTrigger.GoalReached += OnGoalTriggerReached;

        SetupSound();
    }

    private void TimeChangedPercentage(float timePercentage)
    {
        officeIntensityMusic.setParameterByName("TimeLeft", timePercentage);
    }

    private void StartGame()
    {
        outlet.gameProgressTracker.Setup();
        SetUIElements();
        outlet.playerController.CanWalk = true;

        timerFart = 0;
        timerGrowl = 0;
        timeForFart = randomFartDelay.GetRandom();
        timeForGrowl = randomFartDelay.GetRandom();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GameTicker.SharedInstance.Update += Update;
        GameTicker.SharedInstance.Update += TimersUpdate;
        officeIntensityMusic.start();
    }

    private void TimersUpdate()
    {
        timerFart += GameTicker.DeltaTime;
        if (timerFart > timeForFart)
        {
            timerFart = 0;
            timeForFart = randomFartDelay.GetRandom();
            fartSound.start();
        }
        timerGrowl += GameTicker.DeltaTime;
        if (timerGrowl > timeForGrowl)
        {
            timerGrowl = 0;
            timeForGrowl = randomFartDelay.GetRandom();
            stomachGrowl.start();
        }
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
        collectablePickup.start();
        objectiveController.CompleteObjective(type);

        var addedTime = ObjectiveController.GetTimeAddValueForObjective(type);
        outlet.gameProgressTracker.AddTime(addedTime);
    }

    public void OnGoalTriggerReached()
    {
        if (objectiveController.CanFinishGame())
        {
            objectiveController.CompleteObjective(ObjectiveType.Toilet);
            var endGameDetails = new EndGameDetails()
            {
                isVictory = true,
                timeRemaining = outlet.gameProgressTracker.CurrentTime
            };

            winSound.start();
            AudioManager.Instance.CleanUp(AudioSceneType.Gameplay);
            var endGameController = new EndGameController(endGameDetails);
            AddChildSceneController(endGameController);
            return;
        }

        Debug.Log("Still playing!!");
        // Highlight wanted objects?
    }

    public void OnTimerRanOut()
    {
        outlet.playerRef.Shit();

        DelayedExecutionManager.ExecuteActionAfterDelay(6000, () =>
        {
            var endGameDetails = new EndGameDetails()
            {
                isVictory = false,
                timeRemaining = outlet.gameProgressTracker.CurrentTime
            };

            failSound.start();
            var endGameController = new EndGameController(endGameDetails);
            AddChildSceneController(endGameController);
        }).disposeBy(disposeBag);
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
        officeIntensityMusic = AudioManager.Instance.CreateInstance(AudioProvider.Instance.intensityOfficeMusic, AudioSceneType.Gameplay);
        officeAmbient = AudioManager.Instance.CreateInstance(AudioProvider.Instance.officeAmbient, AudioSceneType.Gameplay);
        stomachGrowlLoop = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSoundLoop, AudioSceneType.Gameplay);
        stomachGrowl = AudioManager.Instance.CreateInstance(AudioProvider.Instance.stomachSound, AudioSceneType.Gameplay);
        fartSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.fartSound, AudioSceneType.Gameplay);

        winSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.winToilet, AudioSceneType.Gameplay);
        failSound = AudioManager.Instance.CreateInstance(AudioProvider.Instance.fail, AudioSceneType.Gameplay);
        clockAmbient =
            AudioManager.Instance.CreateInstance(AudioProvider.Instance.clockTicking, AudioSceneType.Gameplay);
        collectablePickup = AudioManager.Instance.CreateInstance(AudioProvider.Instance.collectablePickup, AudioSceneType.Gameplay);


        officeAmbient.start();
        clockAmbient.start();
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
        GameTicker.SharedInstance.Update -= TimersUpdate;

    }

    public void ShowStoryModal()
    {
        var modalData = new ModalData()
        {
            title = "Monday Morning",
            message = storyText,
            option1 = "Okay",
            option2 = "",
            option1Callback = ShowTutorialModal,
            option2Callback = ShowTutorialModal
        };
        ModalWindow.Instance.ShowModal(modalData);
    }

    public void ShowTutorialModal()
    {
        DelayedExecutionManager.ExecuteActionAfterDelay(1000, () =>
        {
            var modalData = new ModalData()
            {
                title = "Tutorial",
                message = tutorialText,
                option1 = "Okay",
                option2 = "",
                option1Callback = StartGame,
                option2Callback = StartGame
            };
            ModalWindow.Instance.ShowModal(modalData);
        });

    }
}
