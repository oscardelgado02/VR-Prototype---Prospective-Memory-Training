using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public sealed class CSV_Export
{
    private CSV_Export() { Init(); }

    private static CSV_Export instance;
    public static CSV_Export Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CSV_Export();
            }
            return instance;
        }
    }

    // Data Row Events definition
    [System.Serializable]
    public class DataRowEvents
    {
        public string event_;
        public string phase;
        public string taskId;
        public string taskDescription;
        public string taskState;
        public string taskInList;
        public string time;

        public DataRowEvents(int eventId, int phaseId, int taskId, float time)
        {
            // Event data
            if (eventId < 0) { event_ = "NaN"; }
            else { event_ = labels.Instance._eventLabels[eventId]; }

            // Phase data
            if (phaseId < 0) { phase = "NaN"; }
            else { phase = labels.Instance._eventLabels[phaseId]; }

            // Task data
            if (taskId < 0)
            {
                this.taskId = "NaN";
                taskDescription = "NaN";
                taskState = "NaN";
                taskInList = "NaN";
            }
            else
            {
                this.taskId = taskId.ToString();
                taskDescription = TaskList.Instance.GetTask(taskId).description;
                taskState = TaskList.Instance.GetTask(taskId).finished ? "Completed" : "Not Completed";
                taskInList = TaskList.Instance.GetTask(taskId).toDo ? "Yes" : "No";
            }

            // Time data
            this.time = time.ToString().Replace(',', '.');
        }
    }

    // Attributes
    private string fileDirectory;
    private int timer = -1;  // Timer ID

    // Methods
    private void Init()
    {
        // We init the folders and the filename where data will be stored
        InitFilenames();

        // We get the timer ID
        if (timer < 0)
            timer = Timers.Instance.CreateTimer();

        // We start running the timer
        Timers.Instance.ResumeTimer(timer);
    }

    private void InitFilenames()
    {
        // Get the parent directory of the application path
        string parentDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));

        // We create the directory folder
        string folder = Path.Combine(parentDirectory, "ExportedData");

        // Check if the folder exists, create it if not
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);

        // Then, we create the filename, that will contain the current date and the number of tasks to do
        string date = DateTime.Now.ToString("yyyy_MM_dd_HH;mm");
        string numOfTasksToDo = Settings.Instance.numOfTasksToDo.ToString();

        string filename = $"PMT_Date_{date}_NumOfTasksToDo_{numOfTasksToDo}";

        fileDirectory = Path.Combine(folder, $"{filename}.csv");
    }
}
