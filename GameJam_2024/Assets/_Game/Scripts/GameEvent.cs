using System;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static Action EventSuccessfull;
    public static Action EventFailed;

    public EventType type;
    public Canvas canvas;
}

public enum EventType
{
    MatchText, ButtonMash
}