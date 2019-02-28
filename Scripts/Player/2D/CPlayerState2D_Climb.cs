using UnityEngine;

public class CPlayerState2D_Climb : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        transform.position = Controller2D.ClimbInfo.destination;
        Controller2D.ChangeState(EPlayerState2D.Idle);
    }
}
