using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoor : MonoBehaviour
{
    /// <summary>착지 지점 리스트</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private List<GameObject> _landingPoints = null;

    /// <summary>문</summary>
    [SerializeField]
    private Transform _door = null;

    /// <summary>패턴 머테리얼</summary>
    [SerializeField]
    private MeshRenderer _patternMeshRenderer = null;
    /// <summary>패턴 채우기 수치</summary>
    private float[] _patternFill = new float[] { 1.0f, 1.6f, 2f, 6.8f };

    /// <summary>문열기 속도</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private float _doorSpeed = 0f;

    /// <summary>패턴이 차는 시간</summary>
    [SerializeField]
    private float _patternFillTime = 0f;

    private int _nextLandingPointIndex = 4;
    private int _currentKeyCount = 4;

    /// <summary>패턴을 채우고 있는중인지 여부</summary>
    private bool _isPatternFill = false;

    private void Start()
    {
        for (int i = 3; i >= _currentKeyCount; i--, _nextLandingPointIndex--)
            _landingPoints[i].SetActive(false);

        _patternMeshRenderer.material.SetFloat(CString.PatternFill, _patternFill[_currentKeyCount - 1]);
    }

    /// <summary>키 습득</summary>
    public void GetKey()
    {
        _landingPoints[_currentKeyCount++].SetActive(true);

        if(!_isPatternFill)
            StartCoroutine(PatternFillLogic());
    }

    /// <summary>패턴 채우기 로직</summary>
    private IEnumerator PatternFillLogic()
    {
        _isPatternFill = true;

        while(true)
        {
            float goalPatternFill = _patternFill[_currentKeyCount - 1];
            float currentPatternFill = _patternMeshRenderer.material.GetFloat(CString.PatternFill);
            float patternFillSpeed = currentPatternFill < 1.6f ? 0.6f / _patternFillTime : currentPatternFill < 2f ? 0.4f / _patternFillTime : 4.8f / _patternFillTime;
            float nextPatternFill = Mathf.Clamp(_patternMeshRenderer.material.GetFloat(CString.PatternFill) + patternFillSpeed * Time.deltaTime, 1f, goalPatternFill);
            _patternMeshRenderer.material.SetFloat(CString.PatternFill, Mathf.Clamp(nextPatternFill, 1f, goalPatternFill));

            if (nextPatternFill.Equals(goalPatternFill))
                break;

            yield return null;
        }

        _isPatternFill = false;

        if (_currentKeyCount == 4)
            StartCoroutine(OpenDoorLogic());
    }

    /// <summary>문열기 로직</summary>
    private IEnumerator OpenDoorLogic()
    {
        Vector3 downPosition = Vector3.down * 8f;

        while(!_door.localPosition.Equals(downPosition))
        {
            _door.localPosition = Vector3.MoveTowards(_door.localPosition, downPosition, _doorSpeed * Time.deltaTime);

            yield return null;
        }

        _door.gameObject.SetActive(false);
    }

    /// <summary>키 등록</summary>
    public void RegisterKey() { _currentKeyCount--; }
    /// <summary>착지 위치 반환</summary>
    public Vector3 GetLandingPoint() { return _landingPoints[_nextLandingPointIndex++].transform.position; }
}
