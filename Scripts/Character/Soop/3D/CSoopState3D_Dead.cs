using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopState3D_Dead : CSoopState3D
{
    private void Update()
    {
        AnimatorStateInfo currentAnimtorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimtorStateInfo.IsName("Dead"))
            Controller3D.ChangeAnimation();
    }
}
