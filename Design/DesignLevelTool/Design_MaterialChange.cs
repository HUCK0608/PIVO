using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Design_MaterialChange : MonoBehaviour
{
    public Material ChangeMaterial;
    private Material BeforeMaterial;

    public bool RotPlus, RotMinus;
    private bool ChangeRot;

    void Update()
    {
        ChangeMaterialFunc();
        ChangeRot3D();
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
}
