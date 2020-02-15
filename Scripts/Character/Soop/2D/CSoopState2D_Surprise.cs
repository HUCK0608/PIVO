using UnityEngine;

public class CSoopState2D_Surprise : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        Vector3 newScale = Vector3.one;
        newScale.x = Controller2D.Manager.Stat.IsSoopDirectionRight ? -1 : 1;
        transform.localScale = newScale;

        CPlayerManager.Instance.RegisterDetectionSoop(Controller2D.Manager.gameObject);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("Surprise") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(ESoopState.Chase);
    }
}
