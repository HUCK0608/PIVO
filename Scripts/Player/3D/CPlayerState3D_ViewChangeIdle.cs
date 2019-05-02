using System.Collections.Generic;
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

    /// <summary>블락된 오브젝트들</summary>
    private List<CWorldObject> _blockObjects;
    /// <summary>블락된 오브젝트들의 개수</summary>
    private int _blockObjetCount = 0;

    /// <summary>지팡이 시점전환 이펙트</summary>
    [SerializeField]
    private GameObject _viewChangeWandEffect = null;

    /// <summary>캡슐 시점전환 이펙트</summary>
    [SerializeField]
    private GameObject _viewChangeCapsuleEffect = null;

    protected override void Awake()
    {
        base.Awake();

        _blockCheckIgnoreLayerMask = (-1) - (CLayer.Player.LeftShiftToOne() | CLayer.ViewChangeRect.LeftShiftToOne() | CLayer.BackgroundObject.LeftShiftToOne());

        _blockObjects = new List<CWorldObject>();
    }

    public override void InitState()
    {
        base.InitState();

        _blockCheckPoints[0].transform.parent.eulerAngles = Vector3.zero;

        _viewChangeWandEffect.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey1) || Input.GetKey(CKeyManager.AnotherViewRectScaleAdjustKey1))
            ;//Controller3D.ViewChangeRect.IncreaseScaleZ();
        else if (Input.GetKey(CKeyManager.ViewRectScaleAdjustKey2) || Input.GetKey(CKeyManager.AnotherViewRectScaleAdjustKey2))
            ;//Controller3D.ViewChangeRect.DecreaseScaleZ();

        bool isCanChange = IsCanChange();

        if (Input.GetKeyDown(CKeyManager.ViewChangeCancelKey) || Input.GetKeyDown(CKeyManager.AnotherViewChangeCancelKey))
        {
            CWorldManager.Instance.ResetIncludedWorldObjects();
            Controller3D.ChangeState(EPlayerState3D.Idle);
        }
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
        {
            if (isCanChange)
            {
                CWorldManager.Instance.ChangeWorld();
                _viewChangeCapsuleEffect.transform.position = transform.position;
                _viewChangeCapsuleEffect.SetActive(false);
                _viewChangeCapsuleEffect.SetActive(true);
                Controller3D.ChangeState(EPlayerState3D.Idle);
            }
            else
            {
                CCameraController.Instance.OnCamerShaking();
            }
        }
    }

    /// <summary>변환을 할 수 있으면 true를 반환</summary>
    private bool IsCanChange()
    {
        //bool result = true;

        //Vector3 direction = Vector3.zero;
        //direction.z = Mathf.Sign(Controller3D.ViewChangeRect.CurrentScaleZ);

        //float distance = Mathf.Abs(Controller3D.ViewChangeRect.CurrentScaleZ) - 0.1f;

        //RaycastHit hit;
        //for(int i = 0; i < _blockCheckPointCount; i++)
        //{
        //    Debug.DrawRay(_blockCheckPoints[i].position, direction * distance, Color.red);
        //    if(Physics.Raycast(_blockCheckPoints[i].position, direction, out hit, distance, _blockCheckIgnoreLayerMask))
        //    {
        //        result = false;

        //        bool isShowBlock = false;

        //        for(int j = 0; j < _blockObjetCount; j++)
        //        {
        //            if (_blockObjects[j].gameObject.Equals(hit.transform.gameObject))
        //            {
        //                _blockObjects[j].ShowOnBlock();
        //                isShowBlock = true;
        //                break;
        //            }
        //        }

        //        if(!isShowBlock)
        //        {
        //            CWorldObject newBlockObject = hit.transform.GetComponent<CWorldObject>();
        //            newBlockObject.ShowOnBlock();
        //            _blockObjects.Add(newBlockObject);
        //            _blockObjetCount++;
        //        }
        //    }
        //}

        //return result;

        return true;
    }

    public override void EndState()
    {
        base.EndState();

        //Controller3D.ViewChangeRect.StopSetScaleZ();
        //Controller3D.ViewChangeRect.gameObject.SetActive(false);

        //for (int i = 0; i < _blockObjetCount; i++)
        //    _blockObjects[i].ShowOffBlock();

        //_blockObjects.Clear();
        //_blockObjetCount = 0;

        //_viewChangeWandEffect.SetActive(false);
        //Controller3D.ViewChangeRect.SetEffectEnable(false);
    }
}
