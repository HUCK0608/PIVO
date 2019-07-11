using System.Collections;
using UnityEngine;

public class CPushSwitch : MonoBehaviour
{
    /// <summary>밀기 상자</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private CPushBox _pushBox = null;

    /// <summary>플레이어가 고정될 위치</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private Transform _playerHoldingPoint = null;

    /// <summary>플레이어가 진입했을 때 실행</summary>
    public void EnterPlayer()
    {
        StartCoroutine(InitSwitchLogic());
    }

    /// <summary>스위치 Init 로직</summary>
    private IEnumerator InitSwitchLogic()
    {
        // 2D에서 들어오는 상황 때문에 3D가 될 때 까지 기다림
        yield return new WaitUntil(() => CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D));
        bool isOnUi = true;

        while (true)
        {
            // 상호작용 키를 눌렀을 때 상호작용 로직으로 넘어가고 UI 끄기
            if(Input.GetKeyDown(CKeyManager.InteractionKey) && !CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.PushEnd))
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                StartCoroutine(InteractionSwitchLogic());
                break;
            }
            // 시점전환 시 UI 끄기
            if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                break;
            }
            // UI 끄기
            else if(isOnUi && CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.ViewChangeInit))
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                isOnUi = false;
            }
            // UI 켜기
            else if(!isOnUi && !CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.ViewChangeInit) && 
                               !CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.ViewChangeIdle))
            {
                CUIManager.Instance.SetActiveInteractionUI(true);
                isOnUi = true;
            }

            yield return null;
        }
    }

    /// <summary>스위치 상호작용 로직</summary>
    private IEnumerator InteractionSwitchLogic()
    {
        CPlayerManager.Instance.StartAutoMove(_playerHoldingPoint.position);
        yield return new WaitUntil(() => CPlayerManager.Instance.IsCanOperation);
        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PushInit);

        while(true)
        {
            // 상자가 움직이는 중이거나 현재 플레이어 상태가 PushIdle 상태가 아니면 리턴
            if (_pushBox.IsMove || !CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.PushIdle))
            {
                yield return null;
                continue;
            }

            // 상호작용 취소 키를 누르면 pushEnd 상태로 변경 후 UI를 킨 후 InitSwitch 로직으로 넘어감
            if (Input.GetKeyDown(CKeyManager.InteractionCancelKey))
            {
                CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PushEnd);
                CUIManager.Instance.SetActiveInteractionUI(true);
                StartCoroutine(InitSwitchLogic());
                break;
            }
            else
            {
                float vertical = Input.GetAxis(CString.Vertical);
                float horizontal = Input.GetAxis(CString.Horizontal);

                Vector3 direction = (Vector3.right + Vector3.forward) * vertical + (Vector3.right + Vector3.back) * horizontal;
                direction.x = Mathf.Clamp(direction.x, -1f, 1f);
                direction.z = Mathf.Clamp(direction.z, -1f, 1f);

                _pushBox.MoveBox(direction);
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            CUIManager.Instance.SetActiveInteractionUI(true);
            StartCoroutine(InitSwitchLogic());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            CUIManager.Instance.SetActiveInteractionUI(false);
            StopAllCoroutines();
        }
    }
}
