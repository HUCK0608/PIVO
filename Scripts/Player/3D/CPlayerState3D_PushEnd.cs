using UnityEngine;

public class CPlayerState3D_PushEnd : CPlayerState3D
{
    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PushEnd") && currentAnimatorStateInfo.normalizedTime >= 1.0f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
