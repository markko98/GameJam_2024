using System;
using TMPro;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static Action EventSuccessfull;
    public static Action EventFailed;

    public EventType type;
    public TextMeshProUGUI text;
}

public enum EventType
{
    MatchText, ButtonMash
}