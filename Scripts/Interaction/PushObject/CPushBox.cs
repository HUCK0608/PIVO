using System.Collections;
using UnityEngine;

public class CPushBox : MonoBehaviour
{
    /// <summary>상자 이동 속도</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private float _moveSpeed = 0f;

    private bool _isMove = false;
    /// <summary>상자가 이동중인지 여부</summary>
    public bool IsMove { get { return _isMove; } }

    /// <summary>상자 이동</summary>
    public void MoveBox(Vector3 direction)
    {
        // 전, 후면 / 좌, 우측 방향
        Vector3 forwardBackDirection = direction;
        forwardBackDirection.x = 0f;
        Vector3 rightLeftDirection = direction;
        rightLeftDirection.z = 0f;

        // 전, 후면 / 좌, 우측 전진해야하는 센터포인트
        Vector3 forwardBackCenterPoint = transform.position + forwardBackDirection * 2f;
        Vector3 rightLeftCenterPoint = transform.position + rightLeftDirection * 2f;

        // 갈 수 있는지 체크 후 갈 수 있으면 이동
        if (Physics.Raycast(forwardBackCenterPoint, Vector3.down, Mathf.Infinity, CLayer.PushTile.LeftShiftToOne()))
            StartCoroutine(MoveLogic(forwardBackDirection));
        else if(Physics.Raycast(rightLeftCenterPoint, Vector3.down, Mathf.Infinity, CLayer.PushTile.LeftShiftToOne()))
            StartCoroutine(MoveLogic(rightLeftDirection));
    }

    /// <summary>상자 이동 로직</summary>
    private IEnumerator MoveLogic(Vector3 direction)
    {
        _isMove = true;

        Vector3 movePoint = transform.position + direction * 2f;

        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint, _moveSpeed * Time.deltaTime);

            if(transform.position.Equals(movePoint))
                break;

            yield return null;
        }

        _isMove = false;
    }
}
