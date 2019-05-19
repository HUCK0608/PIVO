using UnityEngine;

public class CPlayerState3D_Holding : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();
        Controller3D.IsUseGravity = false;
    }

    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        if (vertical != 0 || horizontal != 0)
            Controller3D.ChangeState(EPlayerState3D.Falling);
    }

    public override void EndState()
    {
        Controller3D.IsUseGravity = true;
    }
}
