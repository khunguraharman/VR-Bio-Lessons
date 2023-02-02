using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_Butt_script_eyeSP : MonoBehaviour {
	
	public Animation anim;
	
	public GameObject button_expand;
	public GameObject button_close;
	


	

public void Start () 		//------------ buton
	{
    anim.Play("idle");
    button_close.active = false;	
	}
	
	
		
public void BButt_expand2() 		//------------ buton
		{
	button_expand.active = false;
	button_close.active = true;
	anim.Play("expand");
		}
		
public void BButt_contract2() 		//------------ buton
		{
	button_expand.active = true;
	button_close.active = false;
 	anim.Play("contract");
		}
	
	

	

	
	}
