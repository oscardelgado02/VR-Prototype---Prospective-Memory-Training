using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhysicsHand : MonoBehaviour
{
    [SerializeField] private Transform trackedTransform;
    [SerializeField] private Rigidbody body;
    [SerializeField] private XRDirectInteractor interactor; // added interactor

    [SerializeField] private float positionStrength = 20;
    [SerializeField] private float positionThreshold = 0.005f;
    [SerializeField] private float maxDistance = 1f;
    [SerializeField] private float rotationStrength = 30;
    [SerializeField] private float rotationThreshold = 10f;

    // List of layers from objects that can be grabbed
    [SerializeField] private List<string> defaultObjectLayers = new List<string>
    {
        "Interactable",
        "TaskInteractable"
    };

    // List of layers that will replace the objects that can be grabbed layers
    [SerializeField] private List<string> grabbedObjectLayers = new List<string>
    {
        "GrabbedInteractable",
        "GrabbedTaskInteractable"
    };

    private void Start()
    {
        // In case an object is grabbed
        interactor.selectEntered.AddListener((SelectEnterEventArgs args) => 
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the grabbed object

            // If the grabbedObj is inside one of the defaultObjectLayers,
            // then we change it to its corresponding grabbedObjectLayers layer
            for(int i = 0; i < defaultObjectLayers.Count; i++)
            {
                if (LayerMask.LayerToName(grabbedObj.layer).Equals(defaultObjectLayers[i]))
                    grabbedObj.layer = LayerMask.NameToLayer(grabbedObjectLayers[i]);   // We change the layer
            }
        });

        // In case an object is ungrabbed
        interactor.selectExited.AddListener((SelectExitEventArgs args) =>
        {
            GameObject grabbedObj = args.interactableObject.transform.gameObject;   // We get the GameObject of the ungrabbed object

            // If the grabbedObj is inside one of the grabbedObjectLayers,
            // then we change it to its corresponding defaultObjectLayers layer
            for (int i = 0; i < grabbedObjectLayers.Count; i++)
            {
                if (LayerMask.LayerToName(grabbedObj.layer).Equals(grabbedObjectLayers[i]))
                    grabbedObj.layer = LayerMask.NameToLayer(defaultObjectLayers[i]);   // We change the layer
            }
        });
    }
    private void FixedUpdate()
    {
        // Rest of the script
        var distance = Vector3.Distance(trackedTransform.position, body.position);
        if (distance > maxDistance || distance < positionThreshold)
        {
            body.MovePosition(trackedTransform.position);
        }
        else
        {
            var vel = (trackedTransform.position - body.position).normalized * positionStrength * distance;
            body.velocity = vel;
        }

        float angleDistance = Quaternion.Angle(body.rotation, trackedTransform.rotation);
        if (angleDistance < rotationThreshold)
        {
            body.MoveRotation(trackedTransform.rotation);
        }
        else
        {
            float kp = (6f * rotationStrength) * (6f * rotationStrength) * 0.25f;
            float kd = 4.5f * rotationStrength;
            Vector3 x;
            float xMag;
            Quaternion q = trackedTransform.rotation * Quaternion.Inverse(transform.rotation);
            q.ToAngleAxis(out xMag, out x);
            x.Normalize();
            x *= Mathf.Deg2Rad;
            Vector3 pidv = kp * x * xMag - kd * body.angularVelocity;
            Quaternion rotInertia2World = body.inertiaTensorRotation * transform.rotation;
            pidv = Quaternion.Inverse(rotInertia2World) * pidv;
            pidv.Scale(body.inertiaTensor);
            pidv = rotInertia2World * pidv;
            body.AddTorque(pidv);
        }
    }
}
