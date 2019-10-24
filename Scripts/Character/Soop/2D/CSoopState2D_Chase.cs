using UnityEngine;

public class CSoopState2D_Chase : CSoopState2D
{
    private void Update()
    {
        Controller2D.MoveToPoint(CPlayerManager.Instance.RootObject2D.transform.position);

        if (Vector2.Distance(transform.position, CPlayerManager.Instance.RootObject2D.transform.position) <= Controller2D.Manager.Stat.PutDistance)
            Controller2D.ChangeState(ESoopState.PutInit);
        else if (!Controller2D.IsDetectionPlayer())
            Controller2D.ChangeState(ESoopState.Return);
    }
}
