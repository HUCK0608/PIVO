using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Design_MaterialChange : MonoBehaviour
{
    public Material ChangeMaterial;
    private Material BeforeMaterial;

    public bool RotPlus = false;
    public bool RotMinus = false;
    public bool RotZero = false;
    private bool ChangeRot = false;

    void Start()
    {
        RotPlus = false;
        RotMinus = false;
        RotZero = false;
        ChangeRot = false;
    }


    void Update()
    {
        ChangeMaterialFunc();
        ChangeRot3D();
        ChangeRot3DZero();
        
    }


    void ChangeMaterialFunc()
    {
        if (BeforeMaterial != ChangeMaterial)
        {
            transform.Find("Root3D").GetComponent<MeshRenderer>().material = ChangeMaterial;
            BeforeMaterial = ChangeMaterial;
        }
    }


    void ChangeRot3D()
    {
        if (RotMinus || RotPlus)
            ChangeRot = true;

        if (ChangeRot)
        {
            float RotValue = 0;

            if (RotPlus)
                RotValue = 90;
            else if (RotMinus)
                RotValue = -90;

            transform.Find("Root3D").transform.Rotate(new Vector3(0, RotValue, 0));
            
            ChangeRot = false;
            RotMinus = false;
            RotPlus = false;
        }
    }


    void ChangeRot3DZero()
    {

        if (RotZero)
        {

            transform.Find("Root3D").transform.rotation = Quaternion.Euler (0, 0, 0);
            RotZero = false;
        }
    }

}
