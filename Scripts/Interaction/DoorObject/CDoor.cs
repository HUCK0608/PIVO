using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoor : MonoBehaviour
{
    /// <summary>착지 지점 리스트</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    List<GameObject> _landingPoints = null;

    /// <summary>문열기 속도</summary>
    [Header("Anyone can edit")]

    private int _nextLandingPointIndex = 4;
    private int _currentKeyCount = 4;

    private void Start()
    {
        for (int i = 3; i >= _currentKeyCount; i--, _nextLandingPointIndex--)
            _landingPoints[i].SetActive(false);
    }

    /// <summary>키 습득</summary>
    public void GetKey()
    {
        _landingPoints[_currentKeyCount++].SetActive(true);

        // 키가 전부 모이면 문이 열림
        if (_currentKeyCount.Equals(4))
            Debug.Log("call");
    }

    /// <summary>문열기 로직</summary>
    private IEnumerator OpenDoorLogic()
    {
        while(true)
        {

        }
    }

    /// <summary>키 등록</summary>
    public void RegisterKey() { _currentKeyCount--; }
    /// <summary>착지 위치 반환</summary>
    public Vector3 GetLandingPoint() { return _landingPoints[_nextLandingPointIndex++].transform.position; }
}
