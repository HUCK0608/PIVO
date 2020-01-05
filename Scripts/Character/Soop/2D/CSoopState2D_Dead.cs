using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopState2D_Dead : CSoopState2D
{
    private void Update()
    {
        AnimatorStateInfo currentAnimtorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimtorStateInfo.IsName("Dead"))
            Controller2D.ChangeAnimation();
    }

}
