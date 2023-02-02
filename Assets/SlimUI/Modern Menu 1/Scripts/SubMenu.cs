using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

namespace SlimUI.ModernMenu{
	public class SubMenu : MonoBehaviour
	{
		[System.NonSerialized]
		public int PanelNumber; 

		public List<Lecturer> Lecturers = new List<Lecturer>(); 
		public AudioSource TickSound = null;
		private bool sounds_disabled = true;

		void  Awake ()
		{
			sounds_disabled = true;
			TickSound.mute = true;
			
		}

        void Start()
        {
			TickSound.mute = true; 
			sounds_disabled = true;
		}

        public void Update ()
		{
			
		}
		public void PlayTick()
        {
			if (sounds_disabled)
            {
				sounds_disabled = true;
				TickSound.mute = false;
            }

			TickSound.Play(); 
        }

		public void Confirm()
        {
			Destroy(gameObject); 
        }

		public void ShowDefaultPanel ()
		{
			 

		}

		
	}

	
		
}