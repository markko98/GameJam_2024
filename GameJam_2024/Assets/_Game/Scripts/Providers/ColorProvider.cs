using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorProvider : MonoBehaviour
{
    public Color greenColor;
    public Color redColor;

    private static ColorProvider _instance;
    public static ColorProvider Instance
    {
        get
        {
            if (_instance == null)
            {
                Prewarm();
            }

            return _instance;
        }
    }
    public static void Prewarm()
    {
        if (_instance != null)
        {
            return;
        }

        _instance = Resources.Load<ColorProvider>(Strings.AssetProvidersPath + "ColorProvider");
        DontDestroyOnLoad(_instance);
    }
}
