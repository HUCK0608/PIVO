using UnityEngine;

public class CSoopState3D_Return : CSoopState3D
{
    private void Update()
    {
        Vector3 startPoint = Controller3D.Manager.transform.position;

        Controller3D.MoveToPoint(startPoint);

        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
        else if (transform.position.Equals(startPoint))
            Controller3D.ChangeState(ESoopState.Idle);
    }
}
