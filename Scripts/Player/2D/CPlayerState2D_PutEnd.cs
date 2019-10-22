using UnityEngine;

public class CPlayerState2D_PutEnd : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Stat.Hp -= 1;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
