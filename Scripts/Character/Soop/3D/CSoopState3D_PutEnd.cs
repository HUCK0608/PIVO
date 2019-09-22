using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopState3D_PutEnd : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PutEnd);
        Controller.ChangeState(ESoopState.Return);
    }
}
