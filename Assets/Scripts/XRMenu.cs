using UnityEngine;
using System.Collections;
using System.Timers;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class XRMenu : MonoBehaviour 
{
	public static int ChosenLesson;
	public static int ChosenPanel;  //default value will be 0
	public static int ChosenLecturer;  // default value is 0											   	

	[Header("Sub Menus")]
	[Tooltip("Prefabs Holding SubMenus")]
	public ChangeLesson ChooseLessonMenuPrefab;
	private ChangeLesson ChooseLessonSubMenu; 
	public ChangeLecturer ChangeLecturerMenuPrefab;		
	private ChangeLecturer ChangeLecturerSubMenu;
	public UserSettings ConfigureSettingsPrefab;
	private UserSettings ConfigureSettingsMenu;

		 
	private Vector3 SubMenuOffset = new Vector3(1.25f, 0.1f, 0.75f);
	private Vector3 SubMenuRotation = new Vector3(0, 20, 0);
	[Header("SFX")]
	[Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
	public AudioSource Hoversound; 
	public static AudioSource hoverSound;

	public float Timer = 0.0f;
	private float Max_Unresponsive_Time = 7.0f;
	private float delta;

	[Header("Metadata")]
	public RectTransform BoundingRectangle;
	/*
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
	*/

	/*
	[RuntimeInitializeOnLoadMethod]
	static void ResetStatics()
	{
        ChosenLesson = LeftHandPresence.LessonIndex;
        ChosenPanel = LeftHandPresence.PanelIndex;
        ChosenLecturer = LeftHandPresence.LecturerIndex;        
    }
	*/

    void Awake()
    {
		hoverSound = Hoversound; 
		ChosenLesson = LeftHandPresence.LessonIndex;
		ChosenPanel = LeftHandPresence.PanelIndex;
		ChosenLecturer = LeftHandPresence.LecturerIndex;
		delta = Time.fixedDeltaTime;
	}


    void FixedUpdate()
    {
		if (!ConfigureSettingsMenu && !ChangeLecturerSubMenu && !ChooseLessonSubMenu) // don't assume they are unresponsive if a submenu is open
		{
			Timer += delta;
		}

		if(Timer > Max_Unresponsive_Time)
        {
			DestroyMainMenu();
        }
	}      
		
	public static void PlayHover()
	{
		if (hoverSound != null)
        {
			hoverSound.Play();
		}		
	}

	public void SpawnChooseLecturer()
    {			 
		if (ChangeLecturerSubMenu)
        {
			ChangeLecturerSubMenu.Close();
		}

		if (ChooseLessonSubMenu)
		{
			ChooseLessonSubMenu.Close();
		}

		if (ConfigureSettingsMenu)
		{
			ConfigureSettingsMenu.Close();
		}

		Vector3 pos = transform.position + transform.TransformDirection(SubMenuOffset);
		ChangeLecturerSubMenu = Instantiate(ChangeLecturerMenuPrefab, pos, transform.rotation, transform);

		ChangeLecturerSubMenu.transform.Rotate(SubMenuRotation, Space.Self);

		ChangeLecturerSubMenu.ShowLecturer(ChosenLecturer, ChosenPanel);
		/*
		if (LeftHandPresence.CurrentLesson)
		{
			LeftHandPresence.CurrentLesson.gameObject.SetActive(false);
		}
		if (LeftHandPresence.CurrentSubTopicCard)
		{
			LeftHandPresence.CurrentSubTopicCard.gameObject.SetActive(false);
		}

		if (LeftHandPresence.CurrentPreview)
		{
			LeftHandPresence.CurrentPreview.gameObject.SetActive(false);
		}
		if (LeftHandPresence.OtherComponentsAnchor)
		{
			LeftHandPresence.OtherComponentsAnchor.gameObject.SetActive(false);
		}
		*/
		LeftHandPresence.Enable_Lesson_Contents(false);
	}

	public void SpawnChooseLesson()
	{
			
		if (ChangeLecturerSubMenu)
		{				
			ChangeLecturerSubMenu.Close();				
		}

		if (ChooseLessonSubMenu)
		{
			ChooseLessonSubMenu.Close();
		}

		if (ConfigureSettingsMenu)
		{
			ConfigureSettingsMenu.Close();
		}

		Vector3 pos = transform.position + transform.TransformDirection(SubMenuOffset);
		ChooseLessonSubMenu = Instantiate(ChooseLessonMenuPrefab, pos, transform.rotation, transform);

		ChooseLessonSubMenu.transform.Rotate(SubMenuRotation, Space.Self);

		ChooseLessonSubMenu.ShowLesson(ChosenLesson);
		/*
		if (LeftHandPresence.CurrentLesson)
		{
			LeftHandPresence.CurrentLesson.gameObject.SetActive(false);
		}
		if (LeftHandPresence.CurrentSubTopicCard)
		{
			LeftHandPresence.CurrentSubTopicCard.gameObject.SetActive(false);
		}
			
		if (LeftHandPresence.CurrentPreview)
		{
			LeftHandPresence.CurrentPreview.gameObject.SetActive(false);
		}
		if (LeftHandPresence.OtherComponentsAnchor)
		{
			LeftHandPresence.OtherComponentsAnchor.gameObject.SetActive(false);
		}
		*/
		LeftHandPresence.Enable_Lesson_Contents(false);
	}

	public void SpawnSettingsMenu()
	{
		if (ChangeLecturerSubMenu)
		{
			ChangeLecturerSubMenu.Close();
		}

		if (ChooseLessonSubMenu)
		{
			ChooseLessonSubMenu.Close();
		}

		if (ConfigureSettingsMenu)
        {
			ConfigureSettingsMenu.Close();
		}

		Vector3 pos = transform.position + transform.TransformDirection(SubMenuOffset);
		ConfigureSettingsMenu = Instantiate(ConfigureSettingsPrefab, pos, transform.rotation, transform);

		ConfigureSettingsMenu.transform.Rotate(SubMenuRotation, Space.Self);
		/*
		if (LeftHandPresence.CurrentLesson)
		{
			LeftHandPresence.CurrentLesson.gameObject.SetActive(true);
		}
		if (LeftHandPresence.CurrentSubTopicCard)
		{
			LeftHandPresence.CurrentSubTopicCard.gameObject.SetActive(true);
		}
		if (LeftHandPresence.CurrentPreview)
		{
			LeftHandPresence.CurrentPreview.gameObject.SetActive(true);
		}
		if (LeftHandPresence.OtherComponentsAnchor)
		{
			LeftHandPresence.OtherComponentsAnchor.gameObject.SetActive(true);
		}
		*/
		LeftHandPresence.Enable_Lesson_Contents(true);
	}

	public static void DestroyMainMenu()
    {
        /*
		if(LeftHandPresence.CurrentLesson)
        {
			LeftHandPresence.CurrentLesson.gameObject.SetActive(true);
        }
		if(LeftHandPresence.CurrentSubTopicCard)
        {
			LeftHandPresence.CurrentSubTopicCard.gameObject.SetActive(true);
        }
		if(LeftHandPresence.CurrentPreview)
        {
			LeftHandPresence.CurrentPreview.gameObject.SetActive(true);
        }
		if(LeftHandPresence.OtherComponentsAnchor)
        {
			LeftHandPresence.OtherComponentsAnchor.gameObject.SetActive(true);
        }
		*/
        Destroy(LeftHandPresence.existingmenu.gameObject);
        Debug.Log("Hand menu should have been destroyed");
        LeftHandPresence.Enable_Lesson_Contents(true);
		
		
		
    }
		
	public void QuitGame()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	public void Reset_Timer() //reset timer if user hover overs anything
    {
			
		Timer = 0.0f;
						
    }		


}
