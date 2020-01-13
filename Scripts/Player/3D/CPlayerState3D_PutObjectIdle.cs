using UnityEngine;

public class CPlayerState3D_PutObjectIdle : CPlayerState3D
{
    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.PutObjectFalling);
        else if (Controller3D.RigidBody.velocity.x != 0 || Controller3D.RigidBody.velocity.z != 0)
            Controller3D.ChangeState(EPlayerState3D.PutObjectMove);
    }
}
