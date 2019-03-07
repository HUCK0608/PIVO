using System.Collections;
using UnityEngine;

public class CPlayerState2D_Climb : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        Controller2D.Move(Vector2.zero);
        transform.position = Controller2D.ClimbInfo.origin;
    }

    /// <summary>기어오르기 애니메이션이 성공적으로 끝났을 때 실행됨</summary>
    public void CompleteClimbAnimation()
    {
        Controller2D.ChangeState(EPlayerState2D.Idle);
        StartCoroutine(EndClimbLogic());
    }

    private IEnumerator EndClimbLogic()
    {
        yield return null;
        transform.position = Controller2D.ClimbInfo.destination;
    }
}
