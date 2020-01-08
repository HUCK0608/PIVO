using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveyFinish : Design_Convey
{
    private ConveySelectorState CurMeshState;

    public CTriggerMagicStone TargetMagicStone;
    public Material OnMaterial;
    public override void BeginPlay()
    {
        base.BeginPlay();
        ConveyState.Add(EConveyDirection.Left);
    }
    public override void PushConveyPower()
    {
        base.PushConveyPower();

        if (WorldManager.CurrentWorldState == EWorldState.View2D && CheckBlockingTile())
            return;

        transform.Find("Root3D").Find("InternalPower").GetComponent<MeshRenderer>().material = OnMaterial;

        Power = true;
        TargetMagicStone.IsActive = true;
    }
}
