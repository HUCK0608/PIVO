using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveyStart : Design_Convey
{
    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);
        if (CurState == EWorldState.View3D)
            ConveyPower(true, EConveyDirection.Right);
        else if (CurState == EWorldState.View2D)
            ConveyPower(false, EConveyDirection.Right);
    }
}
