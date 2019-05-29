using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBiscuit : MonoBehaviour
{
    [SerializeField]
    private float _upDownRange = 0f;

    [SerializeField]
    private float _moveSpeed = 0f;

    /// <summary>이전에 먹은 비스킷인지 여부</summary>
    private bool _isDidEat = false;

    private void Start()
    {
        StartCoroutine(MoveUpDown());
    }

    /// <summary>데이터 불러오기</summary>
    private void LoadData()
    {
        string[] scenePaths = SceneManager.GetActiveScene().name.Split(',');    // 0 : Season, 1 : stage_xx
    }

    /// <summary>위 아래 이동</summary>
    private IEnumerator MoveUpDown()
    {
        Vector3 startPosition = transform.position;
        float currentMoveY = 0f;
        bool isMoveUp = true;

        while(true)
        {
            if(!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            {
                if (isMoveUp)
                {
                    currentMoveY = Mathf.Clamp(currentMoveY + _moveSpeed * Time.deltaTime, -_upDownRange, _upDownRange);

                    if (currentMoveY.Equals(_upDownRange))
                        isMoveUp = false;
                }
                else
                {
                    currentMoveY = Mathf.Clamp(currentMoveY - _moveSpeed * Time.deltaTime, -_upDownRange, _upDownRange);

                    if (currentMoveY.Equals(-_upDownRange))
                        isMoveUp = true;
                }
            }

            transform.position = startPosition + Vector3.up * currentMoveY;

            yield return null;
        }
    }
}
