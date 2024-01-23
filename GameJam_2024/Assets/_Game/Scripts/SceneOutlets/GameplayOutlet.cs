using StarterAssets;
using UnityEngine;

public class GameplayOutlet : SceneControllerOutlet
{
    public GameProgressTracker gameProgressTracker;
    public Transform objectiveHolder;
    public GoalTrigger goalTrigger;
    public Player playerRef;
    public ThirdPersonController playerController;
    public Boss bossRef;
}
