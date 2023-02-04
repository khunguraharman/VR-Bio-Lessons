using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ExpandContractAnimation : MonoBehaviour
{

    public Animation Animations;
    public Image TopRight;
    public Image BottomLeft;
    public TextMeshProUGUI ActionName;

    private Vector3 PointOutwards_TopRight;
    private Vector3 PointOutwards_BottomLeft;

    private bool Expand = false;
    private AnimationEvent Expanded_Eye;
    private AnimationEvent Contracted_Eye;

    private float[] Sprite_Rotations = new float[2];
    private float last_speed; 
    // Start is called before the first frame update
    void Awake()
    {
        PointOutwards_TopRight = TopRight.rectTransform.localEulerAngles;
        //Sprite_Rotations[0] = localrot.z;

        PointOutwards_BottomLeft = BottomLeft.rectTransform.localEulerAngles;
        //Sprite_Rotations[1] = localrot.z;        
    }

    private void Start()
    {
        Animations["contract"].speed = 100f;
        Animations.Play("contract"); //ensure eye is contracted

        Expanded_Eye = new AnimationEvent();
        Expanded_Eye.intParameter = 0;        
        Expanded_Eye.time = Animations["expand"].length/LeftHandPresence.AnimationSpeed;
        last_speed = LeftHandPresence.AnimationSpeed;
        Expanded_Eye.functionName = "CallUpdateModelBounds";
        Animations.GetClip("expand").AddEvent(Expanded_Eye);

        Contracted_Eye = new AnimationEvent();
        Contracted_Eye.intParameter = 0;
        Contracted_Eye.time = 0;
        Contracted_Eye.functionName = "CallUpdateModelBounds";
        Animations.GetClip("contract").AddEvent(Contracted_Eye);

    }

    private void FixedUpdate()
    {
        if(LeftHandPresence.AnimationSpeed != last_speed)
        {
            Expanded_Eye.time = Animations["expand"].length / LeftHandPresence.AnimationSpeed;
            last_speed = LeftHandPresence.AnimationSpeed;
        }
        
    }
    public void Expand_Contract()
    {
        Expand = !Expand;
    
        if (Expand) //we want to run the expand animation and change the arrows to point inwards
        {
        TopRight.rectTransform.localEulerAngles = PointOutwards_BottomLeft;
        BottomLeft.rectTransform.localEulerAngles = PointOutwards_TopRight;
        Animations["expand"].speed = LeftHandPresence.AnimationSpeed;
        Animations.Play("expand");
        ActionName.text = "Contract";
        }
    
        else //we want to run the contract animation and change the arrows to point outwards
        {
        TopRight.rectTransform.localEulerAngles = PointOutwards_TopRight;
        BottomLeft.rectTransform.localEulerAngles = PointOutwards_BottomLeft;
        Animations["contract"].speed = LeftHandPresence.AnimationSpeed;
        Animations.Play("contract");
        ActionName.text = "Expand";
        }

    }

    void CallUpdateModelBounds()
    {
        LeftHandPresence.CurrentLesson.UpdateModelBounds();
    }
}
