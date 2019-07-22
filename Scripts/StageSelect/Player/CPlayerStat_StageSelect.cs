using UnityEngine;

public class CPlayerStat_StageSelect : MonoBehaviour
{
    [Header("Anyone can edit")]
    [SerializeField]
    private float _moveSpeed = 0f;
    /// <summary>이동 속도</summary>
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    private float _gravity = 0f;
    /// <summary>중력</summary>
    public float Gravity { get { return _gravity; } }

    [SerializeField]
    private float _climb1Percent = 0f;
    /// <summary>기어오르기1 확률</summary>
    public float Climb1Percent { get { return _climb1Percent; } }
}
