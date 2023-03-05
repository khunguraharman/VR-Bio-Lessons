
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using SlimUI.ModernMenu;
using System;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandPresenceMainMenu : MonoBehaviour
{
    public static string build_info = "0_0_1";
    List<UnityEngine.XR.InputDevice> AllDevices = new List<UnityEngine.XR.InputDevice>();

    public GameObject[] ControllerPrefabs = new GameObject[4];
    public static InputDevice hand_controller { get; private set; }
    bool assign_controller_models = true; // will need to assign controllers on startup     

    [Header("Audio Mixer Groups")]
    public AudioMixerGroup AMG_UI_object;
    public AudioMixerGroup AMG_VO_object;
    public static AudioMixerGroup AMG_UI;
    public static AudioMixerGroup AMG_VO;    

    private void Awake()
    {
        AMG_UI = AMG_UI_object;
        AMG_VO = AMG_VO_object;        
        
        Application.targetFrameRate = 72;         
        XRRayInteractor LeftHandInteractable = gameObject.GetComponent<XRRayInteractor>();
        Debug.Log("The LH controller interaction mask is " + LeftHandInteractable.interactionLayers); // does this output a string that is the name or an int that is the value property?
    }   

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckDevices_AssignModels();       
    }

    void CheckDevices_AssignModels()
    {
        UnityEngine.XR.InputDevices.GetDevices(AllDevices);

        if (AllDevices.Count < 3) //Some device was disconnected, need to look for and reassign devices
        {
            //Debug.Log("Only " + AllDevices.Count + " Devices. Something Disconnected, Looking for devices...");
            assign_controller_models = true;
        }

        if (AllDevices.Count >= 3 && assign_controller_models == true)
        {
            List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, devices);

            hand_controller = devices[0];

            //Debug.Log("Just Discovered:" + hand_controller.name);

            GameObject controller_model = System.Array.Find(ControllerPrefabs, controller => controller.name == hand_controller.name);
            if (controller_model)
            {
                Instantiate(controller_model, transform);
            }

            assign_controller_models = false;
        }

    }
    

    

    
    
    


}
