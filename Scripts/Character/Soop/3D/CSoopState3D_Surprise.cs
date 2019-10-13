using UnityEngine;

public class CSoopState3D_Surprise : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection((CPlayerManager.Instance.RootObject3D.transform.position - transform.position).normalized);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("Surprise") && currentAnimatorStateInfo.normalizedTime >= 1f)
        {
            if (Controller3D.IsDetectionPlayer())
                Controller3D.ChangeState(ESoopState.Chase);
            else
                Controller3D.ChangeState(ESoopState.Idle);
        }
    }
}
