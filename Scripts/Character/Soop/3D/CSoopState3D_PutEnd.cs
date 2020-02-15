using UnityEngine;

public class CSoopState3D_PutEnd : CSoopState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PutEnd);
        CPlayerManager.Instance.RemoveDetectionSoop(Controller3D.Manager.gameObject);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PutEnd") && currentAnimatorStateInfo.normalizedTime >= 1f)
            Controller3D.ChangeState(ESoopState.Return);
    }
}
