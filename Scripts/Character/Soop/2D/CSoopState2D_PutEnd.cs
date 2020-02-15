using UnityEngine;

public class CSoopState2D_PutEnd : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.PutEnd);
        CPlayerManager.Instance.RemoveDetectionSoop(Controller2D.Manager.gameObject);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
        {
            Manager.Controller3D.ChangeState(ESoopState.Return);
            Manager.Controller3D.ChangeAnimation();
            Controller2D.ChangeState(ESoopState.Return);
        }
    }
}
