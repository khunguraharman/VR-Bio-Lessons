using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


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
	}

	public void Log_LessonJSON(TextMeshPro m_name)
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

		string filename = string.Format("/{0}_lessonsummary_{1}.json", LeftHandPresence.build_info, m_lesson_contents.lesson_summary[0]);

		System.IO.File.WriteAllText(Application.dataPath + filename, JsonUtility.ToJson(m_lesson_contents));
	}

	public TextMeshPro Get_Lesson_Name()
    {
		TextMeshPro name = GetComponentInChildren<TextMeshPro>();
		
		return name;
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

	
		
