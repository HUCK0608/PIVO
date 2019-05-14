using System.Collections;
using UnityEngine;

public class CViewChangeRect : MonoBehaviour
{
    [Header("Anyone can edit")]
    /// <summary>최대 스케일</summary>
    [SerializeField]
    private Vector3 _maxScale = Vector3.zero;
    /// <summary>XY 최대 스케일까지 증가하는데 걸리는 시간</summary>
    [SerializeField]
    private Vector2 _timeToIncraseToMaxScaleXY = Vector2.zero;
    /// <summary>키입력이 유지되는 최소 시간</summary>
    [SerializeField]
    private float _keepInputKeyMinTime = 0f;

    [Header("Programmer can edit")]
    /// <summary>이펙트</summary>
    [SerializeField]
    private Transform _effect = null;
    /// <summary>프로젝터</summary>
    [SerializeField]
    private Projector _projector = null;

    private float _currentScaleZ = 0f;
    /// <summary>현재 Z 크기</summary>
    public float CurrentScaleZ { get { return _currentScaleZ; } }
    /// <summary>목표 Z 크기</summary>
    private float _destinationScaleZ = 0f;

    private bool _isOnIncreaseScaleXY = false;
    /// <summary>X,Y 크기가 증가 중이면 true를 반환</summary>
    public bool IsOnIncreaseScaleXY { get { return _isOnIncreaseScaleXY; } }
    private bool _isOnIncreaseScaleZ = false;
    /// <summary>Z 크기가 증가 중이면 true를 반환</summary>
    public bool IsOnIncreaseScaleZ { get { return _isOnIncreaseScaleZ; } }

    // 코루틴 관련 변수
    private Coroutine _increaseScaleXYCoroutine = null;
    private Coroutine _increaseScaleZCoroutine = null;

    private void Awake()
    {
        _projector.enabled = true;

        gameObject.SetActive(false);
    }

    /// <summary>상자 초기화</summary>
    private void InitViewChangeRect()
    {
        transform.localScale = Vector3.zero;
        transform.position = CPlayerManager.Instance.RootObject3D.transform.position;

        _currentScaleZ = 0f;
        _destinationScaleZ = 0f;
    }

    /// <summary>프로젝터 초기화</summary>
    private void InitProjector()
    {
        _projector.nearClipPlane = 0f;
        _projector.farClipPlane = 0.01f;
        _projector.aspectRatio = 0f;
        _projector.orthographicSize = 0f;
    }

    /// <summary>이펙트 초기화</summary>
    private void InitEffect()
    {
        _effect.position = transform.position;
        _effect.localScale = Vector3.one * 0.1f;
        _effect.gameObject.SetActive(true);
    }

    /// <summary>시점전환 상자 로직 실행</summary>
    public void StartViewChangeRectLogic()
    {
        gameObject.SetActive(true);
        _increaseScaleXYCoroutine = StartCoroutine(IncreaseScaleXYLogic());
    }

    /// <summary>시점전환 상자 로직 종료</summary>
    public void StopViewChangeRectLogic()
    {
        // X, Y 크기 증가 코루틴 종료
        if (_isOnIncreaseScaleXY)
        {
            StopCoroutine(_increaseScaleXYCoroutine);
            _isOnIncreaseScaleXY = false;
        }

        // Z 크기 증가 코루틴 종료
        if (_isOnIncreaseScaleZ)
        {
            StopCoroutine(_increaseScaleZCoroutine);
            _isOnIncreaseScaleZ = false;
        }

        // 이펙트 비활성화
        _effect.gameObject.SetActive(false);
    }

    /// <summary>XY 크기가 커지는 로직</summary>
    private IEnumerator IncreaseScaleXYLogic()
    {
        _isOnIncreaseScaleXY = true;

        InitViewChangeRect();
        InitProjector();
        InitEffect();

        // X, Y 스케일 프레임 당 증가량
        float incrementPerFrameScaleX = _maxScale.x * (1.0f / _timeToIncraseToMaxScaleXY.x);
        float incrementPerFrameScaleY = _maxScale.y * (1.0f / _timeToIncraseToMaxScaleXY.y);

        while(true)
        {
            // 스케일 조절
            Vector2 viewChangeRectScale = transform.localScale;
            viewChangeRectScale.x = Mathf.Clamp(viewChangeRectScale.x + incrementPerFrameScaleX * Time.deltaTime, 0f, _maxScale.x);
            viewChangeRectScale.y = Mathf.Clamp(viewChangeRectScale.y + incrementPerFrameScaleY * Time.deltaTime, 0f, _maxScale.y);
            transform.localScale = viewChangeRectScale;

            // 프로젝터 조절
            _projector.orthographicSize = transform.localScale.y * 0.5f;
            _projector.aspectRatio = transform.localScale.x / (2f * _projector.orthographicSize);

            // 이펙트 조절
            Vector3 effectScale = transform.localScale * 0.1f;
            effectScale.z = effectScale.y;
            effectScale.y = 0.1f;
            _effect.localScale = effectScale;

            if (transform.localScale.x.Equals(_maxScale.x))
                break;

            yield return null;
        }

        _isOnIncreaseScaleXY = false;

        _increaseScaleZCoroutine = StartCoroutine(SetScaleZLogic());
        StartCoroutine(SetScaleZInputLogic());
    }

    /// <summary>Z 크기를 조절하는 로직</summary>
    private IEnumerator SetScaleZLogic()
    {
        _isOnIncreaseScaleZ = true;

        // 시작 Z 크기를 결정
        float originZ = transform.position.z;
        float ceilOriginZ = Mathf.Ceil(originZ);
        _destinationScaleZ = Mathf.Approximately(ceilOriginZ % 2f, 1f) ? ceilOriginZ - originZ - 0.4f : ceilOriginZ - originZ + 0.6f;

        _currentScaleZ = 0f;
        Vector3 originPosition = transform.position;

        while (true)
        {
            // 역방향 판별을 위해 부호를 구함
            float destinationScaleZSign = Mathf.Sign(_destinationScaleZ);
            float currentScaleZSign = Mathf.Sign(_currentScaleZ);
            bool isReverse = !Mathf.Approximately(destinationScaleZSign, currentScaleZSign);

            // 임시 변수
            Vector3 viewChangeRectScale = transform.localScale;
            Vector3 viewChangeRectPosition = originPosition;

            // 역방향으로 증가해야 하는 경우
            if (isReverse)
            {
                if ((_currentScaleZ <= 0.01f && _currentScaleZ >= 0f) || (_currentScaleZ <= 0f && _currentScaleZ >= -0.01f))
                {
                    _currentScaleZ = 0.01f * destinationScaleZSign;
                    _destinationScaleZ += 0.8f * destinationScaleZSign * -1f;
                }

                _currentScaleZ = Mathf.Lerp(_currentScaleZ, 0f, 0.7f);
            }
            // 순방향으로 증가해야 하는 경우
            else
            {
                _currentScaleZ = Mathf.Lerp(_currentScaleZ, _destinationScaleZ, 0.3f);
            }

            // 상자 크기 조절
            viewChangeRectScale.z = _currentScaleZ * Mathf.Sign(_currentScaleZ);
            transform.localScale = viewChangeRectScale;

            // 상자 이동
            viewChangeRectPosition.z += _currentScaleZ * 0.5f;
            transform.position = viewChangeRectPosition;

            // 프로젝터 조절
            _projector.nearClipPlane = transform.localScale.z * -0.5f;
            _projector.farClipPlane = transform.localScale.z * 0.5f;

            // 이펙트 조절
            _effect.position = originPosition + Vector3.forward * _currentScaleZ;

            yield return null;
        }
    }
    
    /// <summary>Z 크기 조절 입력 로직</summary>
    private IEnumerator SetScaleZInputLogic()
    {
        float addTime = 0f;

        bool isFirstInput = false;
        bool isKeepInputIncreaseKey = false, isKeepInputDecreaseKey = false;

        while(_isOnIncreaseScaleZ)
        {
            // 크기 증가 키 입력 관리
            if (Input.GetKeyDown(CKeyManager.ViewRectScaleAdjustKey1) || Input.GetKeyDown(CKeyManager.AnotherViewRectScaleAdjustKey1))
            {
                isFirstInput = true;
                addTime = 0f;
                isKeepInputIncreaseKey = true;

                if (isKeepInputDecreaseKey)
                    isKeepInputDecreaseKey = false;
            }
            else if(Input.GetKeyUp(CKeyManager.ViewRectScaleAdjustKey1) || Input.GetKeyUp(CKeyManager.AnotherViewRectScaleAdjustKey1))
            {
                addTime = 0f;
                isKeepInputIncreaseKey = false;
            }

            // 크기 감소 키 입력 관리
            if (Input.GetKeyDown(CKeyManager.ViewRectScaleAdjustKey2) || Input.GetKeyDown(CKeyManager.AnotherViewRectScaleAdjustKey2))
            {
                isFirstInput = true;
                addTime = 0f;
                isKeepInputDecreaseKey = true;

                if (isKeepInputIncreaseKey)
                    isKeepInputIncreaseKey = false;
            }
            else if (Input.GetKeyUp(CKeyManager.ViewRectScaleAdjustKey2) || Input.GetKeyUp(CKeyManager.AnotherViewRectScaleAdjustKey2))
            {
                addTime = 0f;
                isKeepInputDecreaseKey = false;
            }

            // 입력이 유지되는 시간 누적
            if (isKeepInputIncreaseKey || isKeepInputDecreaseKey)
                addTime += Time.deltaTime;

            // Z 크기 조절 적용
            float temp = _destinationScaleZ;

            // 첫 입력
            if(isFirstInput)
            {
                if (isKeepInputIncreaseKey)
                    temp += 2f;
                else if (isKeepInputDecreaseKey)
                    temp -= 2f;

                isFirstInput = false;
            }
            // 입력 유지
            else if(addTime >= _keepInputKeyMinTime)
            {
                if (isKeepInputIncreaseKey)
                    temp += 2f;
                else if (isKeepInputDecreaseKey)
                    temp -= 2f;
            }

            // 제한된 범위 판별
            if (temp >= -_maxScale.z && temp <= _maxScale.z)
                _destinationScaleZ = temp;

            yield return null;
        }
    }
}
