using UnityEngine;

public class CSoopState3D_Return : CSoopState3D
{
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
