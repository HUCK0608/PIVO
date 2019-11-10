using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveyStart : Design_Convey
{
    public override void BeginPlay()
    {
        base.BeginPlay();

        Power = true;
        ConveyPower(true, EConveyDirection.Right);
    }
    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);

        if (CurState == EWorldState.View3D)
            ConveyPower(true, EConveyDirection.Right);
        else if (CurState == EWorldState.View2D && bShow)
            ConveyPower(false, EConveyDirection.Right);
    }
}
