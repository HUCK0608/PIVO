using UnityEngine;

public class CPlayerState3D_Idle : CPlayerState3D
{
    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(Vector3.zero);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller3D.IsCanClimb())
            Controller3D.ChangeState(EPlayerState3D.Climb);
        else if (!vertical.Equals(0f) || !horizontal.Equals(0f))
            Controller3D.ChangeState(EPlayerState3D.Move);
    }
}
