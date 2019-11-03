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
    public GameObject EBlue, ERed, EPurple;

    public ECubeColor CubeColor;
    private ECubeColor BeforeColor;

    private Material CurMaterial;
    private Sprite CurSprite;
    private GameObject CurEffect;

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
                CurEffect = EBlue;
            }
            else if (CubeColor == ECubeColor.Purple)
            {
                CurMaterial = MPurple;
                CurSprite = SPurple;
                CurEffect = EPurple;
            }
            else if (CubeColor == ECubeColor.Red)
            {
                CurMaterial = MRed;
                CurSprite = SRed;
                CurEffect = ERed;
            }

            for (int i=0; i<transform.Find("ViewChange_Effect").childCount; i++)
            {
                DestroyImmediate(transform.Find("ViewChange_Effect").GetChild(i).gameObject);
            }

            GameObject BoomInstance = Instantiate(CurEffect, transform.position, transform.rotation);
            BoomInstance.transform.parent = transform.Find("ViewChange_Effect");
            //BoomInstance.SetActive(false);

            transform.Find("Root2D").GetComponent<SpriteRenderer>().sprite = CurSprite;
            transform.Find("Root3D").Find("cube_bro").GetComponent<SkinnedMeshRenderer>().material = CurMaterial;
            BeforeColor = CubeColor;
        }
    }

}
