using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
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

    // Attributes
    public bool xrDeviceSimulator = true;  //variable to test the game without a VR headset
    public int numOfTasksToDo = 3;
}
