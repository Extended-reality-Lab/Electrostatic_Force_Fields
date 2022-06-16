using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerManager : MonoBehaviour
{
    private InputDevice right_controller;
    private InputDevice left_controller;
    Vector3 boxScaler;
    // Start is called before the first frame update
    public GameObject Legend;
    public GameObject vector_field_canvas;
    public GameObject hybrid_canvas;
    public GameObject isosurface_canvas;
    public GameObject state_machine_object;
    public GameObject runner;



    bool progressed;
    // bool second_click = false;

    public enum MODE {
        VECTOR,
        HEAT,
        ISO,
        HYBRID,
        ADV
    }

    public MODE VIS_MODE;
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        List<InputDevice> leftDevices = new List<InputDevice>();
        InputDeviceCharacteristics rightControllerCharacteristics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(rightControllerCharacteristics, devices);
        InputDeviceCharacteristics leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;
        InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, leftDevices);
        boxScaler = new Vector3(0.01f,0.01f,0.01f);
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }
        if (devices.Count > 0)
        {
            Debug.Log("RIGHT DEVICE: " + devices[0].name + devices[0].characteristics);
            right_controller = devices[0];
        }
        if (leftDevices.Count > 0)
        {
            Debug.Log("LEFT DEVICE: " + devices[0].name + devices[0].characteristics);
            left_controller = leftDevices[0];
        }
    }
}
