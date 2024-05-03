using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Attributes
    [SerializeField] private UIManager _uiManager;

    // Methods
    private void Start()
    {
        // Write a line in the CSV Export system to indicate that the modifying settings phase started
        CSV_Export.Instance.WriteStartPhaseLine(gameStatus.Instance.currPhaseId);
    }

    public void StartNextPhase(phaseIds nextId)
    {
        // Write a line in the CSV Export system to indicate that the previous phase ended
        CSV_Export.Instance.WriteEndPhaseLine(gameStatus.Instance.currPhaseId);

        // Change the current phase id
        gameStatus.Instance.currPhaseId = nextId;

        bool generateCSVFile = false;   // Check if the csv file has to be generated

        // Do some code depending one the current phase
        switch (gameStatus.Instance.currPhaseId)
        {
            case phaseIds.learning:
                SetTasksToDo();
                break;

            //case phaseIds.doingTasks:
            //    break;

            case phaseIds.end:
                generateCSVFile = true;
                break;

            default: break;
        }

        // We update the UI
        _uiManager.UpdateUI(gameStatus.Instance.currPhaseId);

        // Write a line in the CSV Export system to indicate that the next phase started
        CSV_Export.Instance.WriteStartPhaseLine(gameStatus.Instance.currPhaseId);

        // Export the csv file in case in case the trial finished
        if(generateCSVFile)
            CSV_Export.Instance.ExportCSV();
    }

    private void SetTasksToDo()
    {
        // We get the ids of the tasks to do by the user
        List<int> tasksToDoIds = 
            RandomNumberGenerator.GenerateRandomNumbers(
                TaskList.Instance.GetTotalNumOfTasks(),
                Settings.Instance.numOfTasksToDo);

        // We set the "toDo" attribute as true of the selected tasks
        foreach (int id in tasksToDoIds)
            TaskList.Instance.GetTask(id).toDo = true;

        // We save the selected ids in the gameStatus singleton class
        gameStatus.Instance.selectedTasksId = tasksToDoIds;
    }
}

public sealed class RandomNumberGenerator
{
    public static List<int> GenerateRandomNumbers(int maxNumber, int outputNumbers)
    {
        List<int> numbers = Enumerable.Range(0, maxNumber).ToList(); // Creates a list of integers from 0 to maxNumber

        // Shuffle the list
        numbers = Shuffle(numbers);

        // Select the first three elements
        List<int> selectedNumbers = numbers.Take(outputNumbers).ToList();

        // Return the selected numbers
        return selectedNumbers;
    }

    // Fisher-Yates shuffle algorithm
    private static List<T> Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}