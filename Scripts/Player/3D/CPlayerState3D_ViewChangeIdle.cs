using UnityEngine;

public class CPlayerState3D_ViewChangeIdle : CPlayerState3D
{
    /// <summary>현재 시점전환 상자의 z 크기</summary>
    private float _currentViewRectScaleZ;

    public override void InitState()
    {
        base.InitState();

        _currentViewRectScaleZ = 0f;
    }

    private void Update()
    {
        if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey1))
            _currentViewRectScaleZ += CPlayerManager.Instance.Stat.ViewRectAdjustSpeed * Time.deltaTime;
        else if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey2))
            _currentViewRectScaleZ -= CPlayerManager.Instance.Stat.ViewRectAdjustSpeed * Time.deltaTime;

        _currentViewRectScaleZ = Mathf.Clamp(_currentViewRectScaleZ, -CPlayerManager.Instance.Stat.MaxViewRectScaleZ, CPlayerManager.Instance.Stat.MaxViewRectScaleZ);

        Controller3D.ViewChangeRect.SetScaleZ(_currentViewRectScaleZ);

        if (Input.GetKeyDown(CKeyManager.ViewChangeCancelKey))
        {
            CWorldManager.Instance.ResetIncludedWorldObjects();
            Controller3D.ChangeState(EPlayerState3D.Idle);
        }
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
        {
            CWorldManager.Instance.ChangeWorld();
            Controller3D.ChangeState(EPlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        base.EndState();

        Controller3D.ViewChangeRect.gameObject.SetActive(false);
    }
}
