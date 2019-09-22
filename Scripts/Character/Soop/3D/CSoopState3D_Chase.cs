using UnityEngine;

public class CSoopState3D_Chase : CSoopState3D
{
    private void Update()
    {
        Controller.MoveToPoint(CPlayerManager.Instance.RootObject3D.transform.position);

        if (Vector3.Distance(transform.position, CPlayerManager.Instance.RootObject3D.transform.position) <= Controller.Manager.Stat.PutDistance)
            Controller.ChangeState(ESoopState.PutInit);
        else if (!Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Return);
    }
}
