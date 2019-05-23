using UnityEngine;

public class CPlayerState2D_Falling : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        Controller2D.LastGroundPosition = transform.position + Vector3.right * -transform.localScale.x * 2f;
    }

    private void Update()
    {
        Controller2D.Move(0);

        if (Controller2D.RigidBody2D.velocity.y >= -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
