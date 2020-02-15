using UnityEngine;

public class CSoopState3D_Return : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.RemoveDetectionSoop(Controller3D.Manager.gameObject);
    }

    private void Update()
    {
        Vector3 startPoint = Controller3D.Manager.transform.position;

        Controller3D.MoveToPoint(startPoint);

        if (!Controller3D.Animator.GetInteger("CurrentState").Equals(3))
            Controller3D.ChangeAnimation();

        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
        else if (transform.position.Equals(startPoint))
            Controller3D.ChangeState(ESoopState.Idle);
    }
}
