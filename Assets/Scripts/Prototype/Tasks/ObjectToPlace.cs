using UnityEngine;

public class ObjectToPlace : MonoBehaviour
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "ObjectTagList")]
    [SerializeField] private string objectTag;

    [StringInList(typeof(PropertyDrawersHelper), "ContainerTagList")]
    [SerializeField] private string containerTag;
    public bool placed {  get; private set; }
}
