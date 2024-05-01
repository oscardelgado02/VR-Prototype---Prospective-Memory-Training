using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Attributes
    [SerializeField] private GameObject _xrDeviceSimulator;
    private int currPhaseId = (int)phaseIds.learning;

    // Methods
    private void Start()
    {
        // Enable/Disable XR Device Simulator
        _xrDeviceSimulator.SetActive(Settings.Instance.xrDeviceSimulator);
    }

    private void Update()
    {
        ManageGame();
    }

    private void ManageGame()
    {

    }
}
