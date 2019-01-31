using UnityEngine;

public class CPlayerStat : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 0f;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    private float _climbSpeed = 0f;
    /// <summary>기어오르기 속도</summary>
    public float ClimbSpeed { get { return _climbSpeed; } }

    [SerializeField]
    private float _gravity = 0f;
    /// <summary>중력</summary>
    public float Gravity { get { return _gravity; } }

    [SerializeField]
    private float _rotationSpeed = 0f;
    /// <summary>회전 속도</summary>
    public float RotationSpeed { get { return _rotationSpeed; } }

    [SerializeField]
    private float _viewRectAdjustScaleZSpeed = 0f;
    /// <summary>시점전환 상자의 Z 크기를 조절하는 속도</summary>
    public float ViewRectAdjustSpeed { get { return _viewRectAdjustScaleZSpeed; } }
    
    [SerializeField]
    private float _maxViewRectScaleX = 0f;
    /// <summary>최대 시점전환 상자 x 크기</summary>
    public float MaxViewRectScaleX { get { return _maxViewRectScaleX; } }

    [SerializeField]
    private float _maxViewRectScaleY = 0f;
    /// <summary>최대 시점전환 상자 y 크기</summary>
    public float MaxViewRectScaleY { get { return _maxViewRectScaleY; } }

    [SerializeField]
    private float _maxViewRectScaleZ = 0f;
    /// <summary>최대 시점전환 상자 z 크기</summary>
    public float MaxViewRectScaleZ { get { return _maxViewRectScaleZ; } }

    [SerializeField]
    private float _viewRectIncreaseXYRatePerSec = 0f;
    /// <summary>시점전환 상자의 x, y 크기가 커지는 1초당 비율</summary>
    public float ViewRectIncreaseXYRatePerSec { get { return _viewRectIncreaseXYRatePerSec; } }

    [SerializeField]
    private float _viewRectIncreaseZRatePerSec = 0f;
    /// <summary>시점전환 상자의 z 크기가 커지는 1초당 비율</summary>
    public float ViewRectIncreaseZRatePerSec { get { return _viewRectIncreaseZRatePerSec; } }
}
