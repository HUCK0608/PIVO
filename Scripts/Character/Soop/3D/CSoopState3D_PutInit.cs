using UnityEngine;

public class CSoopState3D_PutInit : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PutInit);
        Controller.ChangeState(ESoopState.PutIdle);
    }
}
