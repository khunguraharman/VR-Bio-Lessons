using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RightHandPresence : MonoBehaviour
{
    List<UnityEngine.XR.InputDevice> AllDevices = new List<UnityEngine.XR.InputDevice>();
    public GameObject[] ControllerPrefabs = new GameObject[4]; 
    public static InputDevice hand_controller { get; private set; }    
    bool assign_controller = true; // will need to assign controllers on startup
    // Start is called before the first frame update

    private void Awake()
    {        
        CheckDevices();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    void CheckDevices()
    {
        UnityEngine.XR.InputDevices.GetDevices(AllDevices);

        if (AllDevices.Count < 3) //Some device was disconnected, need to look for and reassign devices
        {
            //Debug.Log("Only " + AllDevices.Count + " Devices. Something Disconnected, Looking for devices...");
            assign_controller = true;
        }

        if (AllDevices.Count >= 3 && assign_controller == true)
        {
            List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();
            
            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, devices);
            
            hand_controller = devices[0];
            
            //Debug.Log("Just Discovered:" + hand_controller.name);
            
            GameObject controller_model = System.Array.Find(ControllerPrefabs, controller => controller.name == hand_controller.name);
            if (controller_model)
            {
                Instantiate(controller_model, transform);
            }            
            

            assign_controller = false;
        }
        
    }
}
