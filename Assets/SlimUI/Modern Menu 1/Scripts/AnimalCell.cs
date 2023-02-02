using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;


public class AnimalCell : MonoBehaviour
{



	public List<Content> Components = new List<Content>();	
	
	private bool sounds_disabled = true;

	void  Awake ()
	{
		sounds_disabled = true;
					
	}

    void Start()
	{
		
		sounds_disabled = true;
	}

    public void Update ()
	{
			
	}
	

	public void Confirm()
    {
		Destroy(gameObject); 
    }

	

	
		
		
}

	
		
