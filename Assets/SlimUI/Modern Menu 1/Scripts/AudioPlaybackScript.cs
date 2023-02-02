using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System; 


public class AudioPlaybackScript : MonoBehaviour
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

    private void Update()
    {
        
        if (VoiceOver.time == VoiceOver.clip.length) //if reaching end, switch pause sprite to play sprite
        {
            playpausebool = 0;
            PlayPauseBtnImg.sprite = PlayPauseImgs[playpausebool];
        }       
        
        
        if (!sliderselected)
        {
            
            AudioPlaybackSlider.value = VoiceOver.time;
            /*
            if (!VoiceOver.isPlaying && playpausebool == 1)
            {
                VoiceOver.Play(); 
            }
            */

        }

        
    }


    public void PlayPauseVoiceOver()
    {
        playpausebool = playpausebool ^ 1; // XOR if 0 then 1, if 1 then 0 (short hand for toggling)
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
        // VoiceOver = LessonModel.ChosenLecturer.SubTopicVoiceOvers[LessonModel.SubTopicIndex]; // update audio track
        VoiceOver.time = 0;
        VoiceOver.Play();
        Debug.Log(VoiceOver.name + "should have played");
    }

    public void CloseFullTopicCard()
    {
        Destroy(gameObject);
    }

    public void UpdateVOAndSlider()
    {
        int SubTopicIndex = LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard;
        VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
        MaxTime = TimeSpan.FromSeconds(VoiceOver.clip.length);
        MaxTimetext = MaxTime.ToString(@"mm\:ss");
        Debug.Log(MaxTimetext);
        AudioPlaybackSlider.maxValue = VoiceOver.clip.length;
        AudioPlaybackSlider.minValue = 0;
        AudioPlaybackSlider.value = 0;
        DisplayTime.text = "0:00/" + MaxTimetext;

    }

    public void SelectedSlider()
    {
        sliderselected = true;
        /*
        VoiceOver.Stop();
        
        Debug.Log("sliderselected: " + sliderselected.ToString());
        VoiceOver.mute = true;
        */
    }

    public void DiselectedSlider()
    {
        sliderselected = false;
        /*
        Debug.Log("Diselected, sliderselected: " + sliderselected.ToString());
        VoiceOver.time = AudioPlaybackSlider.value;
        VoiceOver.mute = false;  
        */
    }
    
    void SliderChanged(float thevalue)
    {
        if (sliderselected) // user is manually moving the slider
        {
            VoiceOver.mute = true;
            VoiceOver.time = thevalue; //overwrite VO time
        }        
        
        TimeSpan current_time = TimeSpan.FromSeconds(thevalue);
        string PlaybackTime = current_time.ToString(@"mm\:ss");
        DisplayTime.text = PlaybackTime + "/" + MaxTimetext;
        VoiceOver.mute = false;

    }
}

	
		
