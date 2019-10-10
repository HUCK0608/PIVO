using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum ECubeColor { Blue, Purple, Red }

[ExecuteInEditMode]
public class Design_CubeMaterialChange : MonoBehaviour
{
    public Material Blue, Red, Purple;

    public ECubeColor CubeColor;
    private ECubeColor BeforeColor;

    private Material CurMaterial;

    void Update()
    {
        if (Application.isPlaying == false)
        {
            ChangeMaterialFunc();
        }
    }
    void ChangeMaterialFunc()
    {
        if (BeforeColor != CubeColor)
        {
            if (CubeColor == ECubeColor.Blue)
                CurMaterial = Blue;
            else if (CubeColor == ECubeColor.Purple)
                CurMaterial = Purple;
            else if (CubeColor == ECubeColor.Red)
                CurMaterial = Red;

            transform.Find("Root3D").Find("cube_bro").GetComponent<SkinnedMeshRenderer>().material = CurMaterial;
            BeforeColor = CubeColor;
        }
    }
}
