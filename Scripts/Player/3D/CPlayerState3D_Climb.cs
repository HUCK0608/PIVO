using System.Collections;
using UnityEngine;

public class CPlayerState3D_Climb : CPlayerState3D
{
    [SerializeField]
    private Transform _cameraTarget = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.Move(Vector3.zero);
        Controller3D.LookDirection(Controller3D.ClimbInfo.direction);
        transform.position = Controller3D.ClimbInfo.origin;

        CCameraController.Instance.Target = _cameraTarget;
    }

    /// <summary>기어오르기 애니메이션이 성공적으로 끝났을 때 실행됨</summary>
    public void CompleteClimbAnimation()
    {
        Controller3D.ChangeState(EPlayerState3D.Idle);
        StartCoroutine(EndClimbLogic());
    }

    private IEnumerator EndClimbLogic()
    {
        yield return null;
        transform.position = Controller3D.ClimbInfo.destination;
        CCameraController.Instance.Target = transform;
    }
}
