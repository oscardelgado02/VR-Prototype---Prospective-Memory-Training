using UnityEngine;

public class Food : TaskObject
{
    // Methods
    private void EatFood()
    {
        if (interactable)
        {
            // Code

            FinishTask();
        }
    }
}
