using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum ECubeColor { Blue, Purple, Red }

[ExecuteInEditMode]
public class Design_CubeMaterialChange : MonoBehaviour
{
    public Material MBlue, MRed, MPurple;
    public Sprite SBlue, SRed, SPurple;

    public ECubeColor CubeColor;
    private ECubeColor BeforeColor;

    private Material CurMaterial;
    private Sprite CurSprite;

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
            {
                CurMaterial = MBlue;
                CurSprite = SBlue;
            }
            else if (CubeColor == ECubeColor.Purple)
            {
                CurMaterial = MPurple;
                CurSprite = SPurple;
            }
            else if (CubeColor == ECubeColor.Red)
            {
                CurMaterial = MRed;
                CurSprite = SRed;
            }

            transform.Find("Root2D").GetComponent<SpriteRenderer>().sprite = CurSprite;
            transform.Find("Root3D").Find("cube_bro").GetComponent<SkinnedMeshRenderer>().material = CurMaterial;
            BeforeColor = CubeColor;
        }
    }

}
