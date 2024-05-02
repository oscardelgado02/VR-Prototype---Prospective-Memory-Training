using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class BoardInteraction : MonoBehaviour
{
    // Attributes
    [SerializeField] private LineRenderer lineRendererLeft;
    [SerializeField] private LineRenderer lineRendererRight;
    [SerializeField] private Transform controllerTransformLeft;
    [SerializeField] private Transform controllerTransformRight;
    [SerializeField] private LayerMask boardLayer; // Layer mask for the board
    [SerializeField] private float interactionDistance = 2.5f;

    [SerializeField] private Button exitButton;

    //For button down
    private bool previousLeftTriggerState = false;
    private bool previousRightTriggerState = false;

    // Methods
    private void Start()
    {
        //We init the button functions
        exitButton.onClick.AddListener(ExitGame);
    }

    public void ExitGame() { Application.Quit(); }

    private void Update()
    {
        HandleRaycast(controllerTransformLeft, lineRendererLeft);
        HandleRaycast(controllerTransformRight, lineRendererRight);

        bool leftTrigger;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTrigger))
        {
            if (leftTrigger && !previousLeftTriggerState)
            {
                TryInteractWithButton(controllerTransformLeft);
            }
        }

        bool rightTrigger;
        if (UnityEngine.XR.InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightTrigger))
        {
            if (rightTrigger && !previousRightTriggerState)
            {
                TryInteractWithButton(controllerTransformRight);
            }
        }

        previousLeftTriggerState = leftTrigger;
        previousRightTriggerState = rightTrigger;
    }

    private void HandleRaycast(Transform controllerTransform, LineRenderer lineRenderer)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance, boardLayer))
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, controllerTransform.position);
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private void TryInteractWithButton(Transform controllerTransform)
    {
        RaycastHit hit;
        if (Physics.Raycast(controllerTransform.position, controllerTransform.forward, out hit, interactionDistance))
        {
            // Check if the hit component is a button
            Button button = hit.collider.GetComponent<Button>();
            if (button != null)
            {
                // Call the method to interact with the button
                button.onClick.Invoke();
            }
        }
    }
}
