using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> AllDevices = new List<UnityEngine.XR.InputDevice>();
    public InputDeviceCharacteristics controllercharacteristics;
    public List<GameObject> ControllerPrefabs = new List<GameObject>(); 
    private InputDevice hand_controller;
    
    bool assign_controllers = true; // will need to assign controllers on startup
    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = 72;
    }
    void Start()
    {
        if (AllDevices.Count < 3) //Some device was disconnected, need to look for and reassign devices
        {
            Debug.Log("Only " + AllDevices.Count + " Devices. Something Disconnected, Looking for devices...");
            assign_controllers = true;
        }

        if (AllDevices.Count >= 3 && assign_controllers == true)
        {
            List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();


            UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(controllercharacteristics, devices);

            hand_controller = devices[0];


            Debug.Log("Just Discovered:" + hand_controller.name);


            GameObject controller_model = ControllerPrefabs.Find(controller => controller.name == hand_controller.name);
            if (controller_model)
            {
                Instantiate(controller_model, transform);
            }
            /*
            Debug.Log("Total Devices:" + AllDevices.Count);
            Debug.Log("LH Devices: " + lefties.Count);
            Debug.Log("RH Devices: " + righties.Count);
            Debug.Log("Left Controller:" + LHcontroller.characteristics);
            Debug.Log("Right Controller:" + RHcontroller.characteristics); 
            */

            assign_controllers = false;
        }

        else if (AllDevices.Count >= 3 && assign_controllers == false)
        {
            Debug.Log(hand_controller.name + "was working");

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

    }

    
}
