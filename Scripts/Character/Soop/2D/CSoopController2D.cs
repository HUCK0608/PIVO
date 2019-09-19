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

    /// <summary>애니메이터</summary>
    private Animator _animator = null;
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

    /// <summary>애니메이션을 변경</summary>
    public void ChangeAnimation() { _animator.SetInteger(_animParameterPath, (int)_currentState); }
}
