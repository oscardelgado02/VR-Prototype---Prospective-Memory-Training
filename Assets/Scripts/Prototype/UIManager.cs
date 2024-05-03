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

    private void WipeAndCreateUISkeleton(int margins = 5, float spacing = 1f)
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

            // In case it is the learning phase or the end phase
            if(phaseId == phaseIds.learning || phaseId == phaseIds.end)
                CreateTaskLine(phaseId, task, i);
        }
    }

    // Function to create a task line
    private void CreateTaskLine(phaseIds id, Task task, int iteration)
    {
        // We double check to make sure it is not called in any other side
        if (id == phaseIds.learning || id == phaseIds.end)
        {
            // We instantiate a taskToDoText or a taskResultText prefab, depending on the phase, and change its text
            GameObject text = (id == phaseIds.learning)
                ?Instantiate(taskToDoTextPrefab, content):
                Instantiate(taskResultTextPrefab, content);

            TextMeshProUGUI textMesh = (id == phaseIds.learning)
                ?text.GetComponent<TextMeshProUGUI>():
                text.GetComponentInChildren<TextMeshProUGUI>();
            textMesh.text = $"{iteration + 1}. {task.description}";

            if(textMesh != null)
            {
                // We change the text size to adapt it to the number of lines it has
                RectTransform rectTrans = text.GetComponent<RectTransform>();
                rectTrans.rect.Set(rectTrans.rect.x,
                    rectTrans.rect.y,
                    rectTrans.rect.width,
                    rectTrans.rect.height * CountVisibleLines(textMesh));
            }            

            if (id == phaseIds.end)
            {
                // We get the status object of the generated resultText
                RawImage status = text.transform.GetComponentInChildren<RawImage>();

                if (status != null)
                {
                    // We get the texture to assign to the status RawImage component
                    Texture texture = task.finished ? completed.texture : notCompleted.texture;
                    status.texture = texture;
                }
            }
        }
    }

    // Function to count the number of visible lines in a TextMeshProUGUI component
    private int CountVisibleLines(TextMeshProUGUI textMesh)
    {
        // Get the text container of the TextMeshProUGUI component
        TMP_TextInfo textInfo = textMesh.textInfo;

        int lineCount = 0;

        // Iterate through each character in the text
        for (int i = 0; i < textInfo.characterCount; i++)
        {
            // Check if the character is the first character of a new line
            if (textInfo.characterInfo[i].isVisible && textInfo.characterInfo[i].lineNumber > lineCount)
            {
                lineCount++;
            }
        }

        // Return the number of visible lines
        return lineCount + 1; // Add 1 because line numbers are zero-based
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
