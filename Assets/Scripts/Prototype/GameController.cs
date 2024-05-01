using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _xrDeviceSimulator;
    private void Start()
    {
        // Enable/Disable XR Device Simulator
        _xrDeviceSimulator.SetActive(Settings.Instance.xrDeviceSimulator);
    }

    private void Update()
    {
        
    }
}
