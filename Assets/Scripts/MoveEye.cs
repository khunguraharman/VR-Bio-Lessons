using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEye : MonoBehaviour
{
    public Animation Eye_Animation;
    private AnimationEvent Rewind_Elevation;
    private AnimationEvent Rewind_Depression;
    private AnimationEvent Rewind_Left_Abduction;
    private AnimationEvent Rewind_Right_Adduction;
    // Start is called before the first frame update
    void Start()
    {
        Eye_Animation.Play("idle");

        Rewind_Elevation = new AnimationEvent();
        Rewind_Elevation.intParameter = 0;
        Rewind_Elevation.time = 0.22f;
        Rewind_Elevation.functionName = "RewindAnim";
        Eye_Animation.GetClip("updown").AddEvent(Rewind_Elevation);

        Rewind_Depression = new AnimationEvent();
        Rewind_Depression.intParameter = 1;
        Rewind_Depression.time = 2.5f;
        Rewind_Depression.functionName = "RewindAnim";
        Eye_Animation.GetClip("updown").AddEvent(Rewind_Depression);

        Rewind_Left_Abduction = new AnimationEvent(); // abduction
        Rewind_Left_Abduction.intParameter = 2;
        Rewind_Left_Abduction.time = 0.25f;
        Rewind_Left_Abduction.functionName = "RewindAnim";
        Eye_Animation.GetClip("sides").AddEvent(Rewind_Left_Abduction);

        Rewind_Right_Adduction = new AnimationEvent();
        Rewind_Right_Adduction.intParameter = 3;
        Rewind_Right_Adduction.time = 2.33f;
        Rewind_Right_Adduction.functionName = "RewindAnim";
        Eye_Animation.GetClip("sides").AddEvent(Rewind_Right_Adduction);

    }
    public void Elevate()
    {
        Eye_Animation["updown"].normalizedTime = 0.0f;
        Eye_Animation["updown"].speed = LeftHandPresence.AnimationSpeed;
        Eye_Animation.Play("updown"); 
    }

    public void Depress()
    {
        Eye_Animation["updown"].normalizedTime = 1f;
        Eye_Animation["updown"].speed = -1f*LeftHandPresence.AnimationSpeed*2.873f; //(3.133-2.5)/0.22 = 2.873
        Eye_Animation.Play("updown");
    }

    public void Abduction_Left()
    {
        Eye_Animation["sides"].normalizedTime = 0.0f;
        Eye_Animation["sides"].speed = LeftHandPresence.AnimationSpeed;
        Eye_Animation.Play("sides");
    }

    public void Adduction_Right()
    {
        Eye_Animation["sides"].normalizedTime = 1f;
        Eye_Animation["sides"].speed = -1f*LeftHandPresence.AnimationSpeed*3.48f; //(3.2-2.33)/0.25 = 3.48
        Eye_Animation.Play("sides");
    }

    public void RewindAnim(int i)
    {
        if(i==0)
        {
            Debug.Log("The elevation animation should be reversed");
            Eye_Animation["updown"].speed = -1f*LeftHandPresence.AnimationSpeed;                
        }
        
        else if(i==1)
        {
            Debug.Log("The depression animation should be reversed");
            Eye_Animation["updown"].speed = LeftHandPresence.AnimationSpeed * 2.873f; //(3.133-2.5)/0.22 = 2.873
        }

        else if (i == 2)
        {
            Debug.Log("The abduction/left animation should be reversed");
            Eye_Animation["sides"].speed = -1f*LeftHandPresence.AnimationSpeed;
        }

        else if (i == 3)
        {
            Debug.Log("The adduction/right animation should be reversed");
            Eye_Animation["sides"].speed = LeftHandPresence.AnimationSpeed * 3.48f; //(3.2-2.33)/0.25 = 3.48
        }

    }
}
