using UnityEngine;

public class ObjectToPlace : MonoBehaviour
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "ObjectTagList")]
    [SerializeField] private string objectTag;

    [StringInList(typeof(PropertyDrawersHelper), "ContainerTagList")]
    [SerializeField] private string containerTag;
    public bool placed {  get; private set; }

    private void Start() { placed = false; }

    private void OnTriggerEnter(Collider collider)
    {
        Container container = collider.gameObject.GetComponent<Container>();
        if (container != null)
        {
            if (containerTag.Equals(container.GetTag()))
                placed = true;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Container container = collider.gameObject.GetComponent<Container>();
        if (container != null)
        {
            if (containerTag.Equals(container.GetTag()))
                placed = false;
        }
    }
}
