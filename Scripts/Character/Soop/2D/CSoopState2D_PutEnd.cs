using UnityEngine;

public class CSoopState2D_PutEnd : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.PutEnd);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(ESoopState.Return);
    }
}
