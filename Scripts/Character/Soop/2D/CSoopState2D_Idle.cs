using UnityEngine;

public class CSoopState2D_Idle : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        Vector3 newScale = Vector3.one;
        newScale.x = Controller2D.Manager.Stat.IsSoopDirectionRight ? -1 : 1;
        transform.localScale = newScale;
    }

    private void Update()
    {
        if (!Controller2D.CanOperation())
            return;

        Vector3 startPoint = Controller2D.Manager.transform.position;

        if (Controller2D.IsDetectionPlayer())
            Controller2D.ChangeState(ESoopState.Surprise);
        else if (!transform.position.Equals(startPoint))
            Controller2D.ChangeState(ESoopState.Return);
    }
}
