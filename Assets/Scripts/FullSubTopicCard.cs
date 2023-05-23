using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System; 


public class FullSubTopicCard : MonoBehaviour
{
    public TextMeshPro DisplayTime; 
    [NonSerialized]
    public AudioSource VoiceOver = null;
	public Image LecturerPic;
    private int playpausebool = 0;
    [NonSerialized]
    public bool sliderselected = false; 
    public Image PlayPauseBtnImg;
    public Sprite[] PlayPauseImgs = new Sprite[2];
    //public Image[]  =
    [NonSerialized]
    public TimeSpan MaxTime;
    public Slider AudioPlaybackSlider;
    [NonSerialized]
    public string MaxTimetext;
    

    private void Awake()
    {
        AudioPlaybackSlider.onValueChanged.AddListener((v) => SliderChanged(v));
        LecturerPic.material = LeftHandPresence.CurrentLecturerFace; 
        UpdateVOAndSlider();
    }

    void Update()
    {
        if(VoiceOver != null && VoiceOver.gameObject != null)
        {
            try
            {
                if (VoiceOver.time == VoiceOver.clip.length) //if reaching end, switch pause sprite to play sprite
                {
                    playpausebool = 0;
                    PlayPauseBtnImg.sprite = PlayPauseImgs[playpausebool];
                    VoiceOver.time = 0;
                }

                if (!sliderselected)
                {
                    AudioPlaybackSlider.value = VoiceOver.time;
                }
            }
            catch(UnityException ex)
            {
                Debug.Log("Just ignore this error");
            }
            finally
            {

            }
            
        }
        
               
    }


    public void PlayPauseVoiceOver()
    {
        Debug.Log("The playpausebool was:" + playpausebool);
        playpausebool = playpausebool ^ 1; // XOR if 0 then 1, if 1 then 0 (short hand for toggling)
        Debug.Log("The playpausebool is now:" + playpausebool);
        PlayPauseBtnImg.sprite = PlayPauseImgs[playpausebool]; // if 1 show pause, if 0 show play
        if (playpausebool == 1)
        {
            if (VoiceOver.time == 0)
            {
                VoiceOver.Play();
            }

            else
            {
                VoiceOver.UnPause();
            }
        }
        else
        {
            VoiceOver.Pause();
        }      
    }

    public void RepeatVoiceOver()
    {        
        VoiceOver.time = 0;
        VoiceOver.Play();
        Debug.Log(VoiceOver.name + "should have played");
    }

    public void CloseFullTopicCard()
    {
        VoiceOver.Stop();
        Destroy(gameObject);
    }

    public void UpdateVOAndSlider()
    {
        int SubTopicIndex = LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard;
        Debug.Log("Audio file being requested is of sub topic index: " + SubTopicIndex);
        if(SubTopicIndex< LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers.Length)
        {
            try
            {
                VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
            }
            catch(IndexOutOfRangeException e)
            {
                VoiceOver = null;
                Debug.Log("The voice over is missing and is being assigned null");
            }
            finally
            {
                Debug.Log("Caught Index out of range error");
            }
            
        }
        else
        {
            VoiceOver = null;
            Debug.Log("The voice over is missing and is being assigned null");
        }
        try
        {
            VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
            VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
            //VoiceOver.volume = LeftHandPresence.LessonVolume;
            MaxTime = TimeSpan.FromSeconds(VoiceOver.clip.length);
            MaxTimetext = MaxTime.ToString(@"mm\:ss");
            //Debug.Log("The maximum time is:" + MaxTimetext);
            AudioPlaybackSlider.maxValue = VoiceOver.clip.length;
            AudioPlaybackSlider.minValue = 0;
            AudioPlaybackSlider.value = 0;
            DisplayTime.text = "0:00/" + MaxTime.ToString(@"mm\:ss");
        }
        catch
        {

        }
        
    }

    public void SelectedSlider()
    {
        
        VoiceOver.Pause();
        VoiceOver.mute = true;
        sliderselected = true;
        //Debug.Log("You selected the slider and the playpauseboolean is: " + playpausebool);
        /*
        VoiceOver.Pause();
        
        Debug.Log("sliderselected: " + sliderselected.ToString());
        VoiceOver.mute = true;
        */
    }

    public void DraggingSlider()
    {
        Debug.Log("Prior to pausing, the VO playing was:" + VoiceOver.isPlaying);
        VoiceOver.Pause();
        Debug.Log("Now the VO playing is:" + VoiceOver.isPlaying);
        VoiceOver.mute = true;
        sliderselected = true;
        Debug.Log("You began dragging the slider and the playpauseboolean is: " + playpausebool);
        /*
        VoiceOver.Pause();
        
        Debug.Log("sliderselected: " + sliderselected.ToString());
        VoiceOver.mute = true;
        */
    }

    public void DiselectedSlider()
    {
        
        sliderselected = false;
        Debug.Log("The slider is deselected and the playpauseboolean is: " + playpausebool);
        VoiceOver.mute = false;
        if (playpausebool == 1)
        {
            Debug.Log("VO should have resumed");
            VoiceOver.Play();
        }
        

        
        /*
        Debug.Log("Diselected, sliderselected: " + sliderselected.ToString());
        VoiceOver.time = AudioPlaybackSlider.value;
        VoiceOver.mute = false;   
        */
    }

    public void StoppedDraggingSlider()
    {
        sliderselected = false;
        Debug.Log("You stopped dragging the slider and the playpauseboolean is: " + playpausebool);
        VoiceOver.mute = false;
        if (playpausebool == 1)
        {
            Debug.Log("VO should have resumed");
            VoiceOver.UnPause();
        }
    }

    void SliderChanged(float thevalue)
    {
        if (sliderselected) // user is manually moving the slider
        {            
            VoiceOver.time = thevalue; //overwrite VO time
        }        
        
        TimeSpan current_time = TimeSpan.FromSeconds(thevalue);
        string PlaybackTime = current_time.ToString(@"mm\:ss");
        DisplayTime.text = PlaybackTime + "/" + MaxTimetext;
    }
}

	
		
