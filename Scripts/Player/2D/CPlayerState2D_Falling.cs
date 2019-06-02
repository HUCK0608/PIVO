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
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal, true);

        if (Controller2D.RigidBody2D.velocity.y >= -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
