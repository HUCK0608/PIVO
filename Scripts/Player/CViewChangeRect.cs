using System.Collections;
using UnityEngine;

public class CViewChangeRect : MonoBehaviour
{
    private Projector _projector;

    private bool _isOnIncreaseScaleXY;
    /// <summary>XY 스케일이 증가하고 있으면 true를 반환</summary>
    public bool IsOnIncreaseScaleXY { get { return _isOnIncreaseScaleXY; } }

    /// <summary>시점전환 상자 이펙트</summary>
    [SerializeField]
    private Transform _viewChangeRectEffect;

    private void Awake()
    {
        _projector = GetComponent<Projector>();

        _projector.enabled = true;
        gameObject.SetActive(false);
    }

    /// <summary>시점전환 상자 초기화</summary>
    private void ResetViewChangeRect()
    {
        // 상자관련 초기화
        transform.localScale = Vector3.zero;
        transform.position = CPlayerManager.Instance.RootObject3D.transform.position;

        // 프로젝터 초기화
        _projector.nearClipPlane = 0f;
        _projector.farClipPlane = 0.01f;
        _projector.aspectRatio = 0f;
        _projector.orthographicSize = 0f;

        // 이펙트 초기화
        _viewChangeRectEffect.position = transform.position;
        _viewChangeRectEffect.localScale = Vector3.one * 0.1f;
        _viewChangeRectEffect.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }

    /// <summary>상자의 XY 크기를 증가시키는 코루틴</summary>
    public IEnumerator IncreaseScaleXY()
    {
        _isOnIncreaseScaleXY = true;

        ResetViewChangeRect();

        float increaseValueX = CPlayerManager.Instance.Stat.MaxViewRectScaleX * CPlayerManager.Instance.Stat.ViewRectIncreaseXYRatePerSec;
        float increaseValueY = CPlayerManager.Instance.Stat.MaxViewRectScaleY * CPlayerManager.Instance.Stat.ViewRectIncreaseXYRatePerSec;

        float zero = 0f;

        while(!transform.localScale.x.Equals(CPlayerManager.Instance.Stat.MaxViewRectScaleX))
        {
            // 상자 스케일 조절
            Vector3 newScale = transform.localScale;

            newScale.x = Mathf.Clamp(newScale.x + increaseValueX * Time.deltaTime, zero, CPlayerManager.Instance.Stat.MaxViewRectScaleX);
            newScale.y = Mathf.Clamp(newScale.y + increaseValueY * Time.deltaTime, zero, CPlayerManager.Instance.Stat.MaxViewRectScaleY);

            transform.localScale = newScale;

            // 프로젝터 조절
            _projector.orthographicSize = transform.localScale.y * 0.5f;
            _projector.aspectRatio = transform.localScale.x / (2f * _projector.orthographicSize);

            // 이펙트 조절
            Vector3 effectScale = transform.localScale * 0.1f;
            effectScale.z = effectScale.y;
            effectScale.y = 0.1f;
            _viewChangeRectEffect.localScale = effectScale;

            yield return null;
        }

        _isOnIncreaseScaleXY = false;
    }

    /// <summary>상자의 Z 크기를 증가시킴</summary>
    public void SetScaleZ(float value)
    {
        float valueAbs = Mathf.Abs(value);
        float valueSign = Mathf.Sign(value);
        float currentPositionZSign = Mathf.Sign(transform.position.z - CPlayerManager.Instance.RootObject3D.transform.position.z);

        bool isReverse = false;

        // 스케일 조절
        Vector3 newScale = transform.localScale;

        if (!valueSign.Equals(currentPositionZSign) && transform.localScale.z >= 1f)
        {
            newScale.z = 0f;
            isReverse = true;
        }
        else
            newScale.z = valueAbs;

        transform.localScale = Vector3.Lerp(transform.localScale, newScale, CPlayerManager.Instance.Stat.ViewRectIncreaseZRatePerSec);

        // 위치 조절
        Vector3 newPosition = CPlayerManager.Instance.RootObject3D.transform.position;

        if (isReverse)
            newPosition.z = newPosition.z + transform.localScale.z * 0.5f * currentPositionZSign;
        else
            newPosition.z = newPosition.z + transform.localScale.z * 0.5f * valueSign;

        transform.position = newPosition;

        // 프로젝터 조절
        _projector.nearClipPlane = transform.localScale.z * -0.5f;
        _projector.farClipPlane = transform.localScale.z * 0.5f;

        // 이펙트 조절
        _viewChangeRectEffect.transform.position = transform.position + Vector3.forward * transform.localScale.z * valueSign * 0.5f;
    }

    /// <summary>이펙트 활성화 설정</summary>
    public void SetEffectEnable(bool value)
    {
        _viewChangeRectEffect.gameObject.SetActive(value);
    }
}
