using UnityEngine;

public class CPlayerState3D_Idle3 : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CIdleSpecialCountManager.CountUp();
    }

    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller3D.IsCanClimb())
            Controller3D.ChangeState(EPlayerState3D.Climb);
        else if (Controller3D.RigidBody.velocity.x != 0 || Controller3D.RigidBody.velocity.z != 0)
            Controller3D.ChangeState(EPlayerState3D.Move);
    }
}
