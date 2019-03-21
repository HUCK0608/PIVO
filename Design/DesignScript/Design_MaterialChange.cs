using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Design_MaterialChange : MonoBehaviour
{
    public Material ChangeMaterial;
    private Material BeforeMaterial;

    void Update()
    {
        if (BeforeMaterial != ChangeMaterial)
        {
            transform.Find("Root3D").GetComponent<MeshRenderer>().material = ChangeMaterial;
            BeforeMaterial = ChangeMaterial;
        }
    }
}
