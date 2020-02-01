using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
            else
            {
                StopCoroutine("PlayerFlickeringLogic");
                StartCoroutine("PlayerFlickeringLogic");
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

    [SerializeField]
    private float _climb1Percent = 0f;
    /// <summary>기어오르기1 확률</summary>
    public float Climb1Percent { get { return _climb1Percent; } }

    [SerializeField]
    private float _idleVariationMinTime = 0f;
    /// <summary>Idle 바리에이션 최소 시간</summary>
    public float IdleVariationMinTime { get { return _idleVariationMinTime; } }

    [SerializeField]
    private float _putEndAndChange3DDleay = 0f;
    /// <summary>코기가 던져지고 3D로 변경될 때 딜레이</summary>
    public float PutEndAndChange3DDelay { get { return _putEndAndChange3DDleay; } }

    private bool _isPut = false;
    /// <summary>코기가 잡혀있는지 여부</summary>
    public bool IsPut { get { return _isPut; } set { _isPut = value; } }

    /// <summary>깜빡거림을 적용할 오브젝트 리스트</summary>
    [SerializeField]
    private List<GameObject> _flickerObjects = new List<GameObject>();

    /// <summary>플레이어 깜빡거림</summary>
    private IEnumerator PlayerFlickeringLogic()
    {
        int flickeringCount = 5;
        float halfFlickeringDelay = 0.1f;

        int currentFlickeringCount = 0;

        while(true)
        {
            foreach (GameObject obj in _flickerObjects)
                obj.SetActive(false);

            yield return new WaitForSeconds(halfFlickeringDelay);

            foreach (GameObject obj in _flickerObjects)
                obj.SetActive(true);

            currentFlickeringCount++;
            if (currentFlickeringCount == flickeringCount)
                break;

            yield return new WaitForSeconds(halfFlickeringDelay);
        }
    }
}
