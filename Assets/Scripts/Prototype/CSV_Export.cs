using System;
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
    public class DataRowEvent
    {
        public string event_;
        public string phase;
        public string taskId;
        public string taskDescription;
        public string taskState;
        public string taskInList;
        public string time;

        public DataRowEvent(int eventId, int phaseId, int taskId, float time)
        {
            // Event data
            if (eventId < 0) { event_ = "NaN"; }
            else { event_ = labels.Instance._eventLabels[eventId]; }

            // Phase data
            if (phaseId < 0) { phase = "NaN"; }
            else { phase = labels.Instance._phaseLabels[phaseId]; }

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

        // Method to get the Data Row string
        public string GetString() { return $"{event_},{phase},{taskId},{taskDescription},{taskState},{taskInList},{time}"; }
    }

    // Attributes
    public string fileDirectory { get; private set; }
    private int timer = -1;  // Timer ID
    private List<DataRowEvent> events_data_rows;

    // Methods
    private void Init()
    {
        // We init the DataRowEvents list
        events_data_rows = new List<DataRowEvent>();

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

    // Method to add a line
    private void AddEventLine(int eventId, int phaseId, int taskId)
    {
        events_data_rows.Add(new DataRowEvent(eventId, phaseId, taskId, Timers.Instance.GetTime(timer)));
    }

    // Method to add a line that shows the start of an event
    public void WriteStartPhaseLine(phaseIds phaseId) { AddEventLine((int)eventIds.start, (int)phaseId, -1); }

    // Method to add a line that shows the end of an event
    public void WriteEndPhaseLine(phaseIds phaseId) { AddEventLine((int)eventIds.end, (int)phaseId, -1); }

    // Method to add a line that shows if a task has been finished and all its information
    public void WriteTaskFinishedLine(int taskId) { AddEventLine((int)eventIds.taskFinished, (int)phaseIds.doingTasks, taskId); }

    // Method to generate the csv file
    public void ExportCSV()
    {
        if (Settings.Instance.enableDataExtraction)
        {
            // We open the Text Writer
            TextWriter tw = new StreamWriter(fileDirectory, false);

            // We create the headers string
            string headers = string.Empty;

            foreach(string header in labels.Instance._csvHeaders)
                headers += $"{header},";

            headers = headers.Substring(0, headers.Length - 1);

            // We write the headers in the first line
            tw.WriteLine(headers);
            tw.Close();

            // We open the Text Writer again in append mode
            tw = new StreamWriter(fileDirectory, true);

            foreach (DataRowEvent _event in events_data_rows)
                tw.WriteLine(_event.GetString());

            tw.Close();
        }
    }

    // Method to check if the csv file has been generated
    public bool FileHasBeenGenerated() { return File.Exists(fileDirectory); }
}
