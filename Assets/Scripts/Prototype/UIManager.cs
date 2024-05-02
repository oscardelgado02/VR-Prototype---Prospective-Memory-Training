using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Attributes
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject csvGeneratedText;

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
}
