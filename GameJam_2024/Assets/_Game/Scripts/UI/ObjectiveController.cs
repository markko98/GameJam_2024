using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum ObjectiveType { Toilet, ToiletPaper, Magazine, Key }
public class ObjectiveController : MonoBehaviour
{
    [SerializeField] ObjectiveView objectivePrefab;
    [SerializeField] Transform objectiveHolder;

    Dictionary<ObjectiveType, ObjectiveView> objectives = new Dictionary<ObjectiveType, ObjectiveView>();

    public void CreateObjective(ObjectiveType objectiveType, string objectiveDescription)
    {
        var objective = Instantiate(objectivePrefab, objectiveHolder);
        objective.Setup(objectiveDescription);
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
}
