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

    private int _currentKeyCount = 4;

    /// <summary>패턴을 채우고 있는중인지 여부</summary>
    private bool _isPatternFill = false;

    /// <summary>코루틴에서 쓰이는 WU</summary>
    private WaitUntil _cameraFocusingToDoorWU;
    private WaitUntil _cameraFocusingToPlayerWU;

    private void Start()
    {
        // 획득한 열쇠 비활성화
        for (int i = 3; i >= _currentKeyCount; i--)
            _landingPoints[i].SetActive(false);

        // 패턴 초기 수치설정
        if(_currentKeyCount > 1)
            _patternMeshRenderer.material.SetFloat(CString.PatternFill, _patternFill[_currentKeyCount - 1]);

        // WU 초기화
        _cameraFocusingToDoorWU = new WaitUntil(() => Vector3.Distance(transform.position, CCameraController.Instance.transform.position) <= 1f);
        _cameraFocusingToPlayerWU = new WaitUntil(() => Vector3.Distance(CCameraController.Instance.transform.position, CPlayerManager.Instance.RootObject3D.transform.position) < 0.1f);
    }

    /// <summary>키 등록</summary>
    public void RegisterKey() { _currentKeyCount--; }

    /// <summary>키 습득</summary>
    public void GetKey()
    {
        if (!_isPatternFill)
        {
            if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
                StartCoroutine(GetKey2DLogic());
            else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
                StartCoroutine(GetKey3DLogic());
        }
    }

    /// <summary>패턴 채우기 2D 로직</summary>
    private IEnumerator GetKey2DLogic()
    {
        // 열쇠 활성화 및 패턴 채우기
        _landingPoints[_currentKeyCount++].SetActive(true);
        _patternMeshRenderer.material.SetFloat(CString.PatternFill, _patternFill[_currentKeyCount - 1]);

        // 문열기
        if (_currentKeyCount == 4)
        {
            // 카메라 문에 포커싱하기
            yield return new WaitUntil(() => CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D));
            CPlayerManager.Instance.IsCanOperation = false;
            CCameraController.Instance.IsLerpMove = true;
            CCameraController.Instance.Target = transform;
            yield return _cameraFocusingToDoorWU;

            StartCoroutine(OpenDoorLogic());
        }
    }

    /// <summary>패턴 채우기 3D 로직</summary>
    private IEnumerator GetKey3DLogic()
    {
        // 카메라 포커싱이 될 때까지 대기
        yield return _cameraFocusingToDoorWU;

        // 열쇠 활성화
        _landingPoints[_currentKeyCount++].SetActive(true);

        // 수치 설정
        float goalPatternFill = _patternFill[_currentKeyCount - 1];
        float patternFillSpeed = (goalPatternFill - _patternMeshRenderer.material.GetFloat(CString.PatternFill)) / _patternFillTime;

        // 패턴채우기
        while (true)
        {
            float nextPatternFill = Mathf.Clamp(_patternMeshRenderer.material.GetFloat(CString.PatternFill) + patternFillSpeed * Time.deltaTime, 0.91f, goalPatternFill);
            _patternMeshRenderer.material.SetFloat(CString.PatternFill, nextPatternFill);

            if (nextPatternFill.Equals(goalPatternFill))
                break;

            yield return null;
        }

        // 열쇠를 다 모았으면 문열기
        if (_currentKeyCount == 4)
            StartCoroutine(OpenDoorLogic());
        else
        {
            // 카메라 원래상태로 돌리기
            CCameraController.Instance.Target = CPlayerManager.Instance.RootObject3D.transform;
            yield return _cameraFocusingToPlayerWU;
            CCameraController.Instance.IsLerpMove = false;
            CPlayerManager.Instance.IsCanOperation = true;
        }
    }

    /// <summary>문열기 로직</summary>
    private IEnumerator OpenDoorLogic()
    {
        Vector3 downPosition = Vector3.down * 8f;

        // 문열기
        while(!_door.localPosition.Equals(downPosition))
        {
            if(!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
            {
                yield return null;
                continue;
            }

            _door.localPosition = Vector3.MoveTowards(_door.localPosition, downPosition, _doorSpeed * Time.deltaTime);

            yield return null;
        }

        // 카메라 원래상태로 돌리기
        CCameraController.Instance.Target = CPlayerManager.Instance.RootObject3D.transform;
        yield return _cameraFocusingToPlayerWU;
        CCameraController.Instance.IsLerpMove = false;
        CPlayerManager.Instance.IsCanOperation = true;

        // 문 비활성화
        _door.gameObject.SetActive(false);
    }
}
