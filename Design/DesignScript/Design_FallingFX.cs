using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FXType { Leaf, Snow }
public class Design_FallingFX : MonoBehaviour
{
    public FXType SelectFX;
    GameObject FXSnow, FXLeaf;
    void Start()
    {
        FXSnow = transform.Find("FX_Snow").gameObject;
        FXLeaf = transform.Find("FX_Leaf").gameObject;

        if (SelectFX == FXType.Leaf)
        {
            FXSnow.SetActive(false);
            FXLeaf.SetActive(true);
        }
        else
        {
            FXSnow.SetActive(true);
            FXLeaf.SetActive(false);
        }
    }
}
