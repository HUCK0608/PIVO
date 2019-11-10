using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveyFinish : Design_Convey
{
    public GameObject TargetMagicStone;
    public override void PushConveyPower()
    {
        base.PushConveyPower();

        Debug.Log("타겟 매직 스톤을 활성화 시킨다.");
    }
}
