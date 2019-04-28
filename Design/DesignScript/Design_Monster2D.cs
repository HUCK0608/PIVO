using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Monster2D : MonoBehaviour
{
    GameObject Corgi, Monster3D;
    void Start()
    {
        InitializeValue();
        //Monster3D.SetActive(false);
    }

    void Update()
    {
        
    }

    void InitializeValue()
    {
        Monster3D = transform.parent.transform.Find("3D").gameObject;
    }
}
