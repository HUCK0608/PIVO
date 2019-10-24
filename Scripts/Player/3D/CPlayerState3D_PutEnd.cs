using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutEnd : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Stat.Hp -= 1;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
