/**
 * An script that can be used on Unity to manage multiple timers in an efficient way.
 * 
 * How to use:
 *  - Attach the script into a GameObject on your Unity scene.
 *  - To use the timers, just use "Timers.Instance.{input}", where {input} is any method from the Timers class.
 *  - You have an example of some of the instructions that can be used on the "TimeUpdateExamples.cs" file.
 *
 * Scripted by https://github.com/oscardelgado02
 * 
 * MIT License
 *
 * Copyright (c) 2024 Óscar Delgado
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;

public class TimeUpdate : MonoBehaviour
{
    private void Update() { Timers.Instance.UpdateTimers(); }
}

public class Timer
{
    public float time;
    public bool countingTime;
    public bool timerFinished;

    public Timer(bool paused)
    {
        time = 0f; countingTime = !paused; timerFinished = false;
    }
}

public sealed class Timers
{
    private Timers()
    {
        this.timers = new List<Timer>();
    }

    private static Timers instance;
    public static Timers Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Timers();
            }
            return instance;
        }
    }

    private List<Timer> timers;

    //Method to update the timers
    public void UpdateTimers()
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i].countingTime && !timers[i].timerFinished) { timers[i].time += Time.deltaTime; }
        }
    }

    //<----------------------METHODS THAT USES THE TIME CONTROLLER-------------------------->

    //Method to change the time_count
    private void Change_Time_Count(int idx, bool status)
    {
        timers[idx].countingTime = status;
    }

    //<------------------------METHODS TO USE THE TIME CONTROLLER--------------------------->

    /**
     * Method to create a timer.
     *
     * RETURN: An int with the ID of the timer created.
     */
    public int CreateTimer(bool paused = false)
    {
        timers.Add(new Timer(paused));
        return timers.Count - 1;
    }

    //method to get the time value of a timer
    public float GetTime(int idx) { return timers[idx].time; }

    //method to set a time value to a timer
    public void SetTime(int idx, float time) { timers[idx].time = time; }

    //method to know if the timer has arrived to a certain time
    private bool IsTime(int idx, float seconds) { return timers[idx].time > seconds; }

    public void ResumeTimer(int idx) { Change_Time_Count(idx, true); } //method to pause a timer
    public void PauseTimer(int idx) { Change_Time_Count(idx, false); } //method to pause a timer

    //method to reanude all timers
    public void ResumeAllTimers()
    {
        for (int i = 0; i < timers.Count; i++) { ResumeTimer(i); }
    }

    //method to pause all timers
    public void PauseAllTimers()
    {
        for (int i = 0; i < timers.Count; i++) { PauseTimer(i); }
    }

    public void RestartTimer(int idx) { timers[idx].time = 0.0f; } //method to restart a timer

    //method to restart all timers
    public void RestartAllTimers()
    {
        for (int i = 0; i < timers.Count; i++) { RestartTimer(i); }
    }

    //RESET: method to restart and pause a timer
    public void ResetTimer(int idx)
    {
        RestartTimer(idx);
        PauseTimer(idx);
    }

    //method to reset all timers
    public void ResetAllTimers()
    {
        for (int i = 0; i < timers.Count; i++) { ResetTimer(i); }
    }

    //TRUE: The timer "idx" has reached the input time; FALSE: Otherwise
    public bool WaitTime(int idx, float time)
    {
        ResumeTimer(idx);

        return IsTime(idx, time);
    }

    //TRUE: The timer "idx" has reached the input time AND ends the timer; FALSE: Otherwise
    public bool WaitTimeAndFinishTimer(int idx, float time)
    {
        ResumeTimer(idx);

        bool condition = IsTime(idx, time);

        if (condition) { EndTimer(idx); }

        return condition;
    }

    //method to delete all timers
    public void DeleteTimers() { timers.Clear(); }

    //TRUE: The timer "idx" has finished; FALSE: Otherwise
    public bool GetIfFinishedTimer(int idx) { return timers[idx].timerFinished; }

    //method to set the status of a timer as finished
    public void EndTimer(int idx) { timers[idx].timerFinished = true; }
}