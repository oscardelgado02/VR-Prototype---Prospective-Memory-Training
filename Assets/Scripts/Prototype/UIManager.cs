using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Attributes
    [SerializeField] private GameObject UI;

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
}
