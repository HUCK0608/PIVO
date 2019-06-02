using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerEffect : MonoBehaviour
{
    [Header("Programmer can edit")]

    /** 먼지 이펙트 **/
    [SerializeField]
    private GameObject _moveDustEffect = null;
    [SerializeField]
    private Vector3 _moveDustPosition3D = new Vector3(0f, 0.3f, -0.3f);
    [SerializeField]
    private Vector3 _moveDustPosition2D = new Vector3(-0.6f, 0.3f, 0f);

    /// <summary>먼지 이펙트 상태 변경</summary>
    public void MoveDustEffect_ChangeState(EWorldState currentWorldState)
    {
        if(currentWorldState.Equals(EWorldState.View3D))
        {
            _moveDustEffect.transform.parent = CPlayerManager.Instance.RootObject3D.transform;
            _moveDustEffect.transform.localPosition = _moveDustPosition3D;
        }
        else if(currentWorldState.Equals(EWorldState.View2D))
        {
            _moveDustEffect.transform.parent = CPlayerManager.Instance.RootObject2D.transform;
            _moveDustEffect.transform.localPosition = _moveDustPosition2D;
        }
    }

    /// <summary>먼지 이펙트 활성화 설정</summary>
    public void MoveDustEffect_SetActive(bool value) { _moveDustEffect.SetActive(value); }


    /** 시점전환 이펙트 **/
    [SerializeField]
    private GameObject _viewChangeWandEffect = null;
    [SerializeField]
    private GameObject _viewChangeCapsuleEffect = null;

    /// <summary>시점전환 지팡이 이펙트 활성화 설정</summary>
    public void ViewChangeWandEffect_SetActive(bool value) { _viewChangeWandEffect.SetActive(value); }
    /// <summary>시점전환 캡슐 이펙트 활성화
    public void ViewChangeCapsuleEffect_ActiveEnable()
    {
        _viewChangeCapsuleEffect.transform.position = CPlayerManager.Instance.RootObject3D.transform.position;
        _viewChangeCapsuleEffect.SetActive(false);
        _viewChangeCapsuleEffect.SetActive(true);
    }
}
