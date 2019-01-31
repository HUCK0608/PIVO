using UnityEngine;

public class CPlayerState2D_Falling : CPlayerState2D
{
    private void Update()
    {
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal);

        if (Controller2D.RigidBody2D.velocity.y >= -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
