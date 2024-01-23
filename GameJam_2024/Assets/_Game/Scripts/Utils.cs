using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static string FormatTime(float seconds)
    {
        var timeSpan = TimeSpan.FromSeconds(seconds);
        return timeSpan.Hours > 0 ? $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}" : $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }
    public static float GetRandom(this Vector2 vector)
    {
        var random = new System.Random();
        return (float)random.NextDouble() * (vector.y - vector.x) + vector.x;
    }
}
