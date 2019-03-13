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

    private Transform _target;
    public Transform Target { set { _target = value; } }

    private bool _isOnMovingWork = false;
    /// <summary>무빙워크 중일시 true를 반환</summary>
    public bool IsOnMovingWork { get { return _isOnMovingWork; } }

    /// <summary>카메라 흔들림 세기</summary>
    [SerializeField]
    private float _cameraShakingStrength = 0f;
    /// <summary>카메라의 흔들림 효과 시간</summary>
    [SerializeField]
    private float _cameraShakingTime = 0f;
    /// <summary>카메라 흔들림 중일경우 true를 반환</summary>
    private bool _isOnCameraShaking = false;

    [SerializeField]
    private GlobalFog _globalFog;

    private void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _target = CPlayerManager.Instance.RootObject3D.transform;
    }

    private void LateUpdate()
    {
        if (!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D) && !_isOnCameraShaking)
            transform.position = _target.transform.position;

        //_globalFog.height = transform.position.y + 2f;
    }

    /// <summary>2D 무빙워크 실행</summary>
    public void Change2D()
    {
        if (_isOnCameraShaking)
            StopAllCoroutines();

        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, false);
    }

    /// <summary>3D 무빙워크 실행</summary>
    public void Change3D()
    {
        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, true);
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

        Vector3 defaultPosition = transform.position;
        float addTime = 0f;

        while(addTime <= _cameraShakingTime)
        {
            Vector3 randomPosition = Random.insideUnitCircle * _cameraShakingStrength;
            transform.position = randomPosition + defaultPosition;

            addTime += Time.deltaTime;

            yield return null;
        }

        transform.position = defaultPosition;
        _isOnCameraShaking = false;
    }
}
