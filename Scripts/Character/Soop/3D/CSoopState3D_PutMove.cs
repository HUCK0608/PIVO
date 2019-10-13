using UnityEngine;

public class CSoopState3D_PutMove : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.transform.parent = transform;
    }

    private void Update()
    {
        Controller3D.MoveToPoint(Controller3D.Manager.Stat.PutPoint.position);

        if (transform.position.Equals(Controller3D.Manager.Stat.PutPoint.position))
            Controller3D.ChangeState(ESoopState.PutEnd);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.transform.parent = null;
    }
}
