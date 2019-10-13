using UnityEngine;

public class CPlayerState2D_PutInit : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        Controller2D.RigidBody2D.velocity = Vector2.zero;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutInit") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(EPlayerState2D.PutIdle);
    }
}
