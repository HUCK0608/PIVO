using System.Collections;
using UnityEngine;

public class CPlayerManager : CCharacter
{
    public static CPlayerManager _instance;
    /// <summary>플레이어 매니저 싱글턴</summary>
    public static CPlayerManager Instance { get { return _instance; } }

    private CPlayerStat _stat = null;
    /// <summary>플레이어 스텟</summary>
    public CPlayerStat Stat { get { return _stat; } }

    private CPlayerEffect _effect = null;
    /// <summary>플레이어 이펙트</summary>
    public CPlayerEffect Effect { get { return _effect; } }

    private CPlayerController2D _controller2D;
    /// <summary>플레이어 컨트롤러 2D</summary>
    public CPlayerController2D Controller2D { get { return _controller2D; } }

    private CPlayerController3D _controller3D;
    /// <summary>플레이어 컨트롤러 3D</summary>
    public CPlayerController3D Controller3D { get { return _controller3D; } }

    protected override void Awake()
    {
        base.Awake();

        _instance = this;
        _stat = GetComponent<CPlayerStat>();
        _effect = GetComponent<CPlayerEffect>();

        _controller2D = RootObject2D.GetComponent<CPlayerController2D>();
        _controller3D = RootObject3D.GetComponent<CPlayerController3D>();
    }

    private void Start()
    {
        Change3D();
    }

    public override void Change2D()
    {
        RootObject3D.SetActive(false);
        RootObject2D.transform.parent = transform;
        RootObject2D.transform.eulerAngles = Vector3.zero;
        RootObject3D.transform.parent = RootObject2D.transform;
        RootObject2D.SetActive(true);

        _effect.MoveDustEffect_ChangeState(EWorldState.View2D);
    }

    public override void Change3D()
    {
        RootObject2D.SetActive(false);
        RootObject3D.transform.parent = transform;
        RootObject2D.transform.parent = RootObject3D.transform;
        RootObject3D.SetActive(true);

        // 땅이아니면 Holding 상태로 변경
        if (!Controller3D.IsGrounded())
            Controller3D.ChangeState(EPlayerState3D.Holding);

        _effect.MoveDustEffect_ChangeState(EWorldState.View3D);
    }

    /// <summary>자동 이동 시작</summary>
    public void StartAutoMove(Vector3 target)
    {
        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
            _controller2D.StartAutoMove(target);
        else
            _controller3D.StartAutoMove(target);
    }
}
