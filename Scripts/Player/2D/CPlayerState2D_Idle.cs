using UnityEngine;

public class CPlayerState2D_Idle : CPlayerState2D
{
    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing) ||
            !CPlayerManager.Instance.IsCanOperation)
        {
            Controller2D.Move(0f);

            return;
        }

        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(Vector2.zero);

        if (Controller2D.RigidBody2D.velocity.y < -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) && !CPlayerManager.Instance.IsOnSoopDetection)
        {
            SoundManager.Instance.PlaySFX(ESFXType.ViewChange_ChangeEnd);
            CWorldManager.Instance.ChangeWorld();
        }
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller2D.IsCanClimb() && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller2D.ChangeState(EPlayerState2D.Climb);
        else if (!horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.Move);
    }
}
