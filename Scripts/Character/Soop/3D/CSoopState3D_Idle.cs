using UnityEngine;

public class CSoopState3D_Idle : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection(Controller3D.Manager.Stat.IsSoopDirectionRight ? Vector3.right : Vector3.left);
    }

    private void Update()
    {
        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
    }
}
