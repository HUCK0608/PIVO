using UnityEngine;

public class CPlayerState3D_Falling : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.LastGroundPosition = transform.position + -transform.forward * 2f;
    }

    private void Update()
    {
        Controller3D.Move(0, 0);

        if (Controller3D.RigidBody.velocity.y >= -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }
}
