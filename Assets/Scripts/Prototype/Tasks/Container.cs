using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : TaskObject
{
    // Attributes
    [SerializeField] protected List<ObjectToPlace> objectsToPlace;
    [SerializeField] protected string containerTag;

    // Methods
    protected void Update()
    {
        if (interactable)
        {
            if(CheckIfObjectsAreInside())
                FinishTask();
        }
    }

    protected bool CheckIfObjectsAreInside()
    {
        bool result = false;

        // Code

        return result;
    }
}
