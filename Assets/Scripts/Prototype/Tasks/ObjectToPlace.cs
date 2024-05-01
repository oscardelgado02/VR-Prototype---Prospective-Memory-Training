using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectToPlace : MonoBehaviour
{
    // Attributes
    [SerializeField] private string objectTag;
    [SerializeField] private string placeTag;
    public bool placed {  get; private set; }
}
