using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System; 


public class FullSubTopicCardBackUp : MonoBehaviour
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
    

    private void Awake()
    {
        
		LecturerPic.material = LeftHandPresence.CurrentLecturerFace;
        int SubTopicIndex = LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard;
        VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
        MaxTime = TimeSpan.FromSeconds(VoiceOver.clip.length);
        UpdateSlider();

    }

    private void Update()
    {
        
        if (VoiceOver.time == VoiceOver.clip.length) //if reaching end, switch pause sprite to play sprite
        {
            playpausebool = 0;
            PlayPauseBtnImg.sprite = PlayPauseImgs[playpausebool];
        }

        
        string MaxTimetext = MaxTime.ToString(@"mm\:ss");

        if (!sliderselected)
        {
            float thetime = VoiceOver.time;
            TimeSpan current_time = TimeSpan.FromSeconds(thetime);
            string PlaybackTime = current_time.ToString(@"mm\:ss");
            DisplayTime.text = PlaybackTime + "/" + MaxTimetext;
            AudioPlaybackSlider.value = thetime;

            if (!VoiceOver.isPlaying && playpausebool == 1)
            {
                VoiceOver.Play(); 
            }

        }

        else
        {
            float thetime = AudioPlaybackSlider.value;
            TimeSpan current_time = TimeSpan.FromSeconds(thetime);
            string PlaybackTime = current_time.ToString(@"mm\:ss");
            DisplayTime.text = PlaybackTime + "/" + MaxTimetext;
            VoiceOver.time = thetime;
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

    public void UpdateSlider()
    {
        AudioPlaybackSlider.maxValue = VoiceOver.clip.length;
        AudioPlaybackSlider.minValue = 0;
    }

    public void SelectedSlider()
    {
        VoiceOver.Stop();
        sliderselected = true;
        Debug.Log("sliderselected: " + sliderselected.ToString());
        VoiceOver.mute = true;
    }

    public void DiselectedSlider()
    {
        sliderselected = false;
        Debug.Log("Diselected, sliderselected: " + sliderselected.ToString());
        VoiceOver.time = AudioPlaybackSlider.value;
        VoiceOver.mute = false;
        /*
        if (playpausebool == 1)
        {
            VoiceOver.Play();
        }
        */
    }
    /*
    public void SliderWritestoVoiceOver()
    {
        VoiceOver.time = AudioPlaybackSlider.value;

    }
    */
}

	
		
