using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum language
{
    English,
    Castellano
};

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

public abstract class UIGroup
{
    public List<UISentence> uiSentences = new List<UISentence>();
    public UIGroup() { }
    public void TranslateUI(language inputLang) { foreach (UISentence sentence in uiSentences) sentence.TranslateUIText(inputLang); }
}

public class SettingsUIGroup : UIGroup
{
    public SettingsUIGroup(TextMeshProUGUI settingsInfo, TextMeshProUGUI numOfTasksToDo, TextMeshProUGUI startText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "Welcome to the Prospective Memory Training VR Prototype!\r\n\r\nYou can edit here the settings of the experience. Once you are finished, press the \"Start\" button with the Trigger button from your controller."),
                new Translation(language.Castellano, "¡Bienvenido al Prototipo de Entrenamiento de Memoria Prospectiva en VR!\r\n\r\nAquí puedes editar los ajustes de la experiencia. Cuando acabes, presiona el botón \"Empezar\" con el gatillo del controlador.")
            })
            ,settingsInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "Number of tasks to do"),
                new Translation(language.Castellano, "Número de tareas")
            })
            ,numOfTasksToDo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "START"),
                new Translation(language.Castellano, "EMPEZAR")
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
                new Translation(language.English, "Read the list of tasks to do and memorize it. Take the time you need. When you are ready, press the \"Start\" button with the Trigger button of your controller."),
                new Translation(language.Castellano, "Lee todas las tareas y memorizalas. Tómate el tiempo que necesites. Cuando estés listo/a, presiona el botón \"Empezar\" con el gatillo del controlador.")
            })
            ,toDoInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "WARNING: Once you press the button, you won't be able to see the list again."),
                new Translation(language.Castellano, "ATENCIÓN: Una vez presiones el botón, no podrás volver a ver la lista de nuevo.")
            })
            ,warningInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "START"),
                new Translation(language.Castellano, "EMPEZAR")
            })
            ,startText)
        };
    }
}

public class DoingTasksUIGroup : UIGroup
{
    public DoingTasksUIGroup(TextMeshProUGUI doingTasksInfo, TextMeshProUGUI finishText)
    {
        uiSentences = new List<UISentence>()
        {
            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "Go to the living room and do the tasks! Take all the time you need.\r\n\r\nMove holding the right thumbstick and pointing to the direction you want to teletransport.\r\n\r\nGrab items and doors using the Grab button of your controller.\r\n\r\n\r\nTo go to the living room, open the door that is at your right.\r\n\r\n\r\nOnce you finish doing the tasks, come back here and press the \"Finish\" button with the Trigger button from your controller."),
                new Translation(language.Castellano, "¡Ve a la sala de estar y haz las tareas! Tómate el tiempo que necesites.\r\n\r\nMuevete presionando hacia arriba el thumbstick derecho y apuntando a la dirección a la que quieres teletransportarte.\r\n\r\nAgarra objetos y puertas usando el botón de agarre del controlador.\r\n\r\n\r\nPara ir a la sala de estar, abre la puerta que está justo a tu derecha.\r\n\r\n\r\nUna vez acabes las tareas, vuelve aquí y presiona el botón \"Finalizar\" con el gatillo del controlador.")
            })
            ,doingTasksInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "FINISH"),
                new Translation(language.Castellano, "FINALIZAR")
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
                new Translation(language.English, "These are the results of the experience:"),
                new Translation(language.Castellano, "Estos son los resultados de la experiencia:")
            })
            ,resultsInfo),

            new UISentence(new Sentence(new List<Translation>()
            {
                new Translation(language.English, "EXIT"),
                new Translation(language.Castellano, "SALIR")
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

    // Language Dropdown
    [SerializeField] private TMP_Dropdown languageDropdown;

    // TextMeshProUGUI Atributes
    [SerializeField] private TextMeshProUGUI settingsSettingsInfo;
    [SerializeField] private TextMeshProUGUI settingsNumOfTasksToDo;
    [SerializeField] private TextMeshProUGUI settingsStartText;

    [SerializeField] private TextMeshProUGUI tasksToDoInfo;
    [SerializeField] private TextMeshProUGUI tasksWarningInfo;
    [SerializeField] private TextMeshProUGUI tasksStartText;

    [SerializeField] private TextMeshProUGUI doingTasksDoingTasksInfo;
    [SerializeField] private TextMeshProUGUI doingTasksFinishText;

    [SerializeField] private TextMeshProUGUI exitResultsInfo;
    [SerializeField] private TextMeshProUGUI exitFinishText;

    private List<UIGroup> uIGroups = new List<UIGroup>();

    private void Start()
    {
        // Init the UI Groups
        uIGroups = new List<UIGroup>()
        {
            new SettingsUIGroup(settingsSettingsInfo, settingsNumOfTasksToDo, settingsStartText),
            new TaskListUIGroup(tasksToDoInfo, tasksWarningInfo, tasksStartText),
            new DoingTasksUIGroup(doingTasksDoingTasksInfo, doingTasksFinishText),
            new ExitUIGroup(exitResultsInfo, exitFinishText)
        };

        // Fill dropdown with values
        PopulateLanguageDropdown();

        // Give functionality to the dropdown
        languageDropdown.onValueChanged.RemoveAllListeners();
        languageDropdown.onValueChanged.AddListener(delegate {
            Settings.Instance.lang = (language)languageDropdown.value;
            TranslateUI(Settings.Instance.lang);
        });

        // Translate the UI at the beggining
        languageDropdown.value = (int)Settings.Instance.lang;
    }

    private void TranslateUI(language inputLang) { foreach (UIGroup group in uIGroups) group.TranslateUI(inputLang); }

    private void PopulateLanguageDropdown()
    {
        // Clear existing options
        languageDropdown.ClearOptions();

        // Get all the enum names as a string array
        string[] enumNames = System.Enum.GetNames(typeof(language));

        // Convert the string array to a List<string> (required by TMP_Dropdown)
        var options = new List<string>(enumNames);

        // Add options to the dropdown
        languageDropdown.AddOptions(options);
    }
}
