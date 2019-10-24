using UnityEngine;

public class CSoopState3D_Chase : CSoopState3D
{
    private void Update()
    {
        Controller3D.MoveToPoint(CPlayerManager.Instance.RootObject3D.transform.position);

        if (Vector3.Distance(transform.position, CPlayerManager.Instance.RootObject3D.transform.position) <= Controller3D.Manager.Stat.PutDistance)
            Controller3D.ChangeState(ESoopState.PutInit);
        else if (!Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Return);
    }
}
