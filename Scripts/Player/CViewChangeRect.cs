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

    [Header("Programmer can edit")]
    /// <summary>이펙트</summary>
    [SerializeField]
    private Transform _effect = null;
    /// <summary>프로젝터</summary>
    [SerializeField]
    private Projector _projector = null;

    /// <summary>현재 Z 크기</summary>
    private float _currentScaleZ = 0f;
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
        if (_isOnIncreaseScaleXY)
        {
            StopCoroutine(_increaseScaleXYCoroutine);
            _isOnIncreaseScaleXY = false;
        }

        if (_isOnIncreaseScaleZ)
        {
            StopCoroutine(_increaseScaleZCoroutine);
            _isOnIncreaseScaleZ = false;
        }
    }

    /// <summary>XY 스케일이 커지는 로직</summary>
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

        _increaseScaleZCoroutine = StartCoroutine(IncreaseScaleZLogic());
    }

    /// <summary>Z 스케일이 커지는 로직</summary>
    private IEnumerator IncreaseScaleZLogic()
    {
        _isOnIncreaseScaleZ = true;

        // 시작 Z 크기를 결정
        float originZ = transform.position.z;
        float ceilOriginZ = Mathf.Ceil(originZ);
        _destinationScaleZ = Mathf.Approximately(ceilOriginZ % 2f, 1f) ? ceilOriginZ - originZ - 0.4f : ceilOriginZ - originZ + 0.6f;

        Vector3 originPosition = transform.position;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _destinationScaleZ += 2f;
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                _destinationScaleZ -= 2f;

            // 역방향 판별을 위해 부호를 구함
            float destinationScaleZSign = Mathf.Sign(_destinationScaleZ);
            float currentScaleZSign = Mathf.Sign(_currentScaleZ);
            bool isReverse = !Mathf.Approximately(destinationScaleZSign, currentScaleZSign);

            Vector3 viewChangeRectScale = transform.localScale;
            Vector3 viewChangeRectPosition = originPosition;

            // 역방향으로 증가해야 하는 경우
            if(isReverse)
            {

            }
            // 순방향으로 증가해야 하는 경우
            else
            {
                _currentScaleZ = Mathf.Lerp(_currentScaleZ, _destinationScaleZ, 0.3f);
                viewChangeRectScale.z = _currentScaleZ;
                transform.localScale = viewChangeRectScale;

                viewChangeRectPosition.z += _currentScaleZ * 0.5f;
                transform.position = viewChangeRectPosition;
            }

            // 프로젝터 조절
            _projector.nearClipPlane = transform.localScale.z * -0.5f;
            _projector.farClipPlane = transform.localScale.z * 0.5f;

            // 이펙트 조절
            _effect.position = originPosition + Vector3.forward * _currentScaleZ;

            yield return null;
        }

        _isOnIncreaseScaleZ = false;
    }

    ///// <summary>상자의 XY 크기를 증가시키는 코루틴</summary>
    //public IEnumerator IncreaseScaleXYLogic()
    //{
    //    _isOnIncreaseScaleXY = true;

    //    ResetViewChangeRect();

    //    float increaseValueX = CPlayerManager.Instance.Stat.MaxViewRectScaleX * CPlayerManager.Instance.Stat.ViewRectIncreaseXYRatePerSec;
    //    float increaseValueY = CPlayerManager.Instance.Stat.MaxViewRectScaleY * CPlayerManager.Instance.Stat.ViewRectIncreaseXYRatePerSec;

    //    float zero = 0f;

    //    while(!transform.localScale.x.Equals(CPlayerManager.Instance.Stat.MaxViewRectScaleX))
    //    {
    //        // 상자 스케일 조절
    //        Vector3 newScale = transform.localScale;

    //        newScale.x = Mathf.Clamp(newScale.x + increaseValueX * Time.deltaTime, zero, CPlayerManager.Instance.Stat.MaxViewRectScaleX);
    //        newScale.y = Mathf.Clamp(newScale.y + increaseValueY * Time.deltaTime, zero, CPlayerManager.Instance.Stat.MaxViewRectScaleY);

    //        transform.localScale = newScale;

    //        // 프로젝터 조절
    //        _projector.orthographicSize = transform.localScale.y * 0.5f;
    //        _projector.aspectRatio = transform.localScale.x / (2f * _projector.orthographicSize);

    //        // 이펙트 조절
    //        Vector3 effectScale = transform.localScale * 0.1f;
    //        effectScale.z = effectScale.y;
    //        effectScale.y = 0.1f;
    //        _viewChangeRectEffect.localScale = effectScale;

    //        yield return null;
    //    }

    //    float newScaleZ = Mathf.Ceil(transform.position.z);

    //    if (!(newScaleZ % 2).Equals(1f))
    //        newScaleZ += 1f;

    //    newScaleZ -= transform.position.z;

    //    while (transform.localScale.z <= newScaleZ - 0.1f)
    //    {
    //        SetScaleZ(newScaleZ);
    //        yield return null;
    //    }

    //    _currentScaleZ = newScaleZ;

    //    _isOnIncreaseScaleXY = false;
    //}






    ///// <summary>프로젝터</summary>
    //private Projector _projector;

    //private float _currentScaleZ = 0f;
    ///// <summary>현재 상자의 Z 크기</summary>
    //public float CurrentScaleZ { get { return _currentScaleZ; } }

    //private bool _isOnIncreaseScaleXY = false;
    ///// <summary>XY 스케일이 증가하고 있으면 true를 반환</summary>
    //public bool IsOnIncreaseScaleXY { get { return _isOnIncreaseScaleXY; } }

    ///// <summary>Z 스케일이 조정중인지 여부</summary>
    //private bool _isOnSetScaleZ = false;

    ///// <summary>시점전환 상자 이펙트</summary>
    //[SerializeField]
    //private Transform _viewChangeRectEffect = null;

    ///// <summary>상자의 Z크기 조절 딜레이</summary>
    //[SerializeField]
    //private float _setScaleZControllerDelay = 0f;

    //private void Awake()
    //{
    //    _projector = GetComponent<Projector>();

    //    _projector.enabled = true;
    //    gameObject.SetActive(false);
    //}

    ///// <summary>시점전환 상자 초기화</summary>
    //private void ResetViewChangeRect()
    //{
    //    // 상자관련 초기화
    //    transform.localScale = Vector3.zero;
    //    transform.position = CPlayerManager.Instance.RootObject3D.transform.position;

    //    // 프로젝터 초기화
    //    _projector.nearClipPlane = 0f;
    //    _projector.farClipPlane = 0.01f;
    //    _projector.aspectRatio = 0f;
    //    _projector.orthographicSize = 0f;

    //    // 이펙트 초기화
    //    _viewChangeRectEffect.position = transform.position;
    //    _viewChangeRectEffect.localScale = Vector3.one * 0.1f;
    //    _viewChangeRectEffect.gameObject.SetActive(true);

    //    gameObject.SetActive(true);
    //}



    ///// <summary>상자의 Z 크기를 증가시킴</summary>
    //public void IncreaseScaleZ()
    //{
    //    if(!_isOnSetScaleZ)
    //        StartCoroutine(IncreaseScaleZLogic());
    //}

    ///// <summary>상자의 Z 크기를 증가시키는 코루틴</summary>
    //private IEnumerator IncreaseScaleZLogic()
    //{
    //    _isOnSetScaleZ = true;

    //    if (Mathf.Abs(_currentScaleZ + 2f) <= CPlayerManager.Instance.Stat.MaxViewRectScaleZ)
    //    {
    //        _currentScaleZ += 2f;

    //        float currentScaleZSign = Mathf.Sign(_currentScaleZ);

    //        float destinationScaleZ = _currentScaleZ;
    //        if (currentScaleZSign.Equals(-1f))
    //            destinationScaleZ = _currentScaleZ + 0.2f;

    //        float hopeScaleZ = _currentScaleZ - 0.1f * currentScaleZSign;

    //        float addTime = 0f;

    //        while (true)
    //        {
    //            SetScaleZ(destinationScaleZ);
    //            addTime += Time.deltaTime;

    //            float currentDirectionZ = Mathf.Sign(transform.position.z - CPlayerManager.Instance.RootObject3D.transform.position.z);

    //            if (transform.localScale.z * currentDirectionZ >= hopeScaleZ)
    //                break;

    //            yield return null;
    //        }

    //        if (addTime <= _setScaleZControllerDelay)
    //            yield return new WaitForSeconds(_setScaleZControllerDelay - addTime);
    //    }

    //    _isOnSetScaleZ = false;
    //}

    ///// <summary>상자의 Z 크기를 감소시킴</summary>
    //public void DecreaseScaleZ()
    //{
    //    if (!_isOnSetScaleZ)
    //        StartCoroutine(DecreaseScaleZLogic());
    //}

    ///// <summary>상자의 Z 크기를 감소시키는 코루틴</summary>
    //private IEnumerator DecreaseScaleZLogic()
    //{
    //    _isOnSetScaleZ = true;

    //    if (Mathf.Abs(_currentScaleZ - 2f) <= CPlayerManager.Instance.Stat.MaxViewRectScaleZ)
    //    {
    //        _currentScaleZ -= 2f;

    //        float currentScaleZSign = Mathf.Sign(_currentScaleZ);

    //        float destinationScaleZ = _currentScaleZ;
    //        if (currentScaleZSign.Equals(1f))
    //            destinationScaleZ = _currentScaleZ - 0.2f;

    //        float hopeScaleZ = _currentScaleZ + 0.1f * -currentScaleZSign;

    //        float addTime = 0f;

    //        while (true)
    //        {
    //            SetScaleZ(destinationScaleZ);
    //            addTime += Time.deltaTime;

    //            float currentDirectionZ = Mathf.Sign(transform.position.z - CPlayerManager.Instance.RootObject3D.transform.position.z);

    //            if (transform.localScale.z * currentDirectionZ <= hopeScaleZ)
    //                break;

    //            yield return null;
    //        }

    //        if (addTime <= _setScaleZControllerDelay)
    //            yield return new WaitForSeconds(_setScaleZControllerDelay - addTime);
    //    }

    //    _isOnSetScaleZ = false;
    //}

    ///// <summary>상자의 Z 크기를 증가시킴</summary>
    //private void SetScaleZ(float value)
    //{
    //    float valueAbs = Mathf.Abs(value);
    //    float valueSign = Mathf.Sign(value);
    //    float currentPositionZSign = Mathf.Sign(transform.position.z - CPlayerManager.Instance.RootObject3D.transform.position.z);

    //    bool isReverse = false;

    //    // 스케일 조절
    //    Vector3 newScale = transform.localScale;

    //    if (!valueSign.Equals(currentPositionZSign) && transform.localScale.z >= 0.05f)
    //    {
    //        newScale.z = 0f;
    //        isReverse = true;
    //    }
    //    else
    //        newScale.z = valueAbs;

    //    if(isReverse)
    //        transform.localScale = Vector3.Lerp(transform.localScale, newScale, 0.4f);
    //    else
    //        transform.localScale = Vector3.Lerp(transform.localScale, newScale, CPlayerManager.Instance.Stat.ViewRectIncreaseZRatePerSec);

    //    // 위치 조절
    //    Vector3 newPosition = CPlayerManager.Instance.RootObject3D.transform.position;

    //    if (isReverse)
    //        newPosition.z = newPosition.z + transform.localScale.z * 0.5f * currentPositionZSign;
    //    else
    //        newPosition.z = newPosition.z + transform.localScale.z * 0.5f * valueSign;

    //    transform.position = newPosition;

    //    // 프로젝터 조절
    //    _projector.nearClipPlane = transform.localScale.z * -0.5f;
    //    _projector.farClipPlane = transform.localScale.z * 0.5f;

    //    // 이펙트 조절
    //    _viewChangeRectEffect.transform.position = (transform.position + Vector3.forward * transform.localScale.z * currentPositionZSign * 0.5f) + Vector3.forward * -currentPositionZSign * 0.3f;
    //}

    ///// <summary>상자의 Z 크기를 증가시키는 것을 멈춤</summary>
    //public void StopSetScaleZ()
    //{
    //    StopAllCoroutines();
    //    _isOnSetScaleZ = false;
    //}

    ///// <summary>이펙트 활성화 설정</summary>
    //public void SetEffectEnable(bool value)
    //{
    //    _viewChangeRectEffect.gameObject.SetActive(value);
    //}
}
