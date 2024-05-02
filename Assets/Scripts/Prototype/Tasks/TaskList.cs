using System.Collections.Generic;

public class Task
{
    // Constructor
    public Task(int id, string description)
    {
        this.id = id;
        this.description = description;
        toDo = false;   // By default we set the task to do to false
        finished = false;   // By default we set the task as it is not finished
    }

    // Attributes
    public int id {  get; private set; }
    public string description { get; private set; }
    public bool toDo { get; set; }
    public bool finished { get; set; }
}

public sealed class TaskList
{
    private TaskList() { InitTaskList(); }

    private static TaskList instance;
    public static TaskList Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskList();
            }
            return instance;
        }
    }

    // Attributtes
    private List<Task> tasks;

    // Methods

    // Method in which tasks are defined
    private void InitTaskList()
    {
        tasks = new List<Task>();

        for(int i = 0; i < labels.Instance._taskLabels.Count; i++)
            tasks.Add(new Task(i, labels.Instance._taskLabels[i]));
    }

    // Method to get a task
    public Task GetTask(int id) { return tasks[id]; }

    // Method to get the total number of defined tasks
    public int GetTotalNumOfTasks() { return tasks.Count; }
}
