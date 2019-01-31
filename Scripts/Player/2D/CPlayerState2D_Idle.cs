using UnityEngine;

public class CPlayerState2D_Idle : CPlayerState2D
{
    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller2D.Move(Vector2.zero);

        if (Controller2D.RigidBody2D.velocity.y < -Mathf.Epsilon)
            Controller2D.ChangeState(EPlayerState2D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            CWorldManager.Instance.ChangeWorld();
        else if (!horizontal.Equals(0f))
            Controller2D.ChangeState(EPlayerState2D.Move);
    }
}
