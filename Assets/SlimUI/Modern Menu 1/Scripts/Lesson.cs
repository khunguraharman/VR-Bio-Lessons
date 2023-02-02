using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class Lesson : MonoBehaviour
	{		
		private int PanelNumber;				
		private int old_panel_index = 0;
		public GameObject[] Panels = new GameObject[3];

		private LessonContents m_lesson_contents = new LessonContents();
		void  Awake ()
		{		
			for(int i=0; i <Panels.Length; i++)
            {
				Panels[i].SetActive(false);
            }

			TextMeshPro name = GetComponentInChildren<TextMeshPro>();
			if (!LeftHandPresence.lesson_submenus_viewed.Contains(name.text)) //only update if its the first time viewing the page
            {
				Log_LessonJSON(name);
				LeftHandPresence.lesson_submenus_viewed.Add(name.text); //add the page to the viewing history
				Debug.Log(string.Format("First time viewing {0} lesson in the submenu", name.text));
            }
            else
            {
				Debug.Log(string.Format("Already saved {0} lesson from the submenu", name.text));
            }
		}

		void Log_LessonJSON(TextMeshPro m_name)
        {
			//Get Summary Info
			TextMeshPro text_1 = m_name;
			TextMeshPro[] text_2 = Panels[0].GetComponentsInChildren<TextMeshPro>();
			TextMeshPro[] texts = new TextMeshPro[1 + text_2.Length];
			texts[0] = text_1;
			text_2.CopyTo(texts, 1);

			for (int i = 0; i < texts.Length; i++)
			{
				m_lesson_contents.lesson_summary[i] = texts[i].text;
			}

			//Get Discussion Info
			text_2 = Panels[1].GetComponentsInChildren<TextMeshPro>();

			for (int i = 0; i < text_2.Length; i++)
			{
				m_lesson_contents.lesson_discussions[i] = text_2[i].text;
			}

			string filename = string.Format("/{0}_lessonsummary.json", m_lesson_contents.lesson_summary[0]);

			System.IO.File.WriteAllText(Application.dataPath + filename, JsonUtility.ToJson(m_lesson_contents));
		}

        void Start()
        {
			
		}

        public void Update ()
		{			
			
		}
		

		public void ShowPanel(int panel_index)
        {
			if (old_panel_index != panel_index )
            {
				Panels[old_panel_index].gameObject.SetActive(false);
				Panels[panel_index].gameObject.SetActive(true);
			}

            else
            {
				Panels[panel_index].gameObject.SetActive(true); // for first bootup, make sure some panel is showing
			}

			old_panel_index = panel_index;
		}



	}

	public class LessonContents
    {
		
		public string[] lesson_summary = new string[3];
		public string[] lesson_discussions = new string[3];

		
    }

	
		
}