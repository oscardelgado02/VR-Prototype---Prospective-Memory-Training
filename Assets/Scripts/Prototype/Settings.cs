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
    public bool enableDataExtraction = false;    // Variable to extract data from the game

    // Settings
    public int numOfTasksToDo = 3;
}
