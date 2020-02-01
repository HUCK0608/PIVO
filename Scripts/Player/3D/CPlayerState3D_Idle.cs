using UnityEngine;

public class CPlayerState3D_Idle : CPlayerState3D
{
    private float _currentIdleTime = 0f;

    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing) ||
            !CPlayerManager.Instance.IsCanOperation)
        {
            Controller3D.Move(0f, 0f);
            return;
        }

        _currentIdleTime += Time.deltaTime;

        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller3D.IsCanClimb() && !CPlayerManager.Instance.IsOnSoopDetection)
            Controller3D.ChangeState(EPlayerState3D.Climb);
        else if (Controller3D.RigidBody.velocity.x != 0 || Controller3D.RigidBody.velocity.z != 0)
            Controller3D.ChangeState(EPlayerState3D.Move);
        else if(_currentIdleTime >= CPlayerManager.Instance.Stat.IdleVariationMinTime)
        {
            int randomValue = Random.Range(0, 2);

            if (randomValue.Equals(0))
                Controller3D.ChangeState(EPlayerState3D.Idle2);
            else
                Controller3D.ChangeState(EPlayerState3D.Idle3);
        }
    }

    public override void EndState()
    {
        base.EndState();

        _currentIdleTime = 0f;
    }
}
