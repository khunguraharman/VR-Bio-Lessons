using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class UI_Butt_script_eyeSM : MonoBehaviour {
	
	public Animation anim;

	

public void Start () 		//------------ buton
	{
    anim.Play("idle");	
	}
	
public void BButt_side_animation() 		//------------ buton
	{
    anim.Play("sides");	
	}
	
	
public void BButt_updown_animation() 		//------------ buton
	{
    anim.Play("updown");	
	}
	
		


	

	
	}
