using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Attributes
    [Header("UI")]
    [Space(10)]

    [SerializeField] private Transform canvas; // Canvas
    [SerializeField] private Transform content; // Content window to which UI elements will be added
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject csvGeneratedText;

    [Header("Sprites")]
    [Space(10)]

    [SerializeField] private Sprite completed;
    [SerializeField] private Sprite notCompleted;

    [Header("Prefabs")]
    [Space(10)]

    [SerializeField] private GameObject emptyUIPrefab;
    [SerializeField] private GameObject taskToDoTextPrefab;
    [SerializeField] private GameObject taskResultTextPrefab;

    // Methods
    public void UpdateUI(phaseIds phaseId)
    {
        switch (phaseId)
        {
            case phaseIds.learning:
                // Code
                break;
            case phaseIds.doingTasks:
                // Code
                break;
            case phaseIds.end:
                // Code
                break;
            default:
                break;
        }
    }

    private void Start()
    {
        // We init the UI
        exitButton.SetActive(false);
        csvGeneratedText.SetActive(false);
    }

    private void Update()
    {
        // At the end, we check if the csv file has been generated to show the "Exit" button, this way we can close the
        // app in a safe way without corrupting the data
        if (Settings.Instance.enableDataExtraction)
        {
            if (CSV_Export.Instance.FileHasBeenGenerated())
            {
                csvGeneratedText.SetActive(true);
                csvGeneratedText.GetComponent<TextMeshPro>().text = $"CSV file in directory\n{CSV_Export.Instance.fileDirectory}\nhas been generated";
                exitButton.SetActive(true);
            }
        }
        else
            exitButton.SetActive(true);
    }

    // Methods to add instances to the UI
    private void WipeAndCreateUISkeleton(int margins = 50, float spacing = 30f)
    {
        // Add CanvasScaler to the canvas
        CanvasScaler canvasScaler = canvas.gameObject.GetComponent<CanvasScaler>();
        if (canvasScaler == null)
        {
            canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
        }

        // You may need to adjust CanvasScaler settings based on your requirements
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(1920, 1080);

        // Clear existing content
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        //Adjust the content to the top
        content.position = new Vector2(0, 0);

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

    private RectTransform CreateHorizontalLayout(float spacing = 10f, TextAnchor alignment = TextAnchor.MiddleCenter)
    {
        //Horizontal Layout to add Tittle and Start Button
        RectTransform horizontalLayout = Instantiate(emptyUIPrefab, content).GetComponent<RectTransform>();
        horizontalLayout.sizeDelta = new Vector2(1700, 100);
        HorizontalLayoutGroup horizontalLayoutComponent = horizontalLayout.gameObject.AddComponent<HorizontalLayoutGroup>();

        horizontalLayoutComponent.spacing = spacing; // Adjust the spacing between UI elements

        // Add more settings to the horizontal layout group
        horizontalLayoutComponent.childAlignment = alignment;
        horizontalLayoutComponent.childControlWidth = false;
        horizontalLayoutComponent.childControlHeight = false;
        horizontalLayoutComponent.childForceExpandWidth = false;
        horizontalLayoutComponent.childForceExpandHeight = false;

        return horizontalLayout;
    }
}
