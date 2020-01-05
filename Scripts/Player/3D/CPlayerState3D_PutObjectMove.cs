using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerState3D_PutObjectMove : CPlayerState3D
{
    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.x.Equals(0f) && Controller3D.RigidBody.velocity.z.Equals(0f))
            Controller3D.ChangeState(EPlayerState3D.PutObjectIdle);
    }
}
