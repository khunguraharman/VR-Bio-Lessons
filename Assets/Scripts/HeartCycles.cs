using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class HeartCycles : MonoBehaviour
{
    public Sprite[] PlayPauseSprites = new Sprite[2];

    public Animation Heart_Animation;
    private AnimationEvent SystolePause;
    private AnimationEvent DiastolePause;
    private Toggle[] All_Toggles = new Toggle[3];

    [Header("Full Cardiac Cycle")]
    public Toggle FullCycleToggle;
    public Slider HeartRateSlider;
    public TextMeshPro HeartRateText;
    
    private float default_BPM;
    public string BPM_Readout { get; private set; }
    public float BPM_Scalar { get; private set; }
    public float Target_BPM { get; private set; }

    [Header("Systole Cycle")]
    public Toggle SystoleToggle;
    public Slider SystoleSlider;
    public TextMeshPro SystoleText;
    public Button SysPlayPause;
    public Button SysRepeat;
    private bool SysEventTriggered = false;
    private bool ResetSys = false;
    private bool SysSliderSelected = false;
    private float StartofSystole = 0.345f; //normalized time
    private float EndOfSystole = 0.85f; //normalized time
    private int play_sys = 0;

    [Header("Diastole Cycle")]
    public Toggle DiastoleToggle;
    public Slider DiastoleSlider;
    public TextMeshPro DiastoleText;
    public Button DiaPlayPause;
    public Button DiaRepeat;
    private bool DiaEventTriggered = false;
    private bool ResetDia = false;
    private bool DiaSliderSelected = false;
    private float StartOfDiastole = 0.851f; //normalized time
    private float EndofDiastole = 0.344f; //normalized time
    private int play_dia = 0; 

    private void Awake()
    {
        InitializeSliders();
        Target_BPM = HeartRateSlider.value;
        default_BPM = 60 / Heart_Animation["play"].length;
        BPM_Scalar = Target_BPM / default_BPM;
        Heart_Animation["play"].speed = BPM_Scalar;

        All_Toggles[0] = FullCycleToggle;
        All_Toggles[1] = SystoleToggle;
        All_Toggles[2] = DiastoleToggle;        

        //Ensure only one of toggles is on at a time
        FullCycleToggle.onValueChanged.AddListener(FullCardiacCycleToggled);
        SystoleToggle.onValueChanged.AddListener(SystoleToggled);
        DiastoleToggle.onValueChanged.AddListener(DiastoleToggled);       

    }

    // Start is called before the first frame update
    void Start()
    {
        Heart_Animation.Play("play");

        SystolePause = new AnimationEvent();
        SystolePause.intParameter = 0;
        SystolePause.time = EndOfSystole*Heart_Animation["play"].length; // end of systole animation, switch to idle or pause
        SystolePause.functionName = "PauseAnimation";
        Heart_Animation.GetClip("play").AddEvent(SystolePause);

        DiastolePause = new AnimationEvent();
        DiastolePause.intParameter = 1;
        DiastolePause.time = EndofDiastole*Heart_Animation["play"].length; // end of diastole animation, switch to idle or pause
        DiastolePause.functionName = "PauseAnimation";
        Heart_Animation.GetClip("play").AddEvent(DiastolePause); 
    }

    private void Update()
    {
        if(SystoleToggle.isOn)
        {
            if(!SysSliderSelected && play_sys == 1 ) //user is not dragging the slider, so just update the slider
            {
                SystoleSlider.value = Heart_Animation["play"].time;
                /*
                string slidervalue = string.Format("Animation time is {0}, but the slider value was updated to {1}", Heart_Animation["play"].time, SystoleSlider.value);
                Debug.Log(slidervalue);
                */
            }
            else
            {
                Heart_Animation["play"].time = SystoleSlider.value; 
            }
        }
        
        else if(DiastoleToggle.isOn)
        {
            if(!DiaSliderSelected && play_dia == 1) //user is not dragging the slider
            {
                DiastoleSlider.value = Heart_Animation["play"].time;
            }
            else
            {
                Heart_Animation["play"].time = DiastoleSlider.value;
            }
            
        }       

    }

    public void ChangeHeartRate(float value)
    {
        BPM_Readout = string.Format("{0} BPM", value);
        HeartRateText.text = BPM_Readout;
        Target_BPM = HeartRateSlider.value;
        BPM_Scalar = Target_BPM / default_BPM;
        Heart_Animation["play"].speed = BPM_Scalar; 
    }
    public void PlayPauseSys()
    {
        play_sys = play_sys ^ 1;
        if(play_sys == 1)
        {
            if (SysEventTriggered && ResetSys) // Heart_Animation["play"].time >= 0.99f*EndOfSystole*Heart_Animation["play"].length
            {
                Heart_Animation["play"].time = StartofSystole*Heart_Animation["play"].length; //bring the animation to the first relevant frame
                string animationwasreset = string.Format("Completed systole animation so it was reset");
                Debug.Log(animationwasreset);
                SysEventTriggered = false;
            }
            Heart_Animation["play"].speed = LeftHandPresence.AnimationSpeed;
            SysPlayPause.image.sprite = PlayPauseSprites[1]; //change to pause sprite
        }
        else
        {
            Heart_Animation["play"].speed = 0f;
            SysPlayPause.image.sprite = PlayPauseSprites[0]; //change to pause sprite
        }       
    }

    public void PlayPauseDias()
    {
        play_dia = play_dia ^ 1;
        if (play_dia == 1)
        {
            if(DiaEventTriggered && ResetDia)
            {
                Heart_Animation["play"].time = StartOfDiastole * Heart_Animation["play"].length; //bring the animation to the first relevant frame
                DiaEventTriggered = false;
            }
            
            Heart_Animation["play"].speed = LeftHandPresence.AnimationSpeed;
            DiaPlayPause.image.sprite = PlayPauseSprites[1];
        }
        else
        {
            Heart_Animation["play"].speed = 0f;
            DiaPlayPause.image.sprite = PlayPauseSprites[0];
        }
    }

    public void PauseAnimation(int i)
    {
        if(i==0 && SystoleToggle.isOn)
        {            
            Heart_Animation["play"].speed = 0f;  //pause the Systole animation
            SysPlayPause.image.sprite = PlayPauseSprites[0];
            play_sys = 0;
            SysEventTriggered = true;
            ResetSys = true;
            SystoleText.text = "100%";
            Debug.Log("The EndSys Animation event triggered. The slider should be at its max value");
        }
        
        else if(i==1 && DiastoleToggle.isOn)
        {            
            Heart_Animation["play"].speed = 0f;  //pause the Systole animation  
            DiaPlayPause.image.sprite = PlayPauseSprites[0];
            play_dia = 0;
            DiaEventTriggered = true;
            ResetDia = true;
            DiastoleText.text = "100%";
        }
        
    }

    void InitializeSliders()
    {
        HeartRateSlider.maxValue = 200;
        HeartRateSlider.minValue = 0;
        HeartRateSlider.wholeNumbers = true;
        HeartRateSlider.value = 60;
        Target_BPM = HeartRateSlider.value;
        string heartrate = string.Format("{0} BPM", Target_BPM);
        HeartRateText.text = heartrate;

        HeartRateSlider.onValueChanged.AddListener(ChangeHeartRate);

        SystoleSlider.maxValue = EndOfSystole*Heart_Animation["play"].length;
        SystoleSlider.minValue = StartofSystole* Heart_Animation["play"].length;
        SystoleSlider.wholeNumbers = false;
        SystoleSlider.value = SystoleSlider.minValue;
        SystoleSlider.onValueChanged.AddListener((v) => SysSliderChanged(v));
        SystoleSlider.gameObject.SetActive(false);
        SysPlayPause.gameObject.SetActive(false);
        SysRepeat.gameObject.SetActive(false);

        DiastoleSlider.maxValue = (1+EndofDiastole)*Heart_Animation["play"].length;
        DiastoleSlider.minValue = StartOfDiastole*Heart_Animation["play"].length;
        DiastoleSlider.wholeNumbers = false;
        DiastoleSlider.value = DiastoleSlider.minValue;
        DiastoleSlider.onValueChanged.AddListener((v) => DiaSliderChanged(v));
        DiastoleSlider.gameObject.SetActive(false);
        DiaPlayPause.gameObject.SetActive(false);
        DiaRepeat.gameObject.SetActive(false);

        //SystoleSlider.onValueChanged
    }

    void FullCardiacCycleToggled(bool new_status)
    {
        if(new_status)
        {
            HeartRateSlider.gameObject.SetActive(true);
            Heart_Animation["play"].normalizedTime = 0.0f; //bring the animation to the first frame
            Heart_Animation["play"].speed = BPM_Scalar; //ensure the animation is paused            

            SystoleToggle.isOn = false;
            SystoleSlider.gameObject.SetActive(false);
            SysPlayPause.gameObject.SetActive(false);
            SysRepeat.gameObject.SetActive(false);
            play_sys = 0;

            DiastoleToggle.isOn = false;
            DiastoleSlider.gameObject.SetActive(false);
            DiaPlayPause.gameObject.SetActive(false);
            DiaRepeat.gameObject.SetActive(false);
            play_dia = 0;
        }

        else if (!new_status && (All_Toggles.Count(v => v.isOn) < 1))
        {
            FullCycleToggle.isOn = true;
        }
    }

    void SystoleToggled(bool new_status)
    {
        if (new_status)
        {
            SystoleSlider.gameObject.SetActive(true);
            SystoleSlider.value = SystoleSlider.minValue;
            SysPlayPause.gameObject.SetActive(true);
            SysRepeat.gameObject.SetActive(true);            

            Heart_Animation["play"].time = StartofSystole*Heart_Animation["play"].length; ; //bring the animation to the first frame
            Heart_Animation["play"].speed = 0.0f; //ensure the animation is paused
            SysPlayPause.image.sprite = PlayPauseSprites[0]; //ensure the play sprite is shown when the audio is paused
            play_sys = 0;

            FullCycleToggle.isOn = false;
            HeartRateSlider.gameObject.SetActive(false);
            DiastoleToggle.isOn = false;
            DiastoleSlider.gameObject.SetActive(false);
            DiaPlayPause.gameObject.SetActive(false);
            DiaRepeat.gameObject.SetActive(false);
            play_dia = 0; //ensure             
        }

        else if(!new_status && (All_Toggles.Count(v => v.isOn)<1))
        {
            SystoleToggle.isOn = true;
        }
    }

    void DiastoleToggled(bool new_status)
    {
        if (new_status)
        {
            DiastoleSlider.gameObject.SetActive(true);
            DiastoleSlider.value = DiastoleSlider.minValue;
            DiaPlayPause.gameObject.SetActive(true);
            DiaRepeat.gameObject.SetActive(true);

            Heart_Animation["play"].time = StartOfDiastole*Heart_Animation["play"].length; //bring the animation to the first relevant frame
            Heart_Animation["play"].speed = 0.0f; //ensure the animation is paused
            DiaPlayPause.image.sprite = PlayPauseSprites[0];
            play_dia = 0;

            FullCycleToggle.isOn = false;
            HeartRateSlider.gameObject.SetActive(false);
            SystoleToggle.isOn = false;
            SystoleSlider.gameObject.SetActive(false);
            SysPlayPause.gameObject.SetActive(false);
            SysRepeat.gameObject.SetActive(false);
            play_sys = 0;
        }

        else if (!new_status && (All_Toggles.Count(v => v.isOn) < 1))
        {
            DiastoleToggle.isOn = true;
        }
    }

    void SysSliderChanged(float value)
    {
        float percent =  100f*(value - SystoleSlider.minValue) / (SystoleSlider.maxValue - SystoleSlider.minValue);
        string newpercent = string.Format("{0}%", (int)percent);
        SystoleText.text = newpercent;
    }

    void DiaSliderChanged(float value)
    {
        float percent = 100f * (value - DiastoleSlider.minValue) / (DiastoleSlider.maxValue - DiastoleSlider.minValue);
        string newpercent = string.Format("{0}%", (int)percent);
        DiastoleText.text = newpercent;
    }

    public void DraggingSlider(Slider the_slider)
    {
        Heart_Animation["play"].speed = 0f; //pause any animation playing

        if (the_slider == SystoleSlider)
        {
            SysSliderSelected = true;
            ResetSys = false;
        }

        else if( the_slider == DiastoleSlider)
        {
            DiaSliderSelected = true;
            ResetDia = false;
        }
    }

    public void StoppedDraggingSlider(Slider the_slider)
    {        

        if (the_slider == SystoleSlider)
        {
            SysSliderSelected = false;            
        }

        else if (the_slider == DiastoleSlider)
        {
            DiaSliderSelected = false;
        }

        //Heart_Animation["play"].time = the_slider.value;

        if( play_sys == 1 || play_dia == 1) // continue the animation when the slider is deselected
        {
            Heart_Animation["play"].speed = LeftHandPresence.AnimationSpeed;
        }
    }
}
