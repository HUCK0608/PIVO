using UnityEngine;

public class CPlayerState3D_Move : CPlayerState3D
{
    private void Update()
    {
        if (!CPlayerManager.Instance.IsCanOperation)
            return;

        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller3D.IsCanClimb() && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller3D.ChangeState(EPlayerState3D.Climb);
        else if (Controller3D.RigidBody.velocity.x.Equals(0f) && Controller3D.RigidBody.velocity.z.Equals(0f))
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
