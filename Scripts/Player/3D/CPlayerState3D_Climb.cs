using System.Collections;
using UnityEngine;

public class CPlayerState3D_Climb : CPlayerState3D
{
    [SerializeField]
    private Transform[] _cameraTarget = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.Move(Vector3.zero);
        Controller3D.LookDirection(Controller3D.ClimbInfo.direction);
        transform.position = Controller3D.ClimbInfo.origin;

        Controller3D.Animator.SetInteger("Climb", Controller3D.ClimbInfo.aniNumber);

        CCameraController.Instance.Target = _cameraTarget[Controller3D.ClimbInfo.aniNumber];
    }

    private void Update()
    {
        if(Controller3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("Climb_0") && 
           Controller3D.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
        else if(Controller3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("Climb_1") &&
                Controller3D.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.02f)
            Controller3D.ChangeState(EPlayerState3D.Idle);
    }

    public override void EndState()
    {
        transform.position = Controller3D.ClimbInfo.destination;
        CCameraController.Instance.Target = transform;
        Controller3D.Animator.SetInteger("Climb", -1);
    }
}
