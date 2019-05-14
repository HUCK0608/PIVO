using UnityEngine;

public class CPlayerStat : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 0f;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    private float _gravity = 0f;
    /// <summary>중력</summary>
    public float Gravity { get { return _gravity; } }

    [SerializeField]
    private float _rotationSpeed = 0f;
    /// <summary>회전 속도</summary>
    public float RotationSpeed { get { return _rotationSpeed; } }
}
