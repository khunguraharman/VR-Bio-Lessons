
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using SlimUI.ModernMenu;
using System;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;

public class LeftHandPresence : MonoBehaviour
{
    
    public static string build_info = "0_0_1";
    List<UnityEngine.XR.InputDevice> AllDevices = new List<UnityEngine.XR.InputDevice>();

    public GameObject[] ControllerPrefabs = new GameObject[4];
    public static InputDevice hand_controller { get; private set; }
    bool assign_controller_models = true; // will need to assign controllers on startup    
    bool LHMenuButtonPressed;

    [Header("Main Menus")]
    public XRMenu MainMenuPrefab;    
    // Default indices for panel and lecturer on startup
    public static int LecturerIndex = 0;
    public static int PanelIndex = 0;
    public static int LessonIndex = 0;

    private Vector3 menuOffset = new Vector3(0, 0, 0.15f);
    public static XRMenu existingmenu;

    //[Header("Lesson Models")]    
    //public LessonModel[] LessonModels = new LessonModel[2];
    //public static LessonModel[] SpawnableModels;
    [Header("Spawning Notes")]
    public static LessonModel CurrentLesson=null; //{ get; private set; }
    public Material DefaultLecturerFace;
    public static Material CurrentLecturerFace;    
    public static FullSubTopicCard CurrentSubTopicCard;
    //public static GameObject CurrentPreview;
    public static Vector3 spawn_point = new Vector3(0.75f, 1.142f, 2.427f);
    public static float spawn_y;
    public static Vector2 XZ_Boundary_Offset = new Vector2(1,1);

    //public static bool lecturer_chosen = false;
    public static GameObject CurrentPreview;
    public GameObject PreviewAnchorGameObject;
    public static Transform PreviewAnchor;
    public static Quaternion PreviewAnchor_Rot;
    public GameObject FullCardGameObject;
    public static Transform FullCardAnchor;
    public GameObject OtherComponentsObject;
    public static Transform OtherComponentsAnchor;

    [Header("Audio Mixer Groups")]
    public AudioMixerGroup AMG_UI_object;
    public AudioMixerGroup AMG_VO_object;
    public static AudioMixerGroup AMG_UI;
    public static AudioMixerGroup AMG_VO;

    //Lesson Scale
    public static float LessonModelScale = 1f;
    public static Vector3 LessonModelDefaultScale;
    public static float PreviewScale = 1f;
    public static Vector3 PreviewDefaultScale;
    public static float FullLessonScale = 1f;
    public static Vector3 FullLessonDefaultScale;

    public static float AnimationSpeed = 1f;   

    public static string Chosen_Lesson_Model;
    public static string Chosen_subtopiccard;
    /*
    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {
        build_info = "0_0_1";

        LecturerIndex = 0;
        PanelIndex = 0;
        LessonIndex = 0;

        existingmenu = null;

        CurrentLesson = null;
        CurrentLecturerFace = null;
        CurrentSubTopicCard = null;
        spawn_point = new Vector3(0.75f, 1.142f, 2.427f);
        spawn_y = 0;
        XZ_Boundary_Offset = new Vector2(1, 1);

        CurrentPreview = null;
        PreviewAnchor = null;
        PreviewAnchor_Rot = Quaternion.identity;
        FullCardAnchor = null;
        OtherComponentsAnchor = null;
        
        AMG_UI = null;  
        AMG_VO = null;

        LessonModelScale = 1f;
        LessonModelDefaultScale = Vector3.one;
        PreviewScale = 1f;
        PreviewDefaultScale = Vector3.one;
        FullLessonDefaultScale = Vector3.one;
        FullLessonScale = 1f;
        Chosen_Lesson_Model = null;
        Chosen_subtopiccard = null;
    }
    */
    private void Awake()
    {
        AMG_UI = AMG_UI_object;
        AMG_VO = AMG_VO_object;
        OtherComponentsAnchor = OtherComponentsObject.transform; 
        FullCardAnchor = FullCardGameObject.transform; 
        PreviewAnchor = PreviewAnchorGameObject.transform;
        PreviewAnchor_Rot = PreviewAnchor.rotation; // want to keep this constant regardless of the models rotation/orientation
        Application.targetFrameRate = 72;
        CurrentLecturerFace = DefaultLecturerFace;
        //SpawnableModels = LessonModels;
        XRRayInteractor LeftHandInteractable = gameObject.GetComponent<XRRayInteractor>();
        Debug.Log("The LH controller interaction mask is " + LeftHandInteractable.interactionLayers); // does this output a string that is the name or an int that is the value property?
    }    

    // Update is called once per frame
    void FixedUpdate()
    {

        CheckDevices_AssignModels();

        Spawn_XRMenu();        
        
    }

    void CheckDevices_AssignModels()
    {
        UnityEngine.XR.InputDevices.GetDevices(AllDevices);

        if (AllDevices.Count < 3) //Some device was disconnected, need to look for and reassign devices
        {
            //Debug.Log("Only " + AllDevices.Count + " Devices. Something Disconnected, Looking for devices...");
            assign_controller_models = true;
        }

        if (AllDevices.Count >= 3 && assign_controller_models == true)
        {
            List<UnityEngine.XR.InputDevice> devices = new List<UnityEngine.XR.InputDevice>();

            UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, devices);

            hand_controller = devices[0];

            //Debug.Log("Just Discovered:" + hand_controller.name);

            GameObject controller_model = System.Array.Find(ControllerPrefabs, controller => controller.name == hand_controller.name);
            if (controller_model)
            {
                Instantiate(controller_model, transform);
            }

            assign_controller_models = false;
        }

    }

    void Spawn_XRMenu()
    {
        if (hand_controller.TryGetFeatureValue(UnityEngine.XR.CommonUsages.menuButton, out LHMenuButtonPressed) && LHMenuButtonPressed)
        {
            Enable_Lesson_Contents(false);

            if (existingmenu)
            {
                //Debug.Log("Existing XRMenu was detected");
                Destroy(existingmenu.gameObject);
            }

            Vector3 pos = transform.position;
            pos += transform.TransformDirection(menuOffset);
            Vector3 rot_ea = transform.rotation.eulerAngles;
            rot_ea[2] = 0;
            Quaternion rot = new Quaternion();
            rot.eulerAngles = rot_ea;
            //Debug.Log("Rotation:" + rot_ea);
            existingmenu = Instantiate(MainMenuPrefab, pos, rot, transform.parent); // want the parent to be Camera Offset
             /*                                                                        
            existingmenu.InheritLecturerandPanelIndex(LecturerIndex, PanelIndex);
            existingmenu.InheritLesson(LessonIndex);
             */
        }
    }

    public static void Enable_Lesson_Contents(bool new_state)
    {
        if (CurrentLesson)
        {            
            CurrentLesson.gameObject.SetActive(new_state);
            
            if (CurrentSubTopicCard && CurrentSubTopicCard.isActiveAndEnabled != new_state)
            {
                CurrentSubTopicCard.gameObject.SetActive(new_state);
                try
                {
                    if (CurrentSubTopicCard.VoiceOver.isPlaying)
                    {
                        CurrentSubTopicCard.PlayPauseVoiceOver();
                    }
                }
                catch (NullReferenceException e)
                {
                    Debug.Log("The Voiceover doesn't exist");
                }

                finally
                {

                }
                Debug.Log("The Voiceover code block succeeded");
                
            }

            if (CurrentPreview)
            {
                CurrentPreview.gameObject.SetActive(new_state);
            }
            if (OtherComponentsAnchor)
            {
                OtherComponentsAnchor.gameObject.SetActive(new_state);
            }
        }
    }
    
    


}
