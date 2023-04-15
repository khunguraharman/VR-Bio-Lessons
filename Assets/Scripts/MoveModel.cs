using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System;

public class MoveModel : MonoBehaviour
{
    private Vector2 XZBounds = LeftHandPresence.XZ_Boundary_Offset;
    // Start is called before the first frame update
    private InputDevice LH = LeftHandPresence.hand_controller;
    private InputDevice RH = RightHandPresence.hand_controller;
    private Quaternion Default_Rotation;

    private float[] scalors = new float[] { 0.5f, 0.01f, 0.005f, 0.001f, 0.0001f };

    private Vector2 X_bounds;
    private Vector2 Z_bounds;

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
    }
    

    // Update is called once per frame
    void FixedUpdate()
    {
        ControlTranslation();
        ControlRotations();
    }

    void ControlTranslation()
    {
        Vector2 command = new Vector2(0,0);
        if (LH.TryGetFeatureValue(CommonUsages.primary2DAxis, out command)) //if both 0, do nothing
        {
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
            /*
            float move_LR = 0.5f*command[0];
            if(gameObject.transform.position.x + move_LR >= LeftHandPresence.spawn_point.x + XZBounds.x || gameObject.transform.position.x + move_LR <= LeftHandPresence.spawn_point.x  - XZBounds.x)
            {
                move_LR = 0;
            }

            float move_UD = 0.5f * command[1];
            if (gameObject.transform.position.z + move_UD >= LeftHandPresence.spawn_point.z + XZBounds.y || gameObject.transform.position.z + move_UD <=   LeftHandPresence.spawn_point.z - XZBounds.y)
            {
                move_UD = 0;
            }            
            */
            Vector3 translation = new Vector3(move_LR, 0, move_UD);
            gameObject.transform.Translate(0.02f * translation, Space.World);
        }
    }

    void ControlRotations()
    {
        Vector2 command = new Vector2(0, 0);
        if (RH.TryGetFeatureValue(CommonUsages.primary2DAxis, out command)) //if both 0, do nothing
        {
            //Vector3 spin = new Vector3(command[0], command[1], 0);
            if(Math.Abs(command[0]) >= Math.Abs(command[1]))
            {
                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.up, 0.5f * command[0]);
            }

            else if(Math.Abs(command[1]) >= Math.Abs(command[0]))
            {
                gameObject.transform.RotateAround(gameObject.transform.position, Vector3.right, 0.5f * command[1]);
            }

            LeftHandPresence.CurrentLesson.UpdateModelBounds();
        }
        bool PressedA;
        if(RH.TryGetFeatureValue(CommonUsages.primaryButton, out PressedA) && PressedA)
        {
            gameObject.transform.rotation = Default_Rotation;

            LeftHandPresence.CurrentLesson.UpdateModelBounds();
        }
    }
}
