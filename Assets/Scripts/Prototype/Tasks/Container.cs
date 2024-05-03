using System.Collections.Generic;
using UnityEngine;

public class Container : TaskObject
{
    // Attributes
    [SerializeField] protected List<ObjectToPlace> objectsToPlace;

    [StringInList(typeof(PropertyDrawersHelper), "ContainerTagList")]
    [SerializeField] protected string containerTag;

    // Methods
    protected void Update()
    {
        if (GetIfTaskCanBeDone())
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
