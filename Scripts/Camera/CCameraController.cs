using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CCameraController : MonoBehaviour
{
    private static CCameraController _instance;
    /// <summary>카메라 컨트롤러 싱글턴</summary>
    public static CCameraController Instance { get { return _instance; } }

    private static string _animParameter = "Is3D";

    private Animator _animator;

    private Transform _target = null;
    /// <summary>카메라의 타겟</summary>
    public Transform Target { get { return _target; } set { _target = value; } }

    private bool _isHoldingToTarget = true;
    /// <summary>카메라가 타겟에게 고정되었는지 여부</summary>
    public bool IsHoldingToTarget { set { _isHoldingToTarget = value; } }

    private bool _isLerpMove = false;
    /// <summary>카메라가 lerp 이동을 하는지 여부</summary>
    public bool IsLerpMove { set { _isLerpMove = value; } }

    private bool _isOnMovingWork = false;
    /// <summary>무빙워크 중일시 true를 반환</summary>
    public bool IsOnMovingWork { get { return _isOnMovingWork; } }

    /// <summary>각 카메라</summary>
    [SerializeField]
    private Camera _mainCamera = null, _skyBoxCamera = null;

    /// <summary>카메라 흔들림 세기</summary>
    [SerializeField]
    private float _cameraShakingStrength = 0f;
    /// <summary>카메라의 흔들림 효과 시간</summary>
    [SerializeField]
    private float _cameraShakingTime = 0f;
    /// <summary>카메라 흔들림 중일경우 true를 반환</summary>
    private bool _isOnCameraShaking = false;

    /// <summary>글로벌 포그</summary>
    private GlobalFog _globalFog = null;
    /// <summary>시작 포그 높이</summary>
    private float _startFogHeight = 0f;

    /// <summary>마지막 2D 위치</summary>
    private Vector3 _last2DPosition;
    /// <summary>마지막 2D 위치로 이동하는지 여부</summary>
    private bool _isMoveLast2DPosition = false;

    private void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();

        _globalFog = GetComponentInChildren<GlobalFog>();
        _globalFog.heightDensity = 2.63f;
        _startFogHeight = _globalFog.height;
    }

    private void Start()
    {
        _target = CPlayerManager.Instance.RootObject3D.transform;
    }

    private void LateUpdate()
    {
        if (_isLerpMove)
            transform.position = Vector3.Lerp(transform.position, _target.position, 0.07f);
        else if (_isHoldingToTarget && !_isOnCameraShaking && !_isMoveLast2DPosition)
            transform.position = _target.position;
        

        _globalFog.height = transform.position.y + _startFogHeight;
    }

    /// <summary>2D 무빙워크 실행</summary>
    public void Change2D()
    {
        if (_isOnCameraShaking)
        {
            StopAllCoroutines();
            _isOnCameraShaking = false;
        }

        _isHoldingToTarget = false;
        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, false);
    }

    /// <summary>3D 무빙워크 실행</summary>
    public void Change3D()
    {
        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, true);

        _isHoldingToTarget = true;
        _isMoveLast2DPosition = false;
        _last2DPosition = transform.position;
    }

    /// <summary>무빙워크가 끝날 경우 변수 설정</summary>
    public void CompleteMovingWork()
    {
        _isOnMovingWork = false;
    }

    /// <summary>카메라 흔들림 작동</summary>
    public void OnCamerShaking()
    {
        if (!_isOnCameraShaking)
            StartCoroutine(ShakingCameraLogic());
    }

    private IEnumerator ShakingCameraLogic()
    {
        _isOnCameraShaking = true;

        float addTime = 0f;

        while(addTime <= _cameraShakingTime)
        {
            Vector3 randomPosition = Random.insideUnitCircle * _cameraShakingStrength;
            transform.position = randomPosition + _target.position;

            addTime += Time.deltaTime;

            yield return null;
        }

        _isOnCameraShaking = false;
    }

    /// <summary>카메라를 마지막 2D 위치로 이동</summary>
    public void MoveLast2DPosition()
    {
        _isMoveLast2DPosition = true;
        transform.position = _last2DPosition;
    }

    /// <summary>목표 디스플레이 설정</summary>
    public void SetTargetDisplay(int targetValue)
    {
        _mainCamera.targetDisplay = targetValue;
        _skyBoxCamera.targetDisplay = targetValue;
    }
}
