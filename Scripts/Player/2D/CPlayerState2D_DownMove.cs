using UnityEngine;

public class CPlayerState2D_DownMove : CPlayerState2D
{
    private void Update()
    {
        if (!CPlayerManager.Instance.IsCanOperation)
            return;

        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(horizontal);

        if (horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.DownIdle);
    }
}
