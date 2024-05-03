using UnityEngine;

public class TaskObject : MonoBehaviour
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "TaskList")]
    [SerializeField] protected int taskId;

    protected bool interactable = true;

    // Methods
    public int GetID() { return taskId; }
    protected void FinishTask()
    {
        if (interactable)
        {
            TaskList.Instance.GetTask(taskId).finished = true;  // Finish the task
            interactable = false;   // This object is no longer interactable for tasks purposes
        }
    }
}
