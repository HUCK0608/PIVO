using UnityEngine;

public class CSoopState3D_PutIdle : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.transform.parent = transform;
    }

    private void Update()
    {
        Controller.MoveToPoint(Controller.Manager.Stat.PutPoint.position);

        if (transform.position.Equals(Controller.Manager.Stat.PutPoint.position))
            Controller.ChangeState(ESoopState.PutEnd);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.transform.parent = null;
    }
}
