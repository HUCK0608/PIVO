using UnityEngine;

public class CPlayerStat : MonoBehaviour
{
    [SerializeField]
    private int _hp = 3;
    /// <summary>체력</summary>
    public int Hp { get { return _hp; }
        set
        {
            _hp = value;
            CUIManager.Instance.SetHpUI(_hp);

            if (_hp.Equals(0))
            {
                if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
                    CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Dead);
                else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
                    CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Dead);
            }
        }
    }

    [SerializeField]
    private float _moveSpeed = 0f;
    /// <summary>이동속도</summary>
    public float MoveSpeed { get { return _moveSpeed; } }

    [SerializeField]
    private float _airMoveSpeed = 0f;
    /// <summary>공중 이동속도</summary>
    public float AirMoveSpeed { get { return _airMoveSpeed; } }

    [SerializeField]
    private float _gravity = 0f;
    /// <summary>중력</summary>
    public float Gravity { get { return _gravity; } }

    [SerializeField]
    private float _rotationSpeed = 0f;
    /// <summary>회전 속도</summary>
    public float RotationSpeed { get { return _rotationSpeed; } }

    [SerializeField]
    private float _holdingMaxTime = 0f;
    /// <summary>바둥거리기 최대 시간</summary>
    public float HoldingMaxTime { get { return _holdingMaxTime; } }

    [SerializeField]
    private float _killYVolume = 0f;
    /// <summary>캐릭터에게 데미지를 입히는 y 위치</summary>
    public float KillYVolume { get { return _killYVolume; } }
}
