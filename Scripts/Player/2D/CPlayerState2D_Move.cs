using UnityEngine;

public class CPlayerState2D_Move : CPlayerState2D
{
    private void Update()
    {
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal);

        if (Controller2D.RigidBody2D.velocity.y < -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
        {
            CWorldManager.Instance.ChangeWorld();
            Controller2D.ChangeState(EPlayerState2D.Idle);
        }
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller2D.IsCanClimb())
            Controller2D.ChangeState(EPlayerState2D.Climb);
        else if (horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
