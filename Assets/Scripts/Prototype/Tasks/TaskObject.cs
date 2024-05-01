using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskObject : MonoBehaviour
{
    // Attributes
    [SerializeField] public int taskId {  get; protected set; }
    protected bool interactable = true;

    // Methods
    protected void FinishTask()
    {
        if (interactable)
        {
            TaskList.Instance.GetTask(taskId).finished = true;  // Finish the task
            interactable = false;   // This object is no longer interactable for tasks purposes
        }
    }
}
