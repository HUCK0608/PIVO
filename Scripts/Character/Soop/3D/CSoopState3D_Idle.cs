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
        Vector3 startPoint = Controller3D.Manager.transform.position;

        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
        if (!transform.position.Equals(startPoint))
            Controller3D.ChangeState(ESoopState.Return);
    }
}
