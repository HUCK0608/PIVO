using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutObjectInit : CPlayerState3D
{
    private void Update()
    {
        Controller3D.Move(Vector3.zero);

        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimatorStateInfo.IsName("PutObjectInit") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller3D.ChangeState(EPlayerState3D.PutObjectIdle);
    }
}
