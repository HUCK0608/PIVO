using UnityEngine;

public class CSoopState2D_Chase : CSoopState2D
{
    private void Update()
    {
        Controller.MoveToPoint(CPlayerManager.Instance.RootObject2D.transform.position);

        if (!Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Return);
    }
}
