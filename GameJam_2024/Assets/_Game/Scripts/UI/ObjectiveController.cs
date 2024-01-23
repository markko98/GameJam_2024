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
        switch (type)
        {
            case ObjectiveType.Toilet:
                return "Get to the toilet before the time runs out!!";
            case ObjectiveType.ToiletPaper:
                return "Pick up toilet paper!!";
            case ObjectiveType.Magazine:
                return "Take some magazines off the kitchen counter!!";
            case ObjectiveType.Key:
                return "The toilet is locked! Get the key from Bob!!";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}
