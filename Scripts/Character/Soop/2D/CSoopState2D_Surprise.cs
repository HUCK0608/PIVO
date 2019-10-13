using UnityEngine;

public class CSoopState2D_Surprise : CSoopState2D
{
    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("Surprise") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(ESoopState.Chase);
    }
}
