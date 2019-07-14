using UnityEngine;

public class CPlayerState3D_PushInit : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.Move(Vector3.zero);
        Controller3D.LookDirection(Vector3.back);
    }

    private void Update()
    {
        AnimatorStateInfo currentAnimatorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentAnimatorStateInfo.IsName("PushInit") && currentAnimatorStateInfo.normalizedTime >= 1.0f)
            Controller3D.ChangeState(EPlayerState3D.PushIdle);
    }
}
