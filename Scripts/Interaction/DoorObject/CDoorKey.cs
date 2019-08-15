using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoorKey : MonoBehaviour
{
    /// <summary>연결된 문</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private CDoor _door = null;

    /// <summary>이동 속도</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private float _moveSpeed = 0f;

    /// <summary>이미 키를 습득했는지 여부</summary>
    bool _isGet = false;

    private void Awake()
    {
        _door.RegisterKey();
    }

    /// <summary>키 습득</summary>
    public void GetKey()
    {
        if(!_isGet)
            StartCoroutine(GetKeyLogic());
    }

    /// <summary>키 습득 로직</summary>
    private IEnumerator GetKeyLogic()
    {
        _isGet = true;
        Vector3 landingPoint = _door.GetLandingPoint() - Vector3.up;

        while (!transform.position.Equals(landingPoint))
        {
            transform.position = Vector3.MoveTowards(transform.position, landingPoint, _moveSpeed * Time.deltaTime);

            yield return null;
        }

        _door.GetKey();
        gameObject.SetActive(false);
    }
}
