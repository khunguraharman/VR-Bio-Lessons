using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class Content : MonoBehaviour
{
	public GameObject Preview;
	private GameObject PreviewInstance;
	public GameObject Full;
	private GameObject FullInstance; 
	public AudioSource VoiceOver = null;
	
	public void PlayTick()
    {	
		VoiceOver.Play(); 
    }

	public void FullLesson()
    {
		Debug.Log("The full lesson should have spawned");
		Vector3 ContentTranslate = new Vector3(0f, 0.1f, -0.75f);
		Vector3 ContentRotation = new Vector3(0, 180f, 0);
		Quaternion rot = new Quaternion();
		rot.eulerAngles = ContentRotation;
		Vector3 pos = transform.position + transform.TransformDirection(ContentTranslate);
		FullInstance = Instantiate(Full, pos, rot*transform.rotation, transform);
		//FullInstance.transform.Rotate(ContentRotation, Space.Self);
	}

	public void PreviewLesson ()
	{
		if (!FullInstance)
        {
			Vector3 ContentTranslate = new Vector3(0f, 0.1f, -0.55f);
			Vector3 ContentRotation = new Vector3(0, 180f, 0);
			Quaternion rot = new Quaternion();
			rot.eulerAngles = ContentRotation;
			Vector3 pos = transform.position + transform.TransformDirection(ContentTranslate);
			PreviewInstance = Instantiate(Preview, pos, rot * transform.rotation, transform);
		}
		
		
	}

	public void DestroyPreview()
    {		 
		Destroy(PreviewInstance);
    }

		
}

	
		
