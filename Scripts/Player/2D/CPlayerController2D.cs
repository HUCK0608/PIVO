using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState2D { Idle, Move, Falling, Climb, Dead, DownIdle, DownMove }

public class CPlayerController2D : MonoBehaviour
{
    /// <summary>상태 모음</summary>
    private Dictionary<EPlayerState2D, CPlayerState2D> _states;

    private EPlayerState2D _currentState;
    /// <summary>현재 플레이어 상태</summary>
    public EPlayerState2D CurrentState { get { return _currentState; } }

    private Rigidbody2D _rigidBody2D;
    /// <summary>리지드바디2D</summary>
    public Rigidbody2D RigidBody2D { get { return _rigidBody2D; } }

    private Animator _animator;
    /// <summary>애니메이터</summary>
    public Animator Animator { get { return _animator; } }
    /// <summary>애니메이터 파라미터 이름</summary>
    private static string _animParameterPath = "CurrentState";

    /// <summary>중력 확인 지점들</summary>
    [SerializeField]
    private Transform[] _gravityCheckPoints = null;
    /// <summary>중력 확인 지점 개수</summary>
    private int _gravityCheckPointCount = 2;

    public struct SClimbInfo2D
    {
        public Vector3 origin;
        public Vector3 destination;
    };
    private SClimbInfo2D _climbInfo;
    /// <summary>기어오르기에 대한 시작점, 도착점, 방향을 담고 있는 구조체</summary>
    public SClimbInfo2D ClimbInfo { get { return _climbInfo; } }

    /// <summary>플레이어를 무시하는 레이어 마스크</summary>
    private int _playerIgnoreLayerMask;

    private bool _isUseGravity = true;
    /// <summary>중력 적용 여부</summary>
    public bool IsUseGravity { get { return _isUseGravity; } set { _isUseGravity = value; } }

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _playerIgnoreLayerMask = (-1) - (CLayer.Player.LeftShiftToOne() | CLayer.BackgroundObject.LeftShiftToOne());

        InitStates();
    }

    private void Start()
    {
        ChangeState(EPlayerState2D.Idle);
    }

    /// <summary>상태 초기화</summary>
    private void InitStates()
    {
        _states = new Dictionary<EPlayerState2D, CPlayerState2D>();

        // EPlayerState2D 값들
        EPlayerState2D[] enumValues = (EPlayerState2D[])Enum.GetValues(typeof(EPlayerState2D));
        // 상태 개수
        int stateCount = enumValues.Length;
        // 상태의 앞 네임
        string stateFirstPath = "CPlayerState2D_";

        for(int i = 0; i < stateCount; i++)
        {
            // 찾을 상태의 풀 네임
            string stateFullPath = stateFirstPath + enumValues[i].ToString("G");
            // 상태 가져오기
            CPlayerState2D state = GetComponent(stateFullPath) as CPlayerState2D;
            // 상태 저장
            _states.Add(enumValues[i], state);
            // 상태 비활성화
            state.enabled = false;
        }
    }

    /// <summary>플레이어의 상태를 변경</summary>
    public void ChangeState(EPlayerState2D state)
    {
        if (_currentState.Equals(EPlayerState2D.Dead))
            return;

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

    /// <summary>중력 계산</summary>
    private void CalcGravity(ref Vector2 velocity)
    {
        bool isApplyGravity = true;

        for(int i = 0; i < _gravityCheckPointCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(_gravityCheckPoints[i].position, Vector2.down, 0.15f, _playerIgnoreLayerMask);
            if (!_isUseGravity || hit.transform != null)
            {
                isApplyGravity = false;
                break;
            }
        }

        if (isApplyGravity)
            velocity.y = _rigidBody2D.velocity.y + CPlayerManager.Instance.Stat.Gravity;
        else
            velocity.y = 0f;
    }

    /// <summary>방향 이동</summary>
    public void Move(Vector2 direction, bool isAirMove = false)
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        float moveSpeed = isAirMove ? CPlayerManager.Instance.Stat.AirMoveSpeed : CPlayerManager.Instance.Stat.MoveSpeed;
        Vector2 velocity = direction * moveSpeed;
        CalcGravity(ref velocity);

        _rigidBody2D.velocity = velocity;

        if (!direction.Equals(Vector2.zero))
            LookDirection(direction);

        if(transform.position.y <= CPlayerManager.Instance.Stat.KillYVolume)
        {
            CPlayerManager.Instance.Stat.Hp -= 1;
            _rigidBody2D.velocity = Vector2.zero;
            transform.position = CPlayerManager.Instance.LastGroundPosition;
        }
    }

    /// <summary>키보드 입력 이동</summary>
    public void Move(float horizontal, bool isAirMove = false)
    {
        Vector2 direction = Vector2.right * horizontal;

        Move(direction.normalized, isAirMove);
    }

    /// <summary>해당 방향을 바라보게 함</summary>
    public void LookDirection(Vector2 direction)
    {
        if(!transform.localScale.x.Equals(direction.x))
        {
            Vector3 newScale = Vector3.one;
            newScale.x = direction.x;
            transform.localScale = newScale;
        }
    }

    public bool IsCanClimb()
    {
        bool result = false;

        RaycastHit2D hit;

        Vector2 origin = transform.position;
        origin.y += 1f;
        Vector2 direction = transform.localScale;
        direction.y = 0f;

        if(hit = Physics2D.Raycast(origin, direction, 1f, _playerIgnoreLayerMask))
        {
            Vector2 center = hit.point + -hit.normal + Vector2.up * 2f;

            if(!Physics2D.BoxCast(center, Vector2.one * 1.5f, 0f, Vector2.up, 2f))
            {
                result = true;

                _climbInfo.origin = transform.position;
                _climbInfo.origin.x = hit.point.x + hit.normal.x;

                _climbInfo.destination = hit.point + -hit.normal * 0.5f + Vector2.up;
                _climbInfo.destination.z = transform.position.z;
            }
        }

        return result;
    }
}
