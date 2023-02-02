using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using System.Linq;
using System;
using UnityEngine.XR.Interaction.Toolkit;


public class LessonModel : MonoBehaviour
{
	public int LecturerIndex;
	public SubTopic[] Components = new SubTopic[4];
	public XRSimpleInteractable[] Components_Interactable { get; private set; }
	private int KeptDefault;
	public LecturerVoiceOver[] AllLecturersAllAudios = new LecturerVoiceOver[2];
	//private string[] LecturerNames = new string[] { "Manj", "Arsh" };


	//private InteractionLayerMask RejectAllLayer;
	//public InteractionLayerMask DefaultInteractionLayer;
	[NonSerialized]
	public LecturerVoiceOver ChosenLecturer;
	//public static Material LecturerFace; 
	[NonSerialized]
	public int CurrentFullSubTopicCard;
	public int CurrentPreview;

	public GameObject OtherComponents;

	public MeshRenderer[] LargestMeshes;

	public Bounds Lesson_Model_Bounds { get; private set; }

	void  Awake ()
	{
		
		ChosenLecturer = AllLecturersAllAudios[LeftHandPresence.LecturerIndex];
		if(OtherComponents != null)
        {
			OtherComponents.transform.parent = LeftHandPresence.OtherComponentsAnchor;
		}

		UpdateModelBounds();
		
	}

    void Start()
	{
		Components_Interactable = new XRSimpleInteractable[Components.Length];
		for(int i=0; i< Components_Interactable.Length; i++)
        {
			Components_Interactable[i] = Components[i].gameObject.GetComponent<XRSimpleInteractable>();
        }
	}

    private void Update ()
	{
		
	}	
	
	public void AssignFullSubTopicIndex(SubTopic thesubtopic) // value is updated when the subtopic is slected/fullcard is instantiated
    {
		CurrentFullSubTopicCard = Array.IndexOf(LeftHandPresence.CurrentLesson.Components, thesubtopic);
    }

	public void AssignPreviewIndex(SubTopic thesubtopic) // value is updated when the subtopic is slected/fullcard is instantiated
	{
		CurrentPreview = Array.IndexOf(LeftHandPresence.CurrentLesson.Components, thesubtopic);
	}	

	public static void AssignNothingMask(XRSimpleInteractable filter_these_out)
    {
		/*
		for (int i=0; i<filter_these_out.Length; i++)
        {
			filter_these_out[i].interactionLayers = InteractionLayerMask.GetMask("Nothing");
		}
		*/
		filter_these_out.interactionLayers = InteractionLayerMask.GetMask("Nothing");
	}

	public static void AssignDefaultMask(XRSimpleInteractable filter_these_out)
	{
		//string write_this = string.Format("The interactable's name is {0} and its current mask is {1}", filter_these_out.name, filter_these_out.interactionLayers);
		//Debug.Log(write_this);
		/*
		for (int i = 0; i < filter_these_out.Length; i++)
		{
			filter_these_out[i].interactionLayers = InteractionLayerMask.GetMask("Default");
		}
		*/
		filter_these_out.interactionLayers = InteractionLayerMask.GetMask("Default");
		//write_this = string.Format("interactable {0} mask is now {1}", filter_these_out.name, filter_these_out.interactionLayers);
	}
	
	public void AssignNothingMaskForall(SubTopic KeepDefault)
    {
		KeptDefault = Array.IndexOf(Components, KeepDefault);
		for(int i =0; i< Components.Length; i++)
        {
			if(i!=KeptDefault)
            {
				Components_Interactable[i].interactionLayers = InteractionLayerMask.GetMask("Nothing");
            }
        }
    }

	public void ReAssignDefaultMaskForAll()
    {
		for (int i = 0; i < Components.Length; i++)
		{
			if (i != KeptDefault)
			{
				Components_Interactable[i].interactionLayers = InteractionLayerMask.GetMask("Default");
			}
		}
	}	

	public void UpdateModelBounds()
    {
		Lesson_Model_Bounds = LargestMeshes[0].GetComponent<Renderer>().bounds;

		for (int i=1; i<LargestMeshes.Length; i++)
        {
			Bounds m_bounds = LargestMeshes[i].GetComponent<Renderer>().bounds;
			Lesson_Model_Bounds.Encapsulate(m_bounds);
        }
    }
}

	
		
