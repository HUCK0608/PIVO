using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif

public class CSoopStat : MonoBehaviour
{
    [Header("Anyone can edit")]
    [SerializeField]
    private bool _isSoopDirectionRight = false;
    /// <summary>숲숲이 방향이 오른쪽인지 여부</summary>
    public bool IsSoopDirectionRight { get { return _isSoopDirectionRight; } }

    [SerializeField]
    private Vector3 _detectionAreaLocalPosition = Vector3.zero;
    /// <summary>탐지범위 위치(현재 위치 + 탐지범위 위치 반환)</summary>
    public Vector3 DetectionAreaPosition { get { return transform.position + _detectionAreaLocalPosition; } }
    [SerializeField]
    private Vector3 _detectionAreaSize = Vector3.zero;
    /// <summary>탐지범위 크기</summary>
    public Vector3 DetectionAreaSize { get { return _detectionAreaSize; } }

    [SerializeField]
    private float _moveSpeed = 0f;
    /// <summary>이동 속도</summary>
    public float MoveSpeed { get { return _moveSpeed; } }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // 탐지범위 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(DetectionAreaPosition, _detectionAreaSize);
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            int directionToInt = _isSoopDirectionRight ? 1 : -1;

            transform.Find("Root3D").localEulerAngles = new Vector3(0f, 90f * directionToInt, 0f);
            transform.Find("Root2D").localScale = new Vector3(-1f * directionToInt, 1f, 1f);
        }
    }
#endif
}
