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

    private float[] Sprite_Rotations = new float[2];
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
}
