﻿using UnityEngine;

public class CPlayerState3D_Move : CPlayerState3D
{
    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (vertical.Equals(0f) && horizontal.Equals(0f))
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
