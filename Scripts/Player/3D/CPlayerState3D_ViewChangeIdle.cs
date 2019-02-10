using UnityEngine;

public class CPlayerState3D_ViewChangeIdle : CPlayerState3D
{
    /// <summary>블락 체크포인트들</summary>
    [SerializeField]
    private Transform[] _blockCheckPoints = null;
    /// <summary>블락 체크포인트 개수</summary>
    private int _blockCheckPointCount = 5;
    /// <summary>블락 체크시 무시할 레이어 마스크</summary>
    private int _blockCheckIgnoreLayerMask;

    /// <summary>현재 시점전환 상자의 z 크기</summary>
    private float _currentViewRectScaleZ;

    protected override void Awake()
    {
        base.Awake();

        _blockCheckIgnoreLayerMask = (-1) - (CLayer.Player.LeftShiftToOne() | CLayer.ViewChangeRect.LeftShiftToOne());
    }

    public override void InitState()
    {
        base.InitState();

        _currentViewRectScaleZ = 0f;
    }

    private void Update()
    {
        if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey1) || Input.GetKey(CKeyManager.AnotherViewRectScaleAdjustKey1))
            _currentViewRectScaleZ += CPlayerManager.Instance.Stat.ViewRectAdjustSpeed * Time.deltaTime;
        else if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey2) || Input.GetKey(CKeyManager.AnotherViewRectScaleAdjustKey2))
            _currentViewRectScaleZ -= CPlayerManager.Instance.Stat.ViewRectAdjustSpeed * Time.deltaTime;

        _currentViewRectScaleZ = Mathf.Clamp(_currentViewRectScaleZ, -CPlayerManager.Instance.Stat.MaxViewRectScaleZ, CPlayerManager.Instance.Stat.MaxViewRectScaleZ);

        Controller3D.ViewChangeRect.SetScaleZ(_currentViewRectScaleZ);

        if (Input.GetKeyDown(CKeyManager.ViewChangeCancelKey) || Input.GetKeyDown(CKeyManager.AnotherViewChangeCancelKey))
        {
            CWorldManager.Instance.ResetIncludedWorldObjects();
            Controller3D.ChangeState(EPlayerState3D.Idle);
        }
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
        {
            if (IsCanChange())
            {
                CWorldManager.Instance.ChangeWorld();
                Controller3D.ChangeState(EPlayerState3D.Idle);
            }
        }
    }

    /// <summary>변환을 할 수 있으면 true를 반환</summary>
    private bool IsCanChange()
    {
        bool result = true;

        Vector3 direction = Vector3.zero;
        direction.z = Mathf.Sign(_currentViewRectScaleZ);

        float distance = Mathf.Abs(_currentViewRectScaleZ);

        RaycastHit hit;
        for(int i = 0; i < _blockCheckPointCount; i++)
        {
            if(Physics.Raycast(_blockCheckPoints[i].position, direction, out hit, distance, _blockCheckIgnoreLayerMask))
            {
                result = false;
                Debug.Log(hit.transform.name, hit.transform.gameObject);
                break;
            }
        }

        return result;
    }

    public override void EndState()
    {
        base.EndState();

        Controller3D.ViewChangeRect.gameObject.SetActive(false);
    }
}
