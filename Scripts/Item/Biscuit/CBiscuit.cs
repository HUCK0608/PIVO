using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBiscuit : MonoBehaviour
{
    [SerializeField]
    private float _upDownRange = 0f;

    [SerializeField]
    private float _moveSpeed = 0f;

    private void Awake()
    {
        LoadData();
    }

    private void Start()
    {
        StartCoroutine(MoveUpDown());
    }

    /// <summary>데이터 로드</summary>
    private void LoadData()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        string[] splitPaths = sceneName.Split('.');

        string nodePath = splitPaths[0] + "Datas/StageDatas/BiscuitDatas/" + splitPaths[1];
        string dataFileName = splitPaths[0] + "Data";
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
