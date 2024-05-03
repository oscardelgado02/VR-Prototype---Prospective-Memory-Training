using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjectToPlace : MonoBehaviour
{
    // Attributes
    [StringInList(typeof(PropertyDrawersHelper), "ObjectTagList")]
    [SerializeField] private string objectTag;

    [StringInList(typeof(PropertyDrawersHelper), "ContainerTagList")]
    [SerializeField] private string containerTag;
    public bool placed {  get; private set; }
    private bool isGrabbing = false;

    private XRGrabInteractable grabInteractable;
    private Transform containerTransform;

    private void Start()
    {
        // We set the object as not placed
        placed = false;

        // We add the listeners to XRGrabInteractable check if an object is being grabbed or not
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener((SelectEnterEventArgs call) => { isGrabbing = true; });
        grabInteractable.selectExited.AddListener((SelectExitEventArgs call) => { isGrabbing = false; });

        // We init the containerTransform with the object transform (it will change in the future when it collides with the corresponding collider)
        containerTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        // In case the container is a Wall
        if (containerTag.Equals(labels.Instance._containerTags[(int)containerIds.wall]))
        {
            // In case the object is placed and it is not being grabbed
            if (placed && !isGrabbing)
            {
                // Fix to the wall
                GetComponent<Rigidbody>().useGravity = false;
                transform.position = containerTransform.position;
                transform.rotation = Quaternion.Euler(containerTransform.rotation.eulerAngles - new Vector3(0,180,0));
                transform.position += transform.rotation * Vector3.forward * 0.02f;    // Little offset forward
                transform.position -= Vector3.up * GetComponent<MeshRenderer>().bounds.size.y * 0.48f;
            }
            else if (!placed && !isGrabbing)
                GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        Container container = collider.gameObject.GetComponent<Container>();
        if (container != null)
        {
            // In case the object is in contact with the container and is not being grabbed
            if (containerTag.Equals(container.GetTag()) && !isGrabbing)
            {
                placed = true;  // We set the object as placed
                containerTransform = collider.transform;    // We get the collider transform
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        Container container = collider.gameObject.GetComponent<Container>();
        if (container != null)
        {
            // In case the object stops being in contact with the container
            if (containerTag.Equals(container.GetTag()))
                placed = false; // We set the object as not placed
        }
    }
}
