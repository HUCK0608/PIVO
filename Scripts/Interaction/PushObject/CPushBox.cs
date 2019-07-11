using System.Collections;
using UnityEngine;

public class CPushBox : MonoBehaviour
{
    /// <summary>상자 이동 속도</summary>
    [SerializeField]
    private float _moveSpeed = 0f;

    private bool _isMove = false;
    /// <summary>상자가 이동중인지 여부</summary>
    public bool IsMove { get { return _isMove; } }

    /// <summary>상자 이동</summary>
    public void MoveBox(Vector3 direction)
    {
        if (direction.Equals(Vector3.zero))
            return;

        Vector3 forwardBackDirection = direction;
        forwardBackDirection.x = 0f;

        Vector3 rightLeftDirection = direction;
        rightLeftDirection.z = 0f;

        // 전, 후면 / 좌, 우측 전진해야하는 센터포인트
        Vector3 forwardBackCenterPoint = transform.position + forwardBackDirection * 2f;
        Vector3 rightLeftCenterPoint = transform.position + rightLeftDirection * 2f;

        // 전, 후면에 상자가 갈 수 있는지 체크 후 가능하면 이동
        if (!forwardBackDirection.z.Equals(0f) && !Physics.BoxCast(transform.position, Vector3.one * 0.9f, forwardBackDirection, Quaternion.LookRotation(forwardBackDirection), 2f))
        {
            if (Physics.Raycast(forwardBackCenterPoint, Vector3.down, 1.2f, CLayer.PushTile.LeftShiftToOne()))
                StartCoroutine(MoveLogic(forwardBackDirection));
        }
        // 좌, 우측에 상자가 갈 수 있는지 체크 후 가능하면 이동
        else if (!rightLeftDirection.x.Equals(0f) && !Physics.BoxCast(transform.position, Vector3.one * 0.9f, rightLeftDirection, Quaternion.LookRotation(rightLeftDirection), 2f))
        {
            if (Physics.Raycast(rightLeftCenterPoint, Vector3.down, 1.2f, CLayer.PushTile.LeftShiftToOne()))
                StartCoroutine(MoveLogic(rightLeftDirection));
        }
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
