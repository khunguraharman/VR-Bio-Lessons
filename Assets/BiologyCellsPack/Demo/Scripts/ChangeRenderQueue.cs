using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeRenderQueue : MonoBehaviour
{
    public int Value = 3000;
   
    void Start()
    {

        GetComponent<MeshRenderer>().material.renderQueue = Value;

    }

   
}
