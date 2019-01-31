using UnityEngine;

public class CPlayerState3D_Falling : CPlayerState3D
{
    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y >= -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
