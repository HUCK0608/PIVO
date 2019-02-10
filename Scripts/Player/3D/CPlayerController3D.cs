﻿using System;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState3D { Idle, Move, Falling, ViewChangeInit, ViewChangeIdle, Climb }

public class CPlayerController3D : MonoBehaviour
{
    /// <summary>상태 모음</summary>
    private Dictionary<EPlayerState3D, CPlayerState3D> _states;

    private EPlayerState3D _currentState;
    /// <summary>현재 플레이어 상태</summary>
    public EPlayerState3D CurrentState { get { return _currentState; } }

    private Rigidbody _rigidBody;
    /// <summary>리지드바디</summary>
    public Rigidbody RigidBody { get { return _rigidBody; } }

    [SerializeField]
    private CViewChangeRect _viewChangeRect = null;
    /// <summary>시점전환 상자</summary>
    public CViewChangeRect ViewChangeRect { get { return _viewChangeRect; } }

    /// <summary>중력 확인 지점들</summary>
    [SerializeField]
    private Transform[] _gravityCheckPoints = null;
    /// <summary>중력 확인 지점 개수</summary>
    private int _gravityCheckPointCount = 4;
    /// <summary>중력 체크시 무시할 레이어</summary>
    private int _gravityCheckIgnoreLayerMask;

    private bool _isUseGravity = true;
    /// <summary>중력 적용 여부</summary>
    public bool IsUseGravity { get { return _isUseGravity; } set { _isUseGravity = value; } }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _gravityCheckIgnoreLayerMask = (-1) - (CLayer.Player.LeftShiftToOne());

        InitStates();
    }

    private void Start()
    {
        ChangeState(EPlayerState3D.Idle);
    }

    /// <summary>상태 초기화</summary>
    private void InitStates()
    {
        _states = new Dictionary<EPlayerState3D, CPlayerState3D>();

        // EPlayerState3D 값들
        EPlayerState3D[] enumValues = (EPlayerState3D[])Enum.GetValues(typeof(EPlayerState3D));
        // 상태 개수
        int stateCount = enumValues.Length;
        // 상태의 앞 네임
        string stateFirstPath = "CPlayerState3D_";

        for(int i = 0; i < stateCount; i++)
        {
            // 찾을 상태 풀 네임
            string stateFullPath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 가져오기
            CPlayerState3D state = GetComponent(stateFullPath) as CPlayerState3D;
            // 상태 저장
            _states.Add(enumValues[i], state);
            // 상태 비활성화
            state.enabled = false;
        }
    }

    /// <summary>플레이어의 상태를 변경</summary>
    public void ChangeState(EPlayerState3D state)
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

    /// <summary>중력 계산</summary>
    private void CalcGravity(ref Vector3 velocity)
    {
        bool isApplyGravity = true;

        for (int i = 0; i < _gravityCheckPointCount; i++)
        {
            if (!_isUseGravity || Physics.Raycast(_gravityCheckPoints[i].position, Vector3.down, 0.3f, _gravityCheckIgnoreLayerMask))
            {
                isApplyGravity = false;
                break;
            }
        }

        if (isApplyGravity)
            velocity.y = _rigidBody.velocity.y + CPlayerManager.Instance.Stat.Gravity * Time.deltaTime;
        else
            velocity.y = 0f;
    }

    /// <summary>방향 이동</summary>
    public void Move(Vector3 direction)
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        Vector3 velocity = direction * CPlayerManager.Instance.Stat.MoveSpeed * Time.deltaTime;
        CalcGravity(ref velocity);

        _rigidBody.velocity = velocity;

        if(!direction.Equals(Vector3.zero))
            RotationSlerp(direction);
    }

    /// <summary>키보드 입력 이동</summary>
    public void Move(float vertical, float horizontal)
    {
        Vector3 direction = (Vector3.right + Vector3.forward) * vertical + (Vector3.right + Vector3.back) * horizontal;

        Move(direction.normalized);
    }

    /// <summary>Slerp 회전</summary>
    public void RotationSlerp(Vector3 direction)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), CPlayerManager.Instance.Stat.RotationSpeed * Time.deltaTime);
    }

    /// <summary>해당 방향을 바라보게 함</summary>
    public void LookDirection(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
