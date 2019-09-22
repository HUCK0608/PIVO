using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerState3D { Idle, Move, Falling, ViewChangeInit, ViewChangeIdle, Climb, Holding, Dead, PushInit, PushIdle, PushEnd, PutInit, PutIdle, PutEnd }

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

    private Animator _animator;
    /// <summary>애니메이터</summary>
    public Animator Animator { get { return _animator; } }
    /// <summary>애니메이터 파라미터 이름</summary>
    private static string _animParameterPath = "CurrentState";

    [SerializeField]
    private CViewChangeRect _viewChangeRect = null;
    /// <summary>시점전환 상자</summary>
    public CViewChangeRect ViewChangeRect { get { return _viewChangeRect; } }

    /// <summary>지면 확인 지점들</summary>
    [SerializeField]
    private Transform[] _groundCheckPoints = null;
    /// <summary>지면 확인 지점 개수</summary>
    private int _groundCheckPointCount = 4;

    /// <summary>기어오를 지점을 탐지하는 지점</summary>
    [SerializeField]
    private Transform[] _climbDetectionPoints = null;
    /// <summary>기어오를 지점을 탐지하는 지점 개수</summary>
    private int _climbDetectionPointCount = 3;

    public struct SClimbInfo
    {
        public int aniNumber;
        public Vector3 origin;
        public Vector3 destination;
        public Vector3 direction;
    };
    private SClimbInfo _climbInfo;
    /// <summary>기어오르기에 대한 시작점, 도착점, 방향을 담고 있는 구조체</summary>
    public SClimbInfo ClimbInfo { get { return _climbInfo; } }

    /// <summary>플레이어를 무시하는 레이어 마스크</summary>
    private int _playerIgnoreLayerMask;

    private bool _isUseGravity = true;
    /// <summary>중력 적용 여부</summary>
    public bool IsUseGravity { get { return _isUseGravity; } set { _isUseGravity = value; } }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _playerIgnoreLayerMask = (-1) - (CLayer.Player.LeftShiftToOne() | CLayer.BackgroundObject.LeftShiftToOne());

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
        if (_currentState.Equals(EPlayerState3D.Dead))
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

    /// <summary>땅 위일 경우 true를 반환</summary>
    public bool IsGrounded()
    {
        bool isGrounded = false;

        for(int i = 0; i < _groundCheckPointCount; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(_groundCheckPoints[i].position, Vector3.down, out hit, 0.15f, _playerIgnoreLayerMask))
            {
                isGrounded = true;

                // 최근 위치 저장
                if (hit.transform.parent.parent.name.Equals(CString.TileOut))
                    CPlayerManager.Instance.LastGroundPosition = hit.transform.position + Vector3.up;

                break;
            }
        }

        return isGrounded;
    }

    /// <summary>중력 계산</summary>
    private void CalcGravity(ref Vector3 velocity)
    {
        bool isApplyGravity = true;

        if (!_isUseGravity || IsGrounded())
            isApplyGravity = false;

        if (isApplyGravity)
            velocity.y = _rigidBody.velocity.y + CPlayerManager.Instance.Stat.Gravity;
        else
            velocity.y = 0f;
    }

    /// <summary>방향 이동</summary>
    public void Move(Vector3 direction, bool isAirMove = false)
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            return;

        float moveSpeed = isAirMove ? CPlayerManager.Instance.Stat.AirMoveSpeed : CPlayerManager.Instance.Stat.MoveSpeed;
        Vector3 velocity = direction * moveSpeed;
        CalcGravity(ref velocity);

        _rigidBody.velocity = velocity;

        if(!direction.Equals(Vector3.zero))
            RotationSlerp(direction);

        if (transform.position.y <= CPlayerManager.Instance.Stat.KillYVolume)
        {
            CPlayerManager.Instance.Stat.Hp -= 1;
            _rigidBody.velocity = Vector3.zero;
            transform.position = CPlayerManager.Instance.LastGroundPosition;
        }
    }

    /// <summary>키보드 입력 이동</summary>
    public void Move(float vertical, float horizontal, bool isAirMove = false)
    {
        Vector3 direction = (Vector3.right + Vector3.forward) * vertical + (Vector3.right + Vector3.back) * horizontal;

        Move(direction.normalized, isAirMove);
    }

    /// <summary>정지</summary>
    public void Stop()
    {
        _rigidBody.velocity = Vector3.zero;
        ChangeState(EPlayerState3D.Idle);
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

    /// <summary>기어오를 수 있으면 true를 반환</summary>
    public bool IsCanClimb()
    {
        bool result = false;

        RaycastHit hit;

        // 플레이어 전방 부채꼴 주위에 올라갈 수 있는 벽이 있는지 확인
        for (int i = 0; i < _climbDetectionPointCount; i++)
        {
            if(Physics.Raycast(_climbDetectionPoints[i].position, _climbDetectionPoints[i].forward, out hit, 1f, _playerIgnoreLayerMask))
            {
                // 올라갈 수 있는 벽이 있으면 올라갈 위치에 장애물이 있는지 확인
                if(Physics.Raycast(_climbDetectionPoints[i].position, -hit.normal, out hit, Mathf.Infinity, _playerIgnoreLayerMask))
                {
                    Vector3 center = hit.point + -hit.normal;

                    if (!Physics.BoxCast(center, Vector3.one * 0.75f, Vector3.up, Quaternion.LookRotation(Vector3.up), 3f))
                    {
                        result = true;

                        _climbInfo.aniNumber = UnityEngine.Random.Range(0f, 100f) <= CPlayerManager.Instance.Stat.Climb1Percent ? 0 : 1;
                        _climbInfo.origin = hit.point + hit.normal;
                        _climbInfo.origin.y = transform.position.y;
                        _climbInfo.direction = -hit.normal;

                        if (_climbInfo.aniNumber.Equals(0))
                            _climbInfo.destination = hit.point + -hit.normal * 0.69f + Vector3.up * 1.025f;
                        else
                            _climbInfo.destination = hit.point + -hit.normal * 0.71f + Vector3.up;
                    }
                }

                break;
            }
        }

        return result;
    }
}
