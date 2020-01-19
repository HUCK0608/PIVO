using UnityEngine;

public class CSoopState2D_PutMove : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.RootObject2D.transform.parent = transform;
    }

    private void Update()
    {
        Controller2D.MoveToPoint(Controller2D.Manager.Stat.PutPoint.position);

        Vector3 putPoint = Controller2D.Manager.Stat.PutPoint.position;
        putPoint.y = transform.position.y;

        if (Vector2.Distance(transform.position, putPoint) <= 0.01f)
            Controller2D.ChangeState(ESoopState.PutEnd);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.RootObject2D.transform.parent = CPlayerManager.Instance.transform;
    }
}
