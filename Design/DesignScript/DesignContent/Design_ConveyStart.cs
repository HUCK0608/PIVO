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
    

    public override void DesignChange2D()
    {
        base.DesignChange2D();

        if (IsCanChange2D && !CheckBlockingTile())
            ConveyPower(false, EConveyDirection.Right);
    }

    public override void DesignChange3D()
    {
        base.DesignChange3D();

        ConveyPower(true, EConveyDirection.Right);
    }
}
