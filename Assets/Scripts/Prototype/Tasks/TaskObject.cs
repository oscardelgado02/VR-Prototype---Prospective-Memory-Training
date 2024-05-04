using UnityEngine;

public class TaskObject : MonoBehaviour
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "TaskList")]
    [SerializeField] protected int taskId;

    protected bool interactable = true;

    // Methods
    public int GetID() { return taskId; }
    protected bool GetIfTaskCanBeDone() { return interactable && gameStatus.Instance.currPhaseId==phaseIds.doingTasks; }
    protected void FinishTask()
    {
        if (GetIfTaskCanBeDone())
        {
            TaskList.Instance.GetTask(taskId).finished = true;  // Finish the task
            interactable = false;   // This object is no longer interactable for tasks purposes

            // We send the data to the CSV extractor
            CSV_Export.Instance.WriteTaskFinishedLine(taskId);
        }
    }
}
