using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Settings
{
    private Settings() {}

    private static Settings instance;
    public static Settings Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Settings();
            }
            return instance;
        }
    }

    // Debug Settings
    public bool xrDeviceSimulator = true;  //variable to test the game without a VR headset

    // Settings
    public bool enableDataExtraction = true;
    public int numOfTasksToDo = 3;
}
