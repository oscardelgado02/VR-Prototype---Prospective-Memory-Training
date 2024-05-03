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
        // In case all the objects are inside
        if (CheckIfObjectsAreInside())
        {
            if(GetIfTaskCanBeDone())
                FinishTask();
        }
        // In case all the objects are not inside
        else
            interactable = true;
    }

    protected bool CheckIfObjectsAreInside()
    {
        bool result = true;

        foreach (ObjectToPlace objectToPlace in objectsToPlace)
            result &= objectToPlace.placed;

        return result;
    }

    public string GetTag() { return containerTag; }
}
