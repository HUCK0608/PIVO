using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutEnd : CPlayerState3D
{
    private bool _bIsHit = false;

    public override void InitState()
    {
        base.InitState();

        _bIsHit = false;
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (false == _bIsHit && currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 0.1f)
        {
            CPlayerManager.Instance.Stat.Hp -= 1;
            _bIsHit = true;
        }

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
