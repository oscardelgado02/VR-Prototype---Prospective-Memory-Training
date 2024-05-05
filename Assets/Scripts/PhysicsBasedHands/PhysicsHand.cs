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

    [SerializeField] private string defaultLayerName = "Hands";
    [SerializeField] private string grabbingLayerName = "HandsGrabbingObject";
    private bool grabbingObject = false;    // bool to check if an object is being grabbed

    private void Start()
    {
        interactor.selectEntered.AddListener((SelectEnterEventArgs args) => grabbingObject = true);
        interactor.selectExited.AddListener((SelectExitEventArgs args) => grabbingObject = false);
    }
    private void FixedUpdate()
    {
        string layerName = grabbingObject ? grabbingLayerName : defaultLayerName;    // We get the layer name

        gameObject.layer = LayerMask.NameToLayer(layerName);    // We change the layer

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
