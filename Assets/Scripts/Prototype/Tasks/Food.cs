using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : TaskObject
{
    // Attributes
    [SerializeField] private string foodTag;

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
