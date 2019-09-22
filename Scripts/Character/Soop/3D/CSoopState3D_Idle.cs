using UnityEngine;

public class CSoopState3D_Idle : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller.LookDirection(Controller.Manager.Stat.IsSoopDirectionRight ? Vector3.right : Vector3.left);
    }

    private void Update()
    {
        if (Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Chase);
    }
}
