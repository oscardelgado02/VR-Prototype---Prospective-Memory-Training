using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Attributes
    [SerializeField] private GameObject UI;
    private int currPhaseId = (int)phaseIds.learning;

    // Methods
    public void UpdateUI(int phaseId)
    {
        switch (phaseId)
        {
            case (int)phaseIds.learning:
                // Code
                break;
            case (int)phaseIds.doingTasks:
                // Code
                break;
            case (int)phaseIds.end:
                // Code
                break;
            default:
                break;
        }
    }
}
