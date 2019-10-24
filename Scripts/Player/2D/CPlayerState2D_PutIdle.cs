using UnityEngine;

public class CPlayerState2D_PutIdle : CPlayerState2D
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
