using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum language { english, spanish };

public class Translation
{
    public string sentence { get; private set; }
    public language lang { get; private set; }
    public Translation(language lang, string sentence)
    {
        this.sentence = sentence;
        this.lang = lang;
    }
}

public class Sentence
{
    public List<Translation> translations = new List<Translation>();
    public Sentence(List<Translation> translations) { this.translations = translations; }
}

public class UISentence
{
    public Sentence sentence { get; private set; }
    public TextMeshProUGUI textUI { get; private set; }
    public UISentence(Sentence sentence, TextMeshProUGUI textUI)
    {
        this.sentence = sentence;
        this.textUI = textUI;
    }

    public void TranslateUIText(language inputLang) { textUI.text = sentence.translations[(int)inputLang].sentence; }
}

public class UIGroup
{
    public List<UISentence> uiSentences = new List<UISentence>();
    public UIGroup() { }
}

public class SettingsUIGroup : UIGroup
{
    public SettingsUIGroup(TextMeshProUGUI settingsInfo, TextMeshProUGUI numOfTasksToDo, TextMeshProUGUI startText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "Welcome to the Prospective Memory Training VR Prototype!\r\n\r\nYou can edit here the settings of the game. Once you are finished, press the \"Start\" button with the Trigger button from your controller."),
                new Translation(language.spanish, "Welcome to the Prospective Memory Training VR Prototype!\r\n\r\nYou can edit here the settings of the game. Once you are finished, press the \"Start\" button with the Trigger button from your controller.")
            })
            ,settingsInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "Number of tasks to do"),
                new Translation(language.spanish, "Número de tareas")
            })
            ,numOfTasksToDo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "START"),
                new Translation(language.spanish, "EMPEZAR")
            })
            ,startText)
        };
    }
}

public class TaskListUIGroup : UIGroup
{
    public TaskListUIGroup(TextMeshProUGUI toDoInfo, TextMeshProUGUI warningInfo, TextMeshProUGUI startText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "Read the list of tasks to do and memorize it. Take the time you need. When you are ready, press the \"Start\" button with the Trigger button of your controller."),
                new Translation(language.spanish, "Read the list of tasks to do and memorize it. Take the time you need. When you are ready, press the \"Start\" button with the Trigger button of your controller.")
            })
            ,toDoInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "WARNING: Once you press the button, you won't be able to see the list again."),
                new Translation(language.spanish, "WARNING: Once you press the button, you won't be able to see the list again.")
            })
            ,warningInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "START"),
                new Translation(language.spanish, "EMPEZAR")
            })
            ,startText)
        };
    }
}

public class DoingTasksUIGroup : UIGroup
{
    public DoingTasksUIGroup(TextMeshProUGUI doingTasksInfo, TextMeshProUGUI warningText, TextMeshProUGUI finishText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "Go to the living room and do the tasks! Take all the time you need.\r\n\r\nMove holding the right joystick and pointing to the direction you want to teletransport.\r\n\r\nGrab items and doors using the Grab button of your controller.\r\n\r\n\r\nTo go to the living room, open the door that is at your right.\r\n\r\n\r\nOnce you finish doing the tasks, come back here and press the \"Finish\" button with the Trigger button from your controller."),
                new Translation(language.spanish, "Go to the living room and do the tasks! Take all the time you need.\r\n\r\nMove holding the right joystick and pointing to the direction you want to teletransport.\r\n\r\nGrab items and doors using the Grab button of your controller.\r\n\r\n\r\nTo go to the living room, open the door that is at your right.\r\n\r\n\r\nOnce you finish doing the tasks, come back here and press the \"Finish\" button with the Trigger button from your controller.")
            })
            ,doingTasksInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "WARNING: Once you press the button, you won't be able to see the list again."),
                new Translation(language.spanish, "WARNING: Once you press the button, you won't be able to see the list again.")
            })
            ,warningText),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "FINISH"),
                new Translation(language.spanish, "FINALIZAR")
            })
            ,finishText)
        };
    }
}

public class ExitUIGroup : UIGroup
{
    public ExitUIGroup(TextMeshProUGUI resultsInfo, TextMeshProUGUI finishText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "These are the results of the experience:"),
                new Translation(language.spanish, "These are the results of the experience:")
            })
            ,resultsInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.english, "EXIT"),
                new Translation(language.spanish, "SALIR")
            })
            ,finishText)
        };
    }
}

public sealed class LanguageManager : MonoBehaviour
{
    private static LanguageManager instance;
    public static LanguageManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Create a new GameObject and attach this script to it if the instance is null
                GameObject languageManagerObject = new GameObject("LanguageManager");
                instance = languageManagerObject.AddComponent<LanguageManager>();

                // Make sure the settings object persists across scenes
                DontDestroyOnLoad(languageManagerObject);
            }
            return instance;
        }
    }

    // Prevent external instantiation
    private LanguageManager() { }

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Ensure that there is only one instance of LanguageManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
