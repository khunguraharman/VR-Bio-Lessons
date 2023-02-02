using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class Settings : MonoBehaviour
	{
		
		[System.NonSerialized]
		public static int old_panel_index = 0;
		public GameObject[] Panels = new GameObject[3];
		
		

		void  Awake ()
		{
			Panels = Panels; 
			for(int i = 0; i<Panels.Length; i++)
            {				
				Panels[i].SetActive(false);
            }
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

	
		
}