using UnityEngine;

public class CPlayerState3D_Falling : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.LastGroundPosition = transform.position + -transform.forward * 2f;

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(false);
    }

    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal, true);

        if (Controller3D.RigidBody.velocity.y >= -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }

    public override void EndState()
    {
        base.EndState();

        CPlayerManager.Instance.Effect.MoveDustEffect_SetActive(true);
    }
}
