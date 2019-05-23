using UnityEngine;

public class CPlayerState3D_HoldFalling : CPlayerState3D
{
    private void Update()
    {
        Controller3D.Move(0f, 0f);

        if (Controller3D.RigidBody.velocity.y >= -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
