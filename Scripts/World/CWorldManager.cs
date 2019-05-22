using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWorldState { View2D, View3D, Changing }
public class CWorldManager : MonoBehaviour
{
    private static CWorldManager _instance;
    /// <summary>월드 매니저 싱글턴</summary>
    public static CWorldManager Instance { get { return _instance; } }

    private EWorldState _currentWorldState = EWorldState.View3D;
    /// <summary>현재 월드 상태</summary>
    public EWorldState CurrentWorldState { get { return _currentWorldState; } set { _currentWorldState = value; } }

    /// <summary>월드의 오브젝트 모음</summary>
    private List<CWorldObject> _worldObjects = null;
    /// <summary>월드 오브젝트 개수</summary>
    private int _worldObjectCount = 0;
    /// <summary>제일 최근에 변경되었던 오브젝트 모음</summary>
    private List<CWorldObject> _lastChangeObjects = null;
    /// <summary>제일 최근에 변경되었던 오브젝트 개수</summary>
    private int _lastChangeObjectCount = 0;

    /// <summary>카메라 무빙워크가 끝날때까지 대기</summary>
    private WaitUntil _endCameraMovingWorkWaitUntil = null;
    /// <summary>플레이어가 2D로 변하기까지 기다리는 시간</summary>
    private WaitForSeconds _changePlayer2DWaitForSeconds = null;

    private void Awake()
    {
        _instance = this;

        _worldObjects = new List<CWorldObject>();
        _lastChangeObjects = new List<CWorldObject>();

        _endCameraMovingWorkWaitUntil = new WaitUntil(() => !CCameraController.Instance.IsOnMovingWork);
        _changePlayer2DWaitForSeconds = new WaitForSeconds(0.3f);
    }

    /// <summary>월드 오브젝트 등록</summary>
    public void AddWorldObject(CWorldObject worldObject)
    {
        _worldObjects.Add(worldObject);
        _worldObjectCount++;
    }

    /// <summary>포함되었던 오브젝트 리셋</summary>
    public void ResetIncludedWorldObjects()
    {
        for (int i = 0; i < _worldObjectCount; i++)
            _worldObjects[i].IsCanChange2D = false;
    }

    /// <summary>월드 변경</summary>
    public void ChangeWorld(bool isChangeLastObject = false)
    {
        if (isChangeLastObject)
            SetLastObjects();

        StartCoroutine(ChangeWorldLogic());
    }

    /// <summary>최근 오브젝트 설정</summary>
    private void SetLastObjects()
    {
        for (int i = 0; i < _lastChangeObjectCount; i++)
            _lastChangeObjects[i].IsCanChange2D = true;
    }

    /// <summary>월드 변경 로직</summary>
    private IEnumerator ChangeWorldLogic()
    {
        if(_currentWorldState.Equals(EWorldState.View2D))
        {
            _currentWorldState = EWorldState.Changing;

            CCameraController.Instance.Change3D();

            for (int i = 0; i < _worldObjectCount; i++)
                _worldObjects[i].Change3D();

            CLightController.Instance.SetShadows(true);
            CPlayerManager.Instance.Change3D();

            yield return _endCameraMovingWorkWaitUntil;

            _currentWorldState = EWorldState.View3D;
        }
        else
        {
            _currentWorldState = EWorldState.Changing;

            CCameraController.Instance.Change2D();

            _lastChangeObjects.Clear();
            _lastChangeObjectCount = 0;

            for (int i = 0; i < _worldObjectCount; i++)
            {
                _worldObjects[i].Change2D();

                if (_worldObjects[i].IsCanChange2D)
                {
                    _lastChangeObjects.Add(_worldObjects[i]);
                    _lastChangeObjectCount++;
                }
            }

            CLightController.Instance.SetShadows(false);

            yield return _changePlayer2DWaitForSeconds;
            CPlayerManager.Instance.Change2D();

            yield return _endCameraMovingWorkWaitUntil;

            _currentWorldState = EWorldState.View2D;
        }
    }
}
