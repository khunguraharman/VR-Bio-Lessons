using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class GetDevices : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> AllDevices = new List<UnityEngine.XR.InputDevice>();
    private InputDevice LHcontroller;
    private InputDevice RHcontroller;
    bool reassign_controllers = true;
    // Start is called before the first frame update

    private void Awake()
    {
        Application.targetFrameRate = 72;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        CheckDevices();

    }

    void CheckDevices()
    {

        
        UnityEngine.XR.InputDevices.GetDevices(AllDevices);

        if (AllDevices.Count < 3) //Some device was disconnected, need to look for and reassign devices
        {
            Debug.Log("Only " + AllDevices.Count + " Devices. Something Disconnected, Looking for devices...");
            reassign_controllers = true;
        }

        else if (AllDevices.Count >= 3 && reassign_controllers == true)
        {
            List<UnityEngine.XR.InputDevice> lefties = new List<UnityEngine.XR.InputDevice>();
            List<UnityEngine.XR.InputDevice> righties = new List<UnityEngine.XR.InputDevice>();
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, lefties);
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, righties);
            LHcontroller = lefties[0];
            RHcontroller = righties[0];

            Debug.Log("Just Discovered:" + LHcontroller.name);
            Debug.Log("Just Discovered:" + RHcontroller.name);
            /*
            Debug.Log("Total Devices:" + AllDevices.Count);
            Debug.Log("LH Devices: " + lefties.Count);
            Debug.Log("RH Devices: " + righties.Count);
            Debug.Log("Left Controller:" + LHcontroller.characteristics);
            Debug.Log("Right Controller:" + RHcontroller.characteristics); 
            */
            reassign_controllers = false;
        }

        else if (AllDevices.Count >= 3 && reassign_controllers == false)
        {
            Debug.Log(LHcontroller.name + "was working");
            Debug.Log(RHcontroller.name + "was working");
        }
    }
}



