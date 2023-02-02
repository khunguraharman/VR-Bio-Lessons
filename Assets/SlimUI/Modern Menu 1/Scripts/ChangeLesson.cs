using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class ChangeLesson : MonoBehaviour
	{
		
		private Lesson TheLesson = null;
		private int PanelIndex = 0; 

		public int LessonIndex { get; private set; } 		
		private int TotalLessons;

		public Lesson[] Lessons = new Lesson[1]; 
		//public AudioSource HoverSound = null;		

		public LessonModel[] LessonModels = new LessonModel[1];

		public Vector3[] SpawnPoints = new Vector3[1];

		public Vector3[] SpawnEulerAngles = new Vector3[1];

		[Header("Metadata")]
		public RectTransform BoundingRectangle;


		void  Awake ()
		{			
			for(int i=0; i <Lessons.Length; i++)
            {
				Lessons[i].gameObject.SetActive(false);
            }
			
		}

        void Start()
        {
			TotalLessons = Lessons.Length;
			//Debug.Log(TotalLessons + "is the total number of lecturers!");			
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
			Quaternion rot = new Quaternion();
			rot.eulerAngles = SpawnEulerAngles[LessonIndex];
			LeftHandPresence.spawn_point = SpawnPoints[LessonIndex];
			
			if (!LeftHandPresence.CurrentLesson) // if there is no model, we must spawn one
            {
				XRMenu.ChosenLesson = LessonIndex;
				LeftHandPresence.LessonIndex = LessonIndex;								

				LeftHandPresence.CurrentLesson = Instantiate(LessonModels[LessonIndex], SpawnPoints[LessonIndex], rot);
				LeftHandPresence.LessonModelDefaultScale = LeftHandPresence.CurrentLesson.transform.localScale; //if a new model is spawned, ensure the default scale is assigned				
			}
			
			else if (LessonIndex != LeftHandPresence.LessonIndex && LeftHandPresence.CurrentLesson) // only run if the model has been changed
			{
				Destroy(LeftHandPresence.CurrentLesson.gameObject);

				if (LeftHandPresence.CurrentSubTopicCard)
				{
					Destroy(LeftHandPresence.CurrentSubTopicCard);
				}

				if (LeftHandPresence.CurrentPreview)
				{
					Destroy(LeftHandPresence.CurrentPreview);
				}
				
				if (LeftHandPresence.OtherComponentsAnchor.GetChild(0).gameObject)
                {
					Destroy(LeftHandPresence.OtherComponentsAnchor.GetChild(0).gameObject);
                }

				XRMenu.ChosenLesson = LessonIndex;
				LeftHandPresence.LessonIndex = LessonIndex;
				LeftHandPresence.CurrentLesson = Instantiate(LessonModels[LessonIndex], SpawnPoints[LessonIndex], rot);
				LeftHandPresence.LessonModelDefaultScale = LeftHandPresence.CurrentLesson.transform.localScale; //if a new model is spawned, ensure the default scale is assigned		
																												
																												

				
			}

			LeftHandPresence.Chosen_Lesson_Model = this.Lessons[LessonIndex].GetComponentInChildren<TextMeshPro>().text; //get the name, make it accessible for the data pipeline

			if (!LeftHandPresence.lesson_submenus_viewed.Contains(LeftHandPresence.Chosen_Lesson_Model)) //only add to history if it's the first time
			{
				LeftHandPresence.lesson_models_spawned.Add(LeftHandPresence.Chosen_Lesson_Model); //update the lesson model history as well
			}

			Destroy(gameObject);
			XRMenu.DestroyMainMenu();
			//Destroy(LeftHandPresence.existingmenu.gameObject);
		}

		public void Close()
		{
			Destroy(gameObject);
		}

		public void HideLesson(int lesson_index)
        {
			Lessons[lesson_index].gameObject.SetActive(false);
			TheLesson.gameObject.SetActive(false);
        }

		public void ShowLesson(int lesson_index)		
		{			 
			LessonIndex = lesson_index; //need to be able to accept default case of lecturer_index = 0 for boot up
			TheLesson = Lessons[LessonIndex];
			TheLesson.gameObject.SetActive(true);
			//Debug.Log(TheLesson.name + "was set as the lesson!");
			TheLesson.ShowPanel(PanelIndex);		
		}

		public void NextLesson()
        {
			//Debug.Log("Next Person was pressed");
			int prev_index = LessonIndex;
			LessonIndex++;		

			if (LessonIndex >= TotalLessons)
            {
				LessonIndex = 0;
            }
			//Debug.Log("Prev Index: " + prev_index + "and New Index: " + LessonIndex);
			HideLesson(prev_index);
			ShowLesson(LessonIndex); 
        }

		public void PrevLesson()
        {
			//Debug.Log("Prev Person was pressed");
			int prev_index = LessonIndex;
			LessonIndex--;			

			if (LessonIndex < 0)
            {
				LessonIndex = TotalLessons-1; 
            }
			//Debug.Log("Prev Index: " + prev_index + "and New Index: " + LessonIndex);

			HideLesson(prev_index);
			ShowLesson(LessonIndex);
		}

		public void SummaryPanel()
        {			
			PanelIndex = 0;
			TheLesson.ShowPanel(PanelIndex);
		}

		public void QuizPanel()
        {			
			PanelIndex = 1;
			TheLesson.ShowPanel(PanelIndex);
		}

		public void StatsPanel()
        {			
			PanelIndex = 2;
			TheLesson.ShowPanel(PanelIndex);
        }		
		
	}

	
		
}