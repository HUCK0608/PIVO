using UnityEngine;

public class CPlayerState3D_Holding : CPlayerState3D
{
    /// <summary>캡슐 시점전환 이펙트</summary>
    [SerializeField]
    private GameObject _viewChangeCapsuleEffect = null;

    private float _holdingAddTime = 0f;

    public override void InitState()
    {
        base.InitState();

        Controller3D.IsUseGravity = false;

        _holdingAddTime = 0f;
    }

    private void Update()
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        _holdingAddTime += Time.deltaTime;

        if (vertical != 0 || horizontal != 0 || _holdingAddTime >= CPlayerManager.Instance.Stat.HoldingMaxTime)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if(Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
        {
            CWorldManager.Instance.ChangeWorld(true);
            _viewChangeCapsuleEffect.transform.position = transform.position;
            _viewChangeCapsuleEffect.SetActive(false);
            _viewChangeCapsuleEffect.SetActive(true);
            Controller3D.ChangeState(EPlayerState3D.Idle);
        }
    }

    public override void EndState()
    {
        Controller3D.IsUseGravity = true;
    }
}
