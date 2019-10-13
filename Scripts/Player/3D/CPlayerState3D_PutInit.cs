using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutInit : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.RigidBody.velocity = Vector3.zero;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutInit") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller3D.ChangeState(EPlayerState3D.PutIdle);
    }
}
