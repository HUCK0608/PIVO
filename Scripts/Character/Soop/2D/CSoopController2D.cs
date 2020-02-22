using System;
using System.Collections.Generic;
using UnityEngine;

public class CSoopController2D : MonoBehaviour
{
    private CSoopManager _manager = null;
    /// <summary>매니저</summary>
    public CSoopManager Manager { get { return _manager; } }

    /// <summary>상태 모음</summary>
    private Dictionary<ESoopState, CSoopState2D> _states = new Dictionary<ESoopState, CSoopState2D>();

    private ESoopState _currentState;
    /// <summary>현재 숲숲이 상태</summary>
    public ESoopState CurrentState { get { return _currentState; } }

    private Animator _animator = null;
    /// <summary>애니메이터</summary>
    public Animator Animator { get { return _animator; } }

    /// <summary>애니메이터 파라미터 이름</summary>
    private string _animParameterPath = "CurrentState";

    private void Awake()
    {
        _manager = GetComponentInParent<CSoopManager>();

        _animator = GetComponent<Animator>();

        InitStates();
    }

    private void Start()
    {
        if(!_currentState.Equals(ESoopState.Dead))
            ChangeState(ESoopState.Idle);
    }

    /// <summary>상태 초기화</summary>
    private void InitStates()
    {
        // ESoopState 값들
        ESoopState[] enumValues = (ESoopState[])Enum.GetValues(typeof(ESoopState));
        // 상태의 앞 네임
        string stateFirstPath = "CSoopState2D_";

        foreach (ESoopState enumValue in enumValues)
        {
            // 찾을 상태의 풀 네임
            string stateFullPath = stateFirstPath + enumValue.ToString("G");
            // 상태 가져오기
            CSoopState2D state = GetComponent(stateFullPath) as CSoopState2D;
            // 상태 저장
            _states.Add(enumValue, state);
            // 상태 비활성화
            state.enabled = false;
        }
    }

    /// <summary>숲숲이의 상태를 변경</summary>
    public void ChangeState(ESoopState state)
    {
        // 기존 상태 종료
        if(_states[_currentState].enabled)
        {
            _states[_currentState].EndState();
            _states[_currentState].enabled = false;
        }

        // 상태 변경
        _currentState = state;

        // 새로운 상태 시작
        _states[_currentState].InitState();
        _states[_currentState].enabled = true;
    }

    /// <summary>해당 위치로 이동</summary>
    public void MoveToPoint(Vector3 point)
    {
        // 이동
        point.y = transform.position.y;
        point.z = transform.position.z;

        transform.position = Vector3.MoveTowards(transform.position, point, _manager.Stat.MoveSpeed * Time.deltaTime);
    }

    /// <summary>플레이어가 탐지되었는지 여부</summary>
    public bool IsDetectionPlayer()
    {
        if (CPlayerManager.Instance.Controller2D.CurrentState.Equals(EPlayerState2D.Climb))
            return false;

        Vector2 playerPosition = CPlayerManager.Instance.RootObject2D.transform.position;
        Vector2 detectionAreaPosition = _manager.Stat.DetectionAreaPosition;
        Vector3 detectionAreaHalfSize = _manager.Stat.DetectionAreaSize * 0.5f;

        Debug.DrawRay(transform.position + Vector3.up, Vector3.right * (_manager.Stat.IsSoopDirectionRight ? 1 : -1) * _manager.Stat.DetectionAreaSize.x * 0.5f, Color.yellow);

        // x 위치 체크
        if (!(playerPosition.x >= detectionAreaPosition.x - detectionAreaHalfSize.x && playerPosition.x <= detectionAreaPosition.x + detectionAreaHalfSize.x))
            return false;

        // y 위치 체크
        if (!(playerPosition.y >= detectionAreaPosition.y - detectionAreaHalfSize.y && playerPosition.y <= detectionAreaPosition.y + detectionAreaHalfSize.y))
            return false;

        // 전방에 있는지 검사
        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up, Vector3.right * (_manager.Stat.IsSoopDirectionRight ? 1 : -1), _manager.Stat.DetectionAreaSize.x * 0.5f, CLayer.Player.LeftShiftToOne());

        // 전방에 존재하지만 벽인지 검사
        if (null != hit.transform && hit.transform.tag.Equals("Player"))
            return true;

        return false;
    }


    /// <summary>숲숲이 작동이 가능한지 여부</summary>
    public bool CanOperation()
    {
        bool isCanOperation = true;

        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.forward, out hit, Mathf.Infinity, (-1) - CLayer.Player.LeftShiftToOne()))
        {
            CWorldObject worldObject = hit.transform.GetComponent<CWorldObject>();
            if (worldObject == null)
                worldObject = hit.transform.GetComponentInChildren<CWorldObject>();
            if (worldObject == null)
                worldObject = hit.transform.GetComponentInParent<CWorldObject>();

            if (worldObject.IsCanChange2D)
                isCanOperation = false;
        }

        return isCanOperation;
    }

    /// <summary>애니메이션을 변경</summary>
    public void ChangeAnimation() { _animator.SetInteger(_animParameterPath, (int)_currentState); }

    /// <summary>숲숲이의 죽음 상태 설정</summary>
    public void ActivateDead(bool active)
    {
        if (active)
        {
            if(!CurrentState.Equals(ESoopState.Dead))
                ChangeState(ESoopState.Dead);
        }
        else
        {
            if (CurrentState.Equals(ESoopState.Dead))
                ChangeState(ESoopState.Idle);
        }
    }
}
