using UnityEngine;

public class CSoopState2D_Return : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        Vector3 newScale = Vector3.one;
        newScale.x = Controller.Manager.Stat.IsSoopDirectionRight ? 1 : -1;
        transform.localScale = newScale;
    }

    private void Update()
    {
        Vector3 startPoint = Controller.Manager.transform.position;

        Controller.MoveToPoint(startPoint);

        if (Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Chase);
        else if (transform.position.Equals(startPoint))
            Controller.ChangeState(ESoopState.Idle);
    }
}
