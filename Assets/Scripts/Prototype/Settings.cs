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
    public bool xrDeviceSimulator = false;  // Variable to test the game without a VR headset
    public bool enableDataExtraction = true;    // Variable to extract data from the game

    // Settings
    public int numOfTasksToDo = 3;
}
