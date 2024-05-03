using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Attributes
    [Header("Game Controller")]
    [Space(10)]
    [SerializeField] private GameController gameController;

    [Header("UI")]
    [Space(10)]

    [SerializeField] private Transform canvas; // Canvas
    [SerializeField] private Transform content; // Content window to which UI elements will be added

    [Header("Settings UI")]
    [Space(10)]

    [SerializeField] private GameObject settingsUI;
    [SerializeField] private Slider numOfTasksSlider;
    [SerializeField] private TextMeshProUGUI numOfTasksNumberText;
    [SerializeField] private Button startExperienceButton;

    [Header("TaskList UI")]
    [Space(10)]

    [SerializeField] private GameObject taskListUI;
    [SerializeField] private Button startButton;

    [Header("DoingTasks UI")]
    [Space(10)]

    [SerializeField] private GameObject doingTasksUI;
    [SerializeField] private Button finishButton;

    [Header("Exit UI")]
    [Space(10)]

    [SerializeField] private GameObject exitUI;
    [SerializeField] private GameObject csvGeneratedText;
    [SerializeField] private Button exitButton;

    [Header("Sprites")]
    [Space(10)]

    [SerializeField] private Sprite completed;
    [SerializeField] private Sprite notCompleted;

    [Header("Prefabs")]
    [Space(10)]

    [SerializeField] private GameObject taskToDoTextPrefab;
    [SerializeField] private GameObject taskResultTextPrefab;

    private List<GameObject> uiGroups;  // List of the diferent UI groups, ordered by phase
    private bool exitButtonShown = false;   // Control bool

    // Methods
    public void UpdateUI(phaseIds phaseId)
    {
        // We enable the current phase group and disable the rest
        for(int i = 0; i < uiGroups.Count; i++)
            uiGroups[i].SetActive(i==(int)phaseId);

        // If the UI is going to change to the To Do Task List UI or the End UI, then generate the Tasks instances
        if (phaseId == phaseIds.learning || phaseId == phaseIds.end)
        {
            content.gameObject.SetActive(true); // We show the list content
            GenerateTaskListUI(phaseId);
        }
        else
            content.gameObject.SetActive(false);    // We hide the list content
    }

    private void Start()
    {
        // We init the UI
        ActivateAllChildren(canvas.gameObject);

        // We init the uiGroups list and then include all the UI groups in order so they match with their phase
        uiGroups = new List<GameObject>()
        { 
            settingsUI,
            taskListUI,
            doingTasksUI,
            exitUI
        };

        // We init the slider and the buttons' functionality
        GiveFunctionalityToTheSlider();
        GiveFunctionalityToTheButtons();

        // We update the UI to match with the current phase
        UpdateUI(gameStatus.Instance.currPhaseId);

        // We disable the Exit and CSV Generated Text button for later
        exitButton.gameObject.SetActive(false);
        csvGeneratedText.SetActive(false);
    }

    private void Update()
    {
        // At the end, we check if the csv file has been generated to show the "Exit" button, this way we can close the
        // app in a safe way without corrupting the data
        ShowExitButton();
    }

    // Methods to add and manage instances to the UI
    private void ActivateAllChildren(GameObject parent)
    {
        // Activate the parent GameObject
        parent.SetActive(true);

        // Loop through all children of the parent GameObject
        foreach (Transform child in parent.transform)
        {
            // Activate the child GameObject
            child.gameObject.SetActive(true);

            // If the child has children, recursively activate them
            if (child.childCount > 0)
            {
                ActivateAllChildren(child.gameObject);
            }
        }
    }

    private void GiveFunctionalityToTheSlider()
    {
        Settings.Instance.numOfTasksToDo = 1;   //Init value

        // Slider that modify the number of tasks to do
        numOfTasksSlider.onValueChanged.RemoveAllListeners();   // We wipe the current listeners

        numOfTasksSlider.maxValue = TaskList.Instance.GetTotalNumOfTasks();
        numOfTasksSlider.minValue = 1;
        numOfTasksSlider.value = Settings.Instance.numOfTasksToDo;
        numOfTasksSlider.wholeNumbers = true;

        numOfTasksSlider.onValueChanged.AddListener((float value) =>
        {
            Settings.Instance.numOfTasksToDo = (int)value;
            numOfTasksNumberText.text = Settings.Instance.numOfTasksToDo.ToString();
        });
    }

    private void GiveFunctionalityToTheButtons()
    {
        // Button from the Settings UI
        startExperienceButton.onClick.RemoveAllListeners(); // We wipe the current listeners
        startExperienceButton.onClick.AddListener(() => { gameController.StartNextPhase(phaseIds.learning); });

        // Button from the TaskList UI
        startButton.onClick.RemoveAllListeners(); // We wipe the current listeners
        startButton.onClick.AddListener(() => { gameController.StartNextPhase(phaseIds.doingTasks); });

        // Button from the DoingTasks UI
        finishButton.onClick.RemoveAllListeners(); // We wipe the current listeners
        finishButton.onClick.AddListener(() => { gameController.StartNextPhase(phaseIds.end); });

        // Button from the End UI
        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }

    private void WipeAndCreateUISkeleton(int margins = 5, float spacing = 5f)
    {
        // Clear existing content
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // Add a layout group to the content
        VerticalLayoutGroup layoutGroup = content.gameObject.GetComponent<VerticalLayoutGroup>();
        if (layoutGroup == null)
        {
            layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();
        }

        layoutGroup.spacing = spacing; // Adjust the spacing between UI elements

        // Add padding to create margins (adjust as needed)
        layoutGroup.padding.top = margins;    // Top margin
        layoutGroup.padding.bottom = margins; // Bottom margin

        // Add more settings to the layout group
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;

        // Add a content size fitter to the content
        ContentSizeFitter sizeFitter = content.gameObject.GetComponent<ContentSizeFitter>();
        if (sizeFitter == null)
        {
            sizeFitter = content.gameObject.AddComponent<ContentSizeFitter>();
        }

        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
    }

    private void GenerateTaskListUI(phaseIds phaseId)
    {
        WipeAndCreateUISkeleton();

        for(int i = 0; i < gameStatus.Instance.selectedTasksId.Count; i++)
        {
            // We get the current id
            int id = gameStatus.Instance.selectedTasksId[i];

            // We get the current task
            Task task = TaskList.Instance.GetTask(id);

            // In case it is the learning phase
            if(phaseId == phaseIds.learning)
            {
                // We instantiate a taskToDoText prefab and change its text
                GameObject toDoText = Instantiate(taskToDoTextPrefab, content);
                toDoText.GetComponent<TextMeshProUGUI>().text = $"{i+1}. {task.description}";
            }
            // In case it is the end phase
            else if(phaseId == phaseIds.end)
            {
                // We instantiate a taskResultText prefab and change its text
                GameObject resultText = Instantiate(taskResultTextPrefab, content);
                resultText.GetComponent<TextMeshProUGUI>().text = $"{i+1}. {task.description}";

                // We get the status object of the generated resultText
                GameObject status = resultText.transform.GetChild(0).gameObject;

                // We get the texture to assign to the status RawImage component
                Texture texture = task.finished?completed.texture : notCompleted.texture;
                status.GetComponent<RawImage>().texture = texture;
            }
        }
    }

    private void ShowExitButton()
    {
        if (!exitButtonShown)
        {
            if (Settings.Instance.enableDataExtraction)
            {
                if (CSV_Export.Instance.FileHasBeenGenerated())
                {
                    csvGeneratedText.SetActive(true);
                    csvGeneratedText.GetComponent<TextMeshProUGUI>().text = $"{CSV_Export.Instance.fileDirectory} has been generated";
                    exitButton.gameObject.SetActive(true);

                    exitButtonShown = true; // We set the control variable as true, as the exit button has been shown
                }
            }
            // In case it does not need to generate the file, just set it to active
            else
            {
                exitButton.gameObject.SetActive(true);
                exitButtonShown = true; // We set the control variable as true, as the exit button has been shown
            }
        }
    }
}
