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

    /// <summary>카메라 포커싱 위치</summary>
    [SerializeField]
    private Transform _cameraFocusingPoint = null;

    private void Awake()
    {
        _currentWorldState3DWU = new WaitUntil(() => CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D));

        _playerCanOperationWU = new WaitUntil(() => CPlayerManager.Instance.IsCanOperation);
        _playerPushIdleStateWU = new WaitUntil(() => CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.PushIdle));
        _cameraDistanceToTargetWU = new WaitUntil(() => Vector3.Distance(CCameraController.Instance.transform.position, CCameraController.Instance.Target.position) < 0.3f);
        _playerIdleStateWU = new WaitUntil(() => CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.Idle));
    }

    /// <summary>플레이어가 진입했을 때 실행</summary>
    public void EnterPlayer()
    {
        StartCoroutine(InitSwitchLogic());
    }

    // 아래 코루틴에서 사용 할 waitUntil들
    WaitUntil _currentWorldState3DWU = null;

    /// <summary>스위치 Init 로직</summary>
    private IEnumerator InitSwitchLogic()
    {
        // 2D에서 들어오는 상황 때문에 3D가 될 때 까지 기다림
        yield return _currentWorldState3DWU;
        bool isOnUi = true;
        bool isPreviousePlayerStateChangeView = false;

        while (true)
        {
            // 상호작용 키를 눌렀을 때 상호작용 로직으로 넘어가고 UI 끄기
            if(Input.GetKeyDown(CKeyManager.InteractionKey) && CPlayerManager.Instance.IsCanOperation && !isPreviousePlayerStateChangeView && !CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.PushEnd))
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
            else if(isOnUi && isPreviousePlayerStateChangeView)
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                isOnUi = false;
            }
            // UI 켜기
            else if(!isOnUi && !isPreviousePlayerStateChangeView)
            {
                CUIManager.Instance.SetActiveInteractionUI(true);
                isOnUi = true;
            }

            // 이전 상태 정보 저장
            EPlayerState3D currentState = CPlayerManager.Instance.Controller3D.CurrentState;
            isPreviousePlayerStateChangeView = currentState.Equals(EPlayerState3D.ViewChangeInit) || currentState.Equals(EPlayerState3D.ViewChangeIdle);

            yield return null;
        }
    }

    // 아래 코루틴에 사용 할 waitUnit들
    WaitUntil _playerCanOperationWU = null;
    WaitUntil _playerPushIdleStateWU = null;
    WaitUntil _cameraDistanceToTargetWU = null;
    WaitUntil _playerIdleStateWU = null;

    /// <summary>스위치 상호작용 로직</summary>
    private IEnumerator InteractionSwitchLogic()
    {
        // 플레이어가 고정될 위치로 뛰어간 후 고정될 위치로 가면 PushInit 상태로 변경
        CPlayerManager.Instance.StartAutoMove(_playerHoldingPoint.position);
        yield return _playerCanOperationWU;
        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PushInit);

        // 플레이어가 PushIdle 상태가 되면 카메라 타겟을 포커싱으로 변경 후 lerp 이동을 시킴
        yield return _playerPushIdleStateWU;
        CCameraController.Instance.Target = _cameraFocusingPoint;
        CCameraController.Instance.IsLerpMove = true;

        // 카메라가 타겟에 일정거리까지 다가갈 때까지 대기
        yield return _cameraDistanceToTargetWU;

        while(true)
        {
            // 상자가 움직이는 중이면 대기
            if(_pushBox.IsMove)
            {
                yield return null;
                continue;
            }

            // 상호작용 취소 키를 눌렀을 때 수행
            if (Input.GetKeyDown(CKeyManager.InteractionCancelKey) && CPlayerManager.Instance.IsCanOperation)
            {
                // 플레이어로 타겟을 변경 후 카메라 고정
                CCameraController.Instance.Target = CPlayerManager.Instance.Controller3D.transform;
                yield return _cameraDistanceToTargetWU;
                CCameraController.Instance.IsLerpMove = false;

                // 플레이어를 PushEnd 상태로 변경
                CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PushEnd);

                // 플레이어가 Idle 상태가 되면 UI를 키고 InitSwitch 로직으로 전환
                yield return _playerIdleStateWU;
                CUIManager.Instance.SetActiveInteractionUI(true);
                StartCoroutine(InitSwitchLogic());
                break;
            }
            else
            {
                if (CPlayerManager.Instance.IsCanOperation)
                {
                    if (Input.GetKey(KeyCode.UpArrow))
                        _pushBox.MoveBox(Vector3.forward + Vector3.right);
                    else if (Input.GetKey(KeyCode.RightArrow))
                        _pushBox.MoveBox(Vector3.back + Vector3.right);
                    else if (Input.GetKey(KeyCode.DownArrow))
                        _pushBox.MoveBox(Vector3.back + Vector3.left);
                    else if (Input.GetKey(KeyCode.LeftArrow))
                        _pushBox.MoveBox(Vector3.forward + Vector3.left);
                }
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
