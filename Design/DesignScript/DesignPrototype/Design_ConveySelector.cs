using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveySelector : Design_Convey
{
    public override void PushConveyPower()
    {
        base.PushConveyPower();

        if (WorldManager.CurrentWorldState == EWorldState.View3D)
            ConveyPower(true, EConveyDirection.Right);
        else if (WorldManager.CurrentWorldState == EWorldState.View2D && bShow)
            ConveyPower(false, EConveyDirection.Right);
    }

}
