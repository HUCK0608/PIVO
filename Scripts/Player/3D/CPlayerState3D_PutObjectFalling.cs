using UnityEngine;

public class CPlayerState3D_PutObjectFalling : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(false);
    }

    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal, true);

        if (Controller3D.RigidBody.velocity.y >= -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.PutObjectIdle);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(true);
    }
}
