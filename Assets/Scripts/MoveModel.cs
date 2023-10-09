using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;
using UnityEngine.InputSystem;

public class MoveModel : MonoBehaviour
{
    private Vector2 XZBounds = LeftHandPresence.XZ_Boundary_Offset;
    // Start is called before the first frame update
    private UnityEngine.XR.InputDevice LH = LeftHandPresence.hand_controller;
    private UnityEngine.XR.InputDevice RH = RightHandPresence.hand_controller;
    private Quaternion Default_Rotation;

    private float[] scalors = new float[] { 0.5f, 0.01f, 0.005f, 0.001f, 0.0001f };

    private Vector2 X_bounds;
    private Vector2 Z_bounds;

    private InputActionAsset default_actions_asset;
    
    private InputAction move;
    private InputAction rotate;
    private InputAction pressedA;

    private void Awake()
    {
        Default_Rotation.eulerAngles = new Vector3(0f, 180f, 0f);
        Debug.Log("The spawn location is: " + gameObject.transform.position);
        X_bounds[0] = LeftHandPresence.spawn_point.x + XZBounds.x;
        X_bounds[1] = LeftHandPresence.spawn_point.x - XZBounds.x;
        Z_bounds[0] = LeftHandPresence.spawn_point.z + XZBounds.y;
        Z_bounds[1] = LeftHandPresence.spawn_point.z - XZBounds.y;
        Debug.Log("X bounds are: " + X_bounds);
        Debug.Log("Y bounds are: " + Z_bounds);

        default_actions_asset = Resources.Load<InputActionAsset>("XRI Default Input Actions");
        //Ensure both are
        move = default_actions_asset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move");
        rotate = default_actions_asset.FindActionMap("XRI RightHand Locomotion").FindAction("Move");
        pressedA = default_actions_asset.FindActionMap("XRI RightHand Interaction").FindAction("PressedA");
        if (move==null) 
        { 
            Debug.Log("could not find move"); 
        }
        else
        {
            Debug.Log("found move");
        }
        if (rotate == null)
        {
            Debug.Log("could not find rotate");
        }
        else
        {
            Debug.Log("found rotate");
        }
        move.performed += ControlTranslation;
        move.Enable();
        rotate.performed += ControlRotations;
        rotate.Enable();
        pressedA.performed += ResetOrientation;
        pressedA.Enable(); 

    }      

    void ControlTranslation(InputAction.CallbackContext context)
    {        
        Vector2 command = context.ReadValue<Vector2>();
        int i = 0;
        float move_LR = scalors[i] * command[0];
        while ((gameObject.transform.position.x + move_LR >= X_bounds[0] || gameObject.transform.position.x + move_LR <= X_bounds[1]) && i < scalors.Length-1) 
        {
            i++;
            move_LR = scalors[i] * command[0];
        }

        i = 0;
        float move_UD = scalors[i] * command[1];
        while ((gameObject.transform.position.z + move_UD >= Z_bounds[0] || gameObject.transform.position.z + move_UD <= Z_bounds[1]) && i < scalors.Length-1)
        {
            i++;
            move_LR = scalors[i] * command[1];
        }
        LeftHandPresence.CurrentLesson.UpdateModelBounds();
            
        Vector3 translation = new Vector3(move_LR, 0, move_UD);
        gameObject.transform.Translate(0.02f * translation, Space.World);
        //new Cognitive3D.CustomEvent("Translated Model").SetProperties(new Dictionary<string, object> { { "Model Position", "Model Translation" } }).Send(gameObject.transform.position);
       
    }

    void ControlRotations(InputAction.CallbackContext context)
    {        
        Vector2 command = context.ReadValue<Vector2>();
        
        if (Math.Abs(command[0]) >= Math.Abs(command[1]))
            {
                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 0.5f * command[0]);
            }

            else if(Math.Abs(command[1]) >= Math.Abs(command[0]))
            {
                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, 0.5f * command[1]);
            }

            //new Cognitive3D.CustomEvent("Rotated Model").SetProperties(new Dictionary<string, object> { { "Model Orientation", "Model Rotation" } }).Send(gameObject.transform.eulerAngles);

            LeftHandPresence.CurrentLesson.UpdateModelBounds();               
    }
    private void OnDisable()
    {
        move.performed -= ControlTranslation;
        move.Disable();
        rotate.performed -= ControlRotations;
        rotate.Disable();
        pressedA.performed -= ResetOrientation;
        pressedA.Disable();
    }
    void ResetOrientation(InputAction.CallbackContext context)
    {
        gameObject.transform.rotation = Default_Rotation;

        LeftHandPresence.CurrentLesson.UpdateModelBounds();
    }
}
