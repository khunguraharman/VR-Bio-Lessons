using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System.Linq;

[RequireComponent(typeof(XRSimpleInteractable))]

public class SubTopic : MonoBehaviour
{
	public bool Get_MeshRenderers_InChildren;
	public bool Make_RendereredMaterials_Emissable;
	public GameObject Preview;
	//private GameObject PreviewInstance;
	public FullSubTopicCard Full;
	private FullSubTopicCard FullInstance; 
	private AudioSource VoiceOver = null;
	private Vector3 Preview_Translation_World = new Vector3(0, 0, 0.08f);
	private SubTopicContents m_subtopic_contents = new SubTopicContents();
	private TextMeshPro[] tmps;
	void Awake()
    {
		
		if (Get_MeshRenderers_InChildren)
        {			
			XRTintInteractableVisual thetopic = gameObject.GetComponent<XRTintInteractableVisual>();
			Renderer[] therenderers = gameObject.GetComponentsInChildren<Renderer>();
			//List<Renderer> therenderers_L = 
			thetopic.tintRenderers = therenderers.ToList();
        }

		if (Make_RendereredMaterials_Emissable)
        {
			XRTintInteractableVisual thetopic_m = gameObject.GetComponent<XRTintInteractableVisual>();
			List<Renderer> renderers = thetopic_m.tintRenderers;
			int iters = renderers.Count;

			for(int i=0; i< iters; i++)
            {
				MaterialGlobalIlluminationFlags flags = renderers[i].material.globalIlluminationFlags;
				flags &= ~MaterialGlobalIlluminationFlags.RealtimeEmissive;
				renderers[i].material.globalIlluminationFlags = flags;					
            }

		}

	}
	public void PlayTick()
    {	
		VoiceOver.Play(); 
    }

	public void FullLesson()
    {
		if (LeftHandPresence.CurrentSubTopicCard)
        {
			Destroy(LeftHandPresence.CurrentSubTopicCard.gameObject);
			//Debug.Log("The FullSubTopicCard Should have been destroyed");
        }
		//Debug.Log("The full lesson should have spawned");
		/*
		Vector3 ContentTranslate = LeftHandPresence.CurrentLesson.FullCardPositions[LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard];
		Vector3 ContentRotation = LeftHandPresence.CurrentLesson.FullCardRotations[LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard];
		Quaternion rot = new Quaternion();
		rot.eulerAngles = ContentRotation;
		Vector3 pos = transform.position + transform.TransformDirection(ContentTranslate);
		*/
		//LeftHandPresence.CurrentSubTopicCard = Instantiate(Full, pos, rot*transform.rotation, transform);
		LeftHandPresence.CurrentSubTopicCard = Instantiate(Full, LeftHandPresence.FullCardAnchor);
		LeftHandPresence.FullLessonDefaultScale = LeftHandPresence.CurrentSubTopicCard.transform.localScale;
		LeftHandPresence.CurrentSubTopicCard.transform.localScale = LeftHandPresence.FullLessonScale * LeftHandPresence.FullLessonDefaultScale;

		Transform contents = Full.transform.Find("Content");
		tmps = contents.GetComponentsInChildren<TextMeshPro>();
		string name = tmps[0].text;
		LeftHandPresence.Chosen_subtopiccard = name;
		string filename = string.Format("{0}/{1}_SubTopicContents.json",Application.dataPath, name );
		if(System.IO.File.Exists(filename))
        {
			Debug.Log("The JSON was saved, no further action needed");
        }
        else
        {
			Debug.Log("no JSON found, running backup function");
			Backup_LogSubTopicData(name);
		}
		 


	}

	public void PreviewLesson ()
	{
		//only show preview if the fullcard isn't shown
		//only show if the open fullcard is different from what is being pointed at
		//ignore above two cases if there is already a preview
		if ((!LeftHandPresence.CurrentSubTopicCard || (LeftHandPresence.CurrentLesson.CurrentPreview != LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard)) && !LeftHandPresence.CurrentPreview) 
        {
			/*
			Vector3 ContentTranslate = LeftHandPresence.CurrentLesson.PreviewPositions[LeftHandPresence.CurrentLesson.CurrentPreview];
			Vector3 ContentRotation = LeftHandPresence.CurrentLesson.PreviewRotations[LeftHandPresence.CurrentLesson.CurrentPreview];
			*/
			//Quaternion rot = new Quaternion();
			//rot.eulerAngles = ContentRotation;
			//Vector3 pos = transform.position + transform.TransformDirection(ContentTranslate);
			Debug.Log("Should have spawned the preview for " + Preview.name);
			LeftHandPresence.CurrentPreview = Instantiate(Preview, LeftHandPresence.PreviewAnchor);
			LeftHandPresence.PreviewAnchor.SetPositionAndRotation(this.transform.position + 0.15f*Vector3.up, LeftHandPresence.PreviewAnchor_Rot);
			LeftHandPresence.PreviewDefaultScale = LeftHandPresence.CurrentPreview.transform.localScale;
			LeftHandPresence.CurrentPreview.transform.localScale = LeftHandPresence.PreviewScale* LeftHandPresence.PreviewDefaultScale;
			//LeftHandPresence.CurrentPreview.transform.parent = LeftHandPresence.PreviewAnchor; 
			//LogSubTopicData();
			//PreviewInstance = Instantiate(Preview, pos, rot * transform.rotation, transform);
			//Debug.Log(Preview.name + "Should have spawned");
		}
		LogSubTopicData();
	}	

	public void DestroyPreview()
    {		 
		Destroy(LeftHandPresence.CurrentPreview);
		//Debug.Log(PreviewInstance.name + " should have been destroyed");
    }

	public void PreviewLessonVec3(Transform position)
    {
		if ((!LeftHandPresence.CurrentSubTopicCard || (LeftHandPresence.CurrentLesson.CurrentPreview != LeftHandPresence.CurrentLesson.CurrentFullSubTopicCard)) && !LeftHandPresence.CurrentPreview)
		{
			/*
			Vector3 ContentTranslate = LeftHandPresence.CurrentLesson.PreviewPositions[LeftHandPresence.CurrentLesson.CurrentPreview];
			Vector3 ContentRotation = LeftHandPresence.CurrentLesson.PreviewRotations[LeftHandPresence.CurrentLesson.CurrentPreview];
			*/
			//Quaternion rot = new Quaternion();
			//rot.eulerAngles = ContentRotation;
			//Vector3 pos = transform.position + transform.TransformDirection(ContentTranslate);
			
			Debug.Log("Should have spawned the preview for " + Preview.name);
			LeftHandPresence.CurrentPreview = Instantiate(Preview, LeftHandPresence.PreviewAnchor);
			
			LeftHandPresence.PreviewAnchor.SetPositionAndRotation(position.position, LeftHandPresence.PreviewAnchor_Rot);
			LeftHandPresence.PreviewAnchor.Translate(Preview_Translation_World, Space.World); 
			LeftHandPresence.PreviewDefaultScale = LeftHandPresence.CurrentPreview.transform.localScale;
			LeftHandPresence.CurrentPreview.transform.localScale = LeftHandPresence.PreviewScale * LeftHandPresence.PreviewDefaultScale;
			//LeftHandPresence.CurrentPreview.transform.parent = LeftHandPresence.PreviewAnchor; 
			//LogSubTopicData();
			//PreviewInstance = Instantiate(Preview, pos, rot * transform.rotation, transform);
			//Debug.Log(Preview.name + "Should have spawned");
		}
		LogSubTopicData();
	}

	public void LogSubTopicData()
    {
		string preview_text = LeftHandPresence.CurrentPreview.GetComponentInChildren<TextMeshPro>().text;
		Debug.Log("You are hovering over the" + preview_text + "subtopic");
		if(!LeftHandPresence.previews_spawned.Contains(preview_text)) //if the preview has not been viewed before, add it to the static string and save the SubTopic Class as a JSON
        {
			LeftHandPresence.previews_spawned.Add(preview_text);
			m_subtopic_contents.subtopic_name = preview_text;
			Transform contents = Full.transform.Find("Content");
			if(!contents)
            {
				Debug.Log("did not find the parent object of the TMPs");
            }
            else
            {
				TextMeshPro[] texts = contents.GetComponentsInChildren<TextMeshPro>(); // should return the name, 2-3 text points, and the time of the audio file
				Debug.Log("found" + texts.Length + "TMPs under Content");
				for (int i = 1; i < texts.Length - 1; i++)
				{
					m_subtopic_contents.subtopic_text.Add(texts[i].text);
					Debug.Log(texts[i].text);
				}

				string filename = string.Format("/{0}_SubTopicContents.json", preview_text);

				System.IO.File.WriteAllText(Application.dataPath + filename, JsonUtility.ToJson(m_subtopic_contents));
				Debug.Log("Should have saved the JSON file for" + filename);
			}		
		}
    }

	public void Backup_LogSubTopicData(string m_name)
	{
		if (!LeftHandPresence.previews_spawned.Contains(m_name)) //if the card is instantatiated, we might as well treat the preview as being viewed
		{
			LeftHandPresence.previews_spawned.Add(m_name);
		}

		if(m_subtopic_contents.subtopic_name == "")
        {
			m_subtopic_contents.subtopic_name = m_name;
		}

		if(m_subtopic_contents.subtopic_text.Count < tmps.Length-2) //if there are 4 tmps, there should be 2 string entries
        {
			for (int i = 1; i < tmps.Length - 1; i++)
			{
				m_subtopic_contents.subtopic_text.Add(tmps[i].text);
				Debug.Log(tmps[i].text);
			}
		}
		
		string filename = string.Format("/{0}_SubTopicContents.json", m_name);

		System.IO.File.WriteAllText(Application.dataPath + filename, JsonUtility.ToJson(m_subtopic_contents));
		Debug.Log("Should have made second attempt at saving JSON for" + filename);				
	}

	public class SubTopicContents
    {
		public string subtopic_name;
		public List<string> subtopic_text = new List<string>();
	}


		
}

	
		
