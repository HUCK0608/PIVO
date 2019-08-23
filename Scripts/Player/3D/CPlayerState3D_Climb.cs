using System.Collections;
using UnityEngine;

public class CPlayerState3D_Climb : CPlayerState3D
{
    [SerializeField]
    private Transform[] _cameraTarget = null;

    private string _sClimb = "Climb";
    private string _sClimb_0 = "Climb_0";
    private string _sClimb_1 = "Climb_1";

    public override void InitState()
    {
        base.InitState();

        Controller3D.Move(Vector3.zero);
        Controller3D.LookDirection(Controller3D.ClimbInfo.direction);
        transform.position = Controller3D.ClimbInfo.origin;

        Controller3D.Animator.SetInteger(_sClimb, Controller3D.ClimbInfo.aniNumber);

        CCameraController.Instance.Target = _cameraTarget[Controller3D.ClimbInfo.aniNumber];
    }

    private void Update()
    {
        AnimatorStateInfo currentStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);

        if (currentStateInfo.IsName(_sClimb_0) && currentStateInfo.normalizedTime >= 1.05f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
        else if (currentStateInfo.IsName(_sClimb_1) && currentStateInfo.normalizedTime >= 1.02f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }

    public override void EndState()
    {
        transform.position = Controller3D.ClimbInfo.destination;
        CCameraController.Instance.Target = transform;
        Controller3D.Animator.SetInteger(_sClimb, -1);
    }
}
