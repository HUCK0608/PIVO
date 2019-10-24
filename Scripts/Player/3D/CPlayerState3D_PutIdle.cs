using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutIdle : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Stat.IsPut = true;
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.Stat.IsPut = false;
    }
}
