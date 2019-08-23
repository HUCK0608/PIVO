using System.Collections;
using UnityEngine;

public class CPlayerState2D_Climb : CPlayerState2D
{
    private string _sClimb = "Climb";

    public override void InitState()
    {
        base.InitState();

        Controller2D.Move(Vector2.zero);
        transform.position = Controller2D.ClimbInfo.origin;
    }

    private void Update()
    {
        AnimatorStateInfo currentStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.IsName(_sClimb) && currentStateInfo.normalizedTime >= 1f)
            Controller2D.ChangeState(EPlayerState2D.Idle);
    }

    public override void EndState()
    {
        transform.position = Controller2D.ClimbInfo.destination;
    }
}
