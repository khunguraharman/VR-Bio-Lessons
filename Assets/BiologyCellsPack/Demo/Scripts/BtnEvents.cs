using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEvents : MonoBehaviour
{
    public GameObject Target;
    
    public void OnBtnClick()
    {
        Camera.main.GetComponent<MouseOrbitSample>().RefreshCamera(Target.transform.Find("_cameraDirection"),1.5f,3f);
    }
}
