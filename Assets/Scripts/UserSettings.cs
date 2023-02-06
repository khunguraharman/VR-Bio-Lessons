using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Audio; 


public class UserSettings : MonoBehaviour
{

	public Settings MainSettings;

	[Header("Audio Settings")]
	public GameObject MenuVolume = null;
	public Slider MenuVolumeSlider = null;
	public TextMeshPro MenuVolumeText = null;
	public bool EnableMenuSounds = true;

		
	public GameObject LessonVolume = null;
	public Slider LessonVolumeSlider = null;
	public TextMeshPro LessonVolumeText = null;
	public bool EnableLessonSounds = true;

	[Header("Visual Settings")]
	public Slider PreviewTextSlider;
	public TextMeshPro PreviewTextScaleText;
	public Slider LessonTextSlider;
	public TextMeshPro LessonTextScaleText;
	public Slider ModelScaleSlider;
	public TextMeshPro ModelScaleText;
	public Slider ModelVerticalPosSlider;
	public TextMeshPro ModelVerticalPosText;

	[Header("Animation Settings")]
	public Slider AnimSpeedSlider;
	public TextMeshPro AnimSpeedText;

	[Header("Metadata")]
	public RectTransform BoundingRectangle;


	public int SettingsIndex { get; private set; }		

	void Awake()
	{
		InitializeMenuAudioSlider();
		InitializeLessonAudioSlider();
		InitializePreviewTextSlider();
		InitializeLessonTextSlider();
		InitializeModelScaleSlider();
		InitializeModelPosSlider();
	}


	public void Update()
	{

	}

	public void PlayHover()
	{
		XRMenu.PlayHover();
	}

	public void Confirm()
	{
		Destroy(gameObject);
		XRMenu.DestroyMainMenu();
		//Destroy(LeftHandPresence.existingmenu.gameObject);
	}

	public void Close()
	{
		Destroy(gameObject);
			
	}

	void InitializeMenuAudioSlider()
	{
		MenuVolumeSlider.maxValue = 100;
		MenuVolumeSlider.minValue = 0;			
		float slidervalue_ui;
		LeftHandPresence.AMG_UI.audioMixer.GetFloat("UIMasterVolume", out slidervalue_ui);
		slidervalue_ui = from_decibels(slidervalue_ui);
		MenuVolumeText.text = slidervalue_ui.ToString("0"); // update the string read out
		MenuVolumeSlider.value = slidervalue_ui; 			
		MenuVolumeSlider.onValueChanged.AddListener(UpdateMenuVolume);
	}

	void InitializeLessonAudioSlider()
	{
		LessonVolumeSlider.maxValue = 100;
		LessonVolumeSlider.minValue = 0;
		float slidervalue_vo;			
		LeftHandPresence.AMG_VO.audioMixer.GetFloat("LecturerMasterVolume", out slidervalue_vo);
		slidervalue_vo = from_decibels(slidervalue_vo);
		LessonVolumeText.text = slidervalue_vo.ToString("0");

		LessonVolumeSlider.value = slidervalue_vo; 
		LessonVolumeSlider.onValueChanged.AddListener(UpdateLessonVolume);
	}	

	public void ToggleMenuSounds()
	{
		EnableMenuSounds = !EnableMenuSounds;

		if (EnableMenuSounds) //Menu sounds should play
		{
			XRMenu.hoverSound.mute = false;
			MenuVolume.SetActive(true);
			InitializeMenuAudioSlider();
		}

		else
		{
			XRMenu.hoverSound.mute = true;
			MenuVolume.SetActive(false);
		}
	}

	public void ToggleLessonSounds()
	{
		EnableLessonSounds = !EnableLessonSounds;

		if (EnableLessonSounds) //Menu sounds should play
		{
			XRMenu.hoverSound.mute = false;
			LessonVolume.SetActive(true);
			InitializeLessonAudioSlider();
		}

		else
		{
			XRMenu.hoverSound.mute = true;
			LessonVolume.SetActive(false);
		}
	}

	void UpdateMenuVolume(float value)
	{
		MenuVolumeText.text = value.ToString("0"); // update the string read out
		LeftHandPresence.AMG_UI.audioMixer.SetFloat("UIMasterVolume", to_decibels(value)); //write to the audio mixer in decibels
																			//XRMenu.hoverSound.volume = value / 100f; //audio source volume is between 0 and 1
		Debug.Log("The menu volume has be set to " + value + " or" + to_decibels(value) + " decibels");
	}

	void UpdateLessonVolume(float value)
	{
		LessonVolumeText.text = value.ToString("0");			
		LeftHandPresence.AMG_VO.audioMixer.SetFloat("LecturerMasterVolume", to_decibels(value)); //write to the audio mixer in decibles		
	}

	public void AudioPanel()
	{
		SettingsIndex = 0;
		MainSettings.ShowPanel(SettingsIndex);
	}

	public void VisualPanel()
	{
		SettingsIndex = 1;
		MainSettings.ShowPanel(SettingsIndex);
	}		

	public void AnimationSettingsPanel()
	{
		//PlayHover();
		SettingsIndex = 2;
		MainSettings.ShowPanel(SettingsIndex);
	}

	float to_decibels(float slidervalue)
	{
		float in_decibels = 20 * Mathf.Log10((slidervalue * 0.01f * 9.999f) + 0.0001f);
		return in_decibels;
	}

	float from_decibels(float decibel_value)
	{
		float slidervalue = (float)Math.Pow(10, decibel_value * 0.05f) - 0.001f;
		slidervalue = slidervalue * 100 / 9.999f;
		return slidervalue;
	}

	void InitializePreviewTextSlider()
	{
		PreviewTextSlider.maxValue = 125;
		PreviewTextSlider.minValue = 75;

		PreviewTextScaleText.text = (LeftHandPresence.PreviewScale * 100).ToString("0");
		PreviewTextSlider.value = LeftHandPresence.PreviewScale * 100; // always show the value that was last set

		PreviewTextSlider.onValueChanged.AddListener(UpdatePreviewScale);
	}

	void UpdatePreviewScale(float value)
	{
		PreviewTextScaleText.text = value.ToString("0");
		LeftHandPresence.PreviewScale = value / 100; //want slider value of 100 to correspond to 1	
		if(LeftHandPresence.CurrentPreview)
        {
			LeftHandPresence.CurrentPreview.transform.localScale = LeftHandPresence.PreviewScale * LeftHandPresence.PreviewDefaultScale;
        }

		//Debug.Log("The VO volume has be set to " + value + " or" + to_decibels(value) + " decibels");
		//LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard].volume = value / 100f; //Volume is 0 to 1, but slider is 0 to 100
		//LeftHandPresence.LessonVolume = value; 
	}

	void InitializeLessonTextSlider()
	{
		LessonTextSlider.maxValue = 125;
		LessonTextSlider.minValue = 75;
			
		LessonTextScaleText.text = (LeftHandPresence.FullLessonScale * 100).ToString("0");
		LessonTextSlider.value = LeftHandPresence.FullLessonScale * 100;
			
		LessonTextSlider.onValueChanged.AddListener(UpdateLessonTextScale);
	}

	void UpdateLessonTextScale(float value)
	{
		LessonTextScaleText.text = value.ToString("0");
		LeftHandPresence.FullLessonScale = value / 100; //want slider value of 100 to correspond to 1		

		if (LeftHandPresence.CurrentSubTopicCard) // if a Subtopic card is currently instantiated, we need to scale it
        {				
			LeftHandPresence.CurrentSubTopicCard.transform.localScale = LeftHandPresence.FullLessonScale * LeftHandPresence.FullLessonDefaultScale;
		}

		//Debug.Log("The VO volume has be set to " + value + " or" + to_decibels(value) + " decibels");
		//LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard].volume = value / 100f; //Volume is 0 to 1, but slider is 0 to 100
		//LeftHandPresence.LessonVolume = value; 
	}

	void InitializeModelScaleSlider()
	{
		ModelScaleSlider.maxValue = 125;
		ModelScaleSlider.minValue = 75;

		ModelScaleText.text = (LeftHandPresence.LessonModelScale * 100).ToString("0");
		ModelScaleSlider.value = LeftHandPresence.LessonModelScale * 100;

		ModelScaleSlider.onValueChanged.AddListener(UpdateModelScale);
	}

	void UpdateModelScale(float value)
	{
		ModelScaleText.text = value.ToString("0");
		LeftHandPresence.LessonModelScale = value / 100; //want slider value of 100 to correspond to 1		

		if (LeftHandPresence.CurrentLesson) // if a Subtopic card is currently instantiated, we need to scale it
		{
			LeftHandPresence.CurrentLesson.transform.localScale = LeftHandPresence.LessonModelScale * LeftHandPresence.LessonModelDefaultScale;
		} 
	}

	void InitializeModelPosSlider()
    {
		ModelVerticalPosSlider.maxValue = 1.3f;
		ModelVerticalPosSlider.minValue = 1f;

		if (LeftHandPresence.CurrentLesson)
        {
			ModelVerticalPosText.text = (LeftHandPresence.CurrentLesson.transform.position.y).ToString("0.00");
			ModelVerticalPosSlider.value = LeftHandPresence.CurrentLesson.transform.position.y;
		}

        else
        {
			ModelVerticalPosText.text = (LeftHandPresence.spawn_point.y).ToString("0.00");
			ModelVerticalPosSlider.value = LeftHandPresence.spawn_point.y;
		}

		ModelVerticalPosSlider.onValueChanged.AddListener(UpdateModelPos);
	}

	void UpdateModelPos(float value)
    {
		ModelVerticalPosText.text = value.ToString("0.00");

		Vector3 new_pos = LeftHandPresence.CurrentLesson.transform.position;

		if (LeftHandPresence.CurrentLesson) // if a Subtopic card is currently instantiated, we need to scale it
		{
			LeftHandPresence.CurrentLesson.transform.position = new Vector3(new_pos.x, value, new_pos.z);
		}
	}

	void InitializeAnimSpeedSlider()
	{
		AnimSpeedSlider.maxValue = 2.0f;
		AnimSpeedSlider.minValue = 0.1f;		

			
		AnimSpeedSlider.value = LeftHandPresence.AnimationSpeed;
		AnimSpeedText.text = AnimSpeedSlider.value.ToString("0.00");



		AnimSpeedSlider.onValueChanged.AddListener(UpdateAnimSpeed);
	}

    void UpdateAnimSpeed(float value)
    {
		LeftHandPresence.AnimationSpeed = value;
		AnimSpeedText.text = value.ToString("0.00");
	}
}

	
		
