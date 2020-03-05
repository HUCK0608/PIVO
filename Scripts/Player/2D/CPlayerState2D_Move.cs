using UnityEngine;

public class CPlayerState2D_Move : CPlayerState2D
{
    private void Update()
    {
        if (!CPlayerManager.Instance.IsCanOperation)
            return;

        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal);

        if (Controller2D.RigidBody2D.velocity.y < -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) && !CPlayerManager.Instance.IsOnSoopDetection)
        {
            SoundManager.Instance.PlaySFX(ESFXType.ViewChange_ChangeEnd);
            CWorldManager.Instance.ChangeWorld();
            Controller2D.ChangeState(EPlayerState2D.Idle);
        }
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller2D.IsCanClimb() && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller2D.ChangeState(EPlayerState2D.Climb);
        else if (horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
