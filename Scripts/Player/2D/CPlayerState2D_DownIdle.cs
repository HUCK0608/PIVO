using UnityEngine;

public class CPlayerState2D_DownIdle : CPlayerState2D
{
    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing) ||
            !CPlayerManager.Instance.IsCanOperation)
            return;

        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(Vector3.zero);

        if (!horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.DownMove);
    }
}
