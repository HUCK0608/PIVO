using UnityEngine;

public class CPlayerState2D_Falling : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(false);
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal, true);

        if (Controller2D.RigidBody2D.velocity.y >= -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(true);
    }
}
