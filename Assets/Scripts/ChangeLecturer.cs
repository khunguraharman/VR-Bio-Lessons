using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class ChangeLecturer : MonoBehaviour
{
		
	private Lecturer ThePerson = null;
		
	private int ViewingLecturer;
	private int ViewingPanel; 
	private int TotalLecturers;

	public Lecturer[] Lecturers = new Lecturer[2];
	//public Material LecturerFaces;//= new Material[2]; 

	[Header("Metadata")]
	public RectTransform BoundingRectangle;

	void  Awake ()
	{
		TotalLecturers = Lecturers.Length;
		for (int i=0; i<TotalLecturers; i++)
        {
			Lecturers[i].gameObject.SetActive(false);
        }			
	}

    void Start()
    {
		ViewingLecturer = LeftHandPresence.LecturerIndex;
		ViewingPanel = LeftHandPresence.PanelIndex;
						
	}

    public void Update ()
	{
			
	}
	public void PlayHover()
    {
		XRMenu.PlayHover(); 
    }

	public void Confirm()
    {
		if(LeftHandPresence.PanelIndex != ViewingPanel)
        {
			LeftHandPresence.PanelIndex = ViewingPanel;
			XRMenu.ChosenPanel = ViewingPanel;
		}
			
		if (ViewingLecturer != LeftHandPresence.LecturerIndex)
        {				
			LeftHandPresence.LecturerIndex = ViewingLecturer;				
			XRMenu.ChosenLecturer = ViewingLecturer;				
			LeftHandPresence.CurrentLecturerFace = ThePerson.Face.material;

			if(LeftHandPresence.CurrentLesson) // if a lesson is loaded, need to update the chosen lecturer
            {
				LeftHandPresence.CurrentLesson.ChosenLecturer = LeftHandPresence.CurrentLesson.AllLecturersAllAudios[LeftHandPresence.LecturerIndex];
				if (LeftHandPresence.CurrentSubTopicCard) // if a subtopiccard is open, update the profile picture & audio file
				{
					LeftHandPresence.CurrentSubTopicCard.LecturerPic.material = ThePerson.Face.material;
					int SubTopicIndex = LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard;
					LeftHandPresence.CurrentSubTopicCard.VoiceOver = LeftHandPresence.CurrentLesson.ChosenLecturer.SubTopicVoiceOvers[SubTopicIndex];
					LeftHandPresence.CurrentSubTopicCard.MaxTime = TimeSpan.FromSeconds(LeftHandPresence.CurrentSubTopicCard.VoiceOver.clip.length);
					LeftHandPresence.CurrentSubTopicCard.UpdateVOAndSlider();
				}
			}			
		}
		Destroy(gameObject);
		XRMenu.DestroyMainMenu();
		//Destroy(LeftHandPresence.existingmenu.gameObject);
	}

	public void Close()
	{			
		Destroy(gameObject);
		//Destroy(LeftHandPresence.existingmenu.gameObject);
	}

	public void HideLecturer(int lecturer_index)
    {
		Lecturers[lecturer_index].gameObject.SetActive(false);
		ThePerson.gameObject.SetActive(false);
    }

	public void ShowLecturer(int lecturer_index, int panel_index)		
	{			 
		ViewingLecturer = lecturer_index; //need to be able to accept default case of lecturer_index = 0 for boot up
		ViewingPanel = panel_index; // need to update ViewingPanel if called from outside
		ThePerson = Lecturers[lecturer_index];
		ThePerson.gameObject.SetActive(true);
		//Debug.Log(ThePerson.name + "was set as the lecturer!");			
		ThePerson.ShowPanel(ViewingPanel);		
	}

	public void NextPerson()
    {
		//Debug.Log("Next Person was pressed");
		int prev_index = ViewingLecturer;
		ViewingLecturer++;	

		if (ViewingLecturer >= TotalLecturers)
        {
			ViewingLecturer = 0;
        }
		//Debug.Log("Prev Index: " + prev_index + "and New Index: " + ViewingLecturer);
		HideLecturer(prev_index);
		ShowLecturer(ViewingLecturer, ViewingPanel); 
    }

	public void PrevPerson()
    {
		//Debug.Log("Prev Person was pressed");
		int prev_index = ViewingLecturer;
		ViewingLecturer--;
			

		if (ViewingLecturer < 0)
        {
			ViewingLecturer = TotalLecturers-1; 
        }
		//Debug.Log("Prev Index: " + prev_index + "and New Index: " + ViewingLecturer);

		HideLecturer(prev_index);
		ShowLecturer(ViewingLecturer,ViewingPanel);
	}

	public void BioPanel()
    {
		ViewingPanel = 0;
		ThePerson.ShowPanel(ViewingPanel);
	}

	public void EducationPanel()
    {
		//PlayHover();
		ViewingPanel = 1;
		ThePerson.ShowPanel(ViewingPanel);
	}

	public void InterestsPanel()
    {
		//PlayHover();
		ViewingPanel = 2;
		ThePerson.ShowPanel(ViewingPanel);
    }
		
		
}

	
		
