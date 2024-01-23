using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ObjectiveType { Toilet, ToiletPaper, Magazine, Key }

public class ObjectiveController
{
    private GameObject objectivePrefab;
    private readonly Dictionary<ObjectiveType, ObjectiveView> objectives = new Dictionary<ObjectiveType, ObjectiveView>();

    public void CreateObjective(ObjectiveType objectiveType, Transform parent)
    {
        objectivePrefab = Resources.Load<GameObject>(Strings.UIPath + "Objective");
        var objective = GameObject.Instantiate(objectivePrefab, parent).GetComponent<ObjectiveView>();
        objective.Setup(GetDescriptionForObjective(objectiveType));
        objectives.Add(objectiveType, objective);
    }

    public void CompleteObjective(ObjectiveType objectiveType)
    {
        if(objectives.TryGetValue(objectiveType, out ObjectiveView objectiveView))
        {
            objectiveView.SetDone();
        }
        else
        {
            Debug.LogWarning("No objective of type " + objectiveType.ToString());
        }
    }

    public bool CanFinishGame()
    {
        return objectives.All(objective  => objective.Value.isCompleted);
    }

    public static string GetDescriptionForObjective(ObjectiveType type)
    {
        return type switch
        {
            ObjectiveType.Toilet => "Get to the toilet before the time runs out!!",
            ObjectiveType.ToiletPaper => "Pick up toilet paper!!",
            ObjectiveType.Magazine => "Take some magazines off the kitchen counter!!",
            ObjectiveType.Key => "The toilet is locked! Get the key from Bob!!",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static int GetTimeAddValueForObjective(ObjectiveType type)
    {
        return type switch
        {
            ObjectiveType.Toilet => 1,
            ObjectiveType.ToiletPaper => 15,
            ObjectiveType.Magazine => 5,
            ObjectiveType.Key => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
