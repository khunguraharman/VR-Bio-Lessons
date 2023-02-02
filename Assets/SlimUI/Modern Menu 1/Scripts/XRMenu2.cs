using UnityEngine;
using System.Collections;
using System.Timers;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace SlimUI.ModernMenu{
	public class XRMenu2 : MonoBehaviour 
	{
		Animator CameraObject;
		/*
		[Header("Panels")]
		[Tooltip("The UI Panel parenting all sub menus")]
		public GameObject mainCanvas;
		
		[Tooltip("The UI Panel that holds the CONTROLS window tab")]
		public GameObject PanelControls;
		[Tooltip("The UI Panel that holds the VIDEO window tab")]
		public GameObject PanelVideo;
		[Tooltip("The UI Panel that holds the GAME window tab")]
		public GameObject PanelGame;
		[Tooltip("The UI Panel that holds the KEY BINDINGS window tab")]
		public GameObject PanelKeyBindings;
		[Tooltip("The UI Sub-Panel under KEY BINDINGS for MOVEMENT")]
		public GameObject PanelMovement;
		[Tooltip("The UI Sub-Panel under KEY BINDINGS for COMBAT")]
		public GameObject PanelCombat;
		[Tooltip("The UI Sub-Panel under KEY BINDINGS for GENERAL")]
		public GameObject PanelGeneral;
		*/
		
		//public AudioSource tickSound = MenuController.Tick;
		/*
		[Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
		public AudioSource sliderSound;
		
		[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
		public AudioSource swooshSound;
		*/
		/*
		// campaign button sub menu
		[Header("Menus")]
		[Tooltip("The Menu for when the MAIN menu buttons")]
		public GameObject mainMenu;
		[Tooltip("THe first list of buttons")]
		*/
		/*
		public GameObject firstMenu;
		[Tooltip("The Menu for when the PLAY button is clicked")]
		public GameObject playMenu;
		[Tooltip("The Menu for when the EXIT button is clicked")]
		public GameObject exitMenu;
		[Tooltip("Optional 4th Menu")]
		public GameObject extrasMenu;
		*/

		// highlights
		[Header("Highlight Effects")]
		[Tooltip("Highlight Image for when GAME Tab is selected in Settings")] 
		public GameObject lineGame;
		[Tooltip("Highlight Image for when VIDEO Tab is selected in Settings")]
		public GameObject lineVideo;
		[Tooltip("Highlight Image for when CONTROLS Tab is selected in Settings")]
		public GameObject lineControls;
		[Tooltip("Highlight Image for when KEY BINDINGS Tab is selected in Settings")]
		public GameObject lineKeyBindings;
		[Tooltip("Highlight Image for when MOVEMENT Sub-Tab is selected in KEY BINDINGS")]
		public GameObject lineMovement;
		[Tooltip("Highlight Image for when COMBAT Sub-Tab is selected in KEY BINDINGS")]
		public GameObject lineCombat;
		[Tooltip("Highlight Image for when GENERAL Sub-Tab is selected in KEY BINDINGS")]
		public GameObject lineGeneral;

		[Header("LOADING SCREEN")]
		public GameObject loadingMenu;
		public Slider loadBar;
		public TMP_Text finishedLoadingText;

		private System.DateTime timer_start;
		private bool sounds_disabled = true;
		private System.DateTime current_time;

        void Awake()
        {
			
			//hoverSound.enabled = false;
			/*
			timer_start = System.DateTime.Now;
			*/
		}

        void Start(){
			CameraObject = transform.GetComponent<Animator>();

			/*playMenu.SetActive(false);
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			firstMenu.SetActive(true);
			*/
			//mainMenu.SetActive(true);

			
		}

        void Update()
        {
            /*
			if (sounds_disabled)
            {
				current_time = System.DateTime.Now;

				System.TimeSpan delta = current_time.Subtract(timer_start);

				if ( delta.TotalMilliseconds > 1000)
                {
					hoverSound.enabled = true;					
					sounds_disabled = false;
                }
            }
			*/
			
        }


        /*
		public void PlayCampaign(){
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			playMenu.SetActive(true);
		}
		
		public void PlayCampaignMobile(){
			exitMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			playMenu.SetActive(true);
			mainMenu.SetActive(false);
		}

		public void ReturnMenu(){
			playMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(false);
			exitMenu.SetActive(false);
			mainMenu.SetActive(true);
		}

		public void NewGame(){
			if(sceneName != ""){
				StartCoroutine(LoadAsynchronously(sceneName));
				//SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
			}
		}

		public void  DisablePlayCampaign(){
			playMenu.SetActive(false);
		}
		
		public void Position2(){
			DisablePlayCampaign();
			CameraObject.SetFloat("Animate",1);
		}
		*/
        public void Position1(){
			CameraObject.SetFloat("Animate",0);
		}
		/*
		void DisablePanels(){
			PanelControls.SetActive(false);
			PanelVideo.SetActive(false);
			PanelGame.SetActive(false);
			PanelKeyBindings.SetActive(false);

			lineGame.SetActive(false);
			lineControls.SetActive(false);
			lineVideo.SetActive(false);
			lineKeyBindings.SetActive(false);

			PanelMovement.SetActive(false);
			lineMovement.SetActive(false);
			PanelCombat.SetActive(false);
			lineCombat.SetActive(false);
			PanelGeneral.SetActive(false);
			lineGeneral.SetActive(false);
		}

		public void GamePanel(){
			DisablePanels();
			PanelGame.SetActive(true);
			lineGame.SetActive(true);
		}

		public void VideoPanel(){
			DisablePanels();
			PanelVideo.SetActive(true);
			lineVideo.SetActive(true);
		}

		public void ControlsPanel(){
			DisablePanels();
			PanelControls.SetActive(true);
			lineControls.SetActive(true);
		}

		public void KeyBindingsPanel(){
			DisablePanels();
			MovementPanel();
			PanelKeyBindings.SetActive(true);
			lineKeyBindings.SetActive(true);
		}

		public void MovementPanel(){
			DisablePanels();
			PanelKeyBindings.SetActive(true);
			PanelMovement.SetActive(true);
			lineMovement.SetActive(true);
		}

		public void CombatPanel(){
			DisablePanels();
			PanelKeyBindings.SetActive(true);
			PanelCombat.SetActive(true);
			lineCombat.SetActive(true);
		}

		public void GeneralPanel(){
			DisablePanels();
			PanelKeyBindings.SetActive(true);
			PanelGeneral.SetActive(true);
			lineGeneral.SetActive(true);
		}
		*/
		
		/*
		public void PlaySliderTick(){
			tickSound.Play();
		}
		
		public void PlaySwoosh(){
			swooshSound.Play();
		}
		*/
		/*
		// Are You Sure - Quit Panel Pop Up
		public void AreYouSure(){
			exitMenu.SetActive(true);
			if(extrasMenu) extrasMenu.SetActive(false);
			DisablePlayCampaign();
		}

		public void AreYouSureMobile(){
			exitMenu.SetActive(true);
			if(extrasMenu) extrasMenu.SetActive(false);
			mainMenu.SetActive(false);
			DisablePlayCampaign();
		}

		public void ExtrasMenu(){
			playMenu.SetActive(false);
			if(extrasMenu) extrasMenu.SetActive(true);
			exitMenu.SetActive(false);
		}
		*/
		public void QuitGame(){
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
		/*
		IEnumerator LoadAsynchronously(string sceneName){ // scene name is just the name of the current scene being loaded
			AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
			operation.allowSceneActivation = false;
			mainCanvas.SetActive(false);
			loadingMenu.SetActive(true);

			while (!operation.isDone){
				float progress = Mathf.Clamp01(operation.progress / .9f);
				loadBar.value = progress;

				if(operation.progress >= 0.9f){
					finishedLoadingText.gameObject.SetActive(true);

					if(Input.anyKeyDown){
						operation.allowSceneActivation = true;
					}
				}
				
				yield return null;
			}
		}
		*/
	}
}