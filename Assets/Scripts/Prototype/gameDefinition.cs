using System.Collections.Generic;

// Enums definitions
public enum phaseIds { modifyingSettings, learning, doingTasks, end };
public enum eventIds { start, taskFinished, end };

// Labels definitions
public sealed class labels
{
    private labels() { }

    private static labels instance;
    public static labels Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new labels();
            }
            return instance;
        }
    }

    // Attributtes
    public readonly string[] _phaseLabels = new string[]
    {
        "Modifying Settings",
        "Learning",
        "Doing Tasks",
        "End"
    };
    public readonly string[] _eventLabels = new string[]
    {
        "Start",
        "Task Finished",
        "End"
    };
    public readonly string[] _taskLabels = new string[]
    {
        "Tap the laptop to send an email.",
        "Grab the shoes next to the sofa and put them inside the washing machine.",
        "Grab the plates that are on the kitchen table and store them in the cupboard next to the fridge.",
        "Put the painting on the wall between the wardrobe and the bathroom door.",
        "Drink the coffee that is above the dishwasher",
        "Eat the lemon that is inside the fridge",
        "Eat the apple that is inside the fridge",
        "Eat the melon that is inside the fridge"
    };
    public readonly string[] _csvHeaders = new string[]
    {
        "Event",
        "Phase",
        "Task ID",
        "Task description",
        "Task completed by the user",
        "Task in list",
        "Time"
    };
    public readonly string[] _objectTags = new string[]
    {
        "Shoe",
        "Plate",
        "Painting"
    };
    public readonly string[] _containerTags = new string[]
    {
        "Washing machine",
        "Cupboard",
        "Wall"
    };
}

public sealed class gameStatus
{
    private gameStatus() { }

    private static gameStatus instance;
    public static gameStatus Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new gameStatus();
            }
            return instance;
        }
    }

    // Attributtes
    public phaseIds currPhaseId = phaseIds.modifyingSettings;
    public List<int> selectedTasksId = new List<int>();
}