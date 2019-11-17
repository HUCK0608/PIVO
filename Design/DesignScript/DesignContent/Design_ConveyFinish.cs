using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveyFinish : Design_Convey
{
    private ConveySelectorState CurMeshState;

    public GameObject TargetMagicStone;
    public Material OnMaterial;
    public override void BeginPlay()
    {
        base.BeginPlay();
    }
    public override void PushConveyPower()
    {
        base.PushConveyPower();

        Power = true;

        transform.Find("Root3D").Find("InternalPower").GetComponent<MeshRenderer>().material = OnMaterial;

        Debug.Log("타겟 매직 스톤을 활성화 시킨다.");
    }
}
