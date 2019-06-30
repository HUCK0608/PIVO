﻿using System.Collections;
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

    private bool _isCanOperation = true;
    /// <summary>조작이 가능한지 여부</summary>
    public bool IsCanOperation { get { return _isCanOperation; }  set { _isCanOperation = value; } }

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
    public void StartAutoMove(Vector3 target) { StartCoroutine(AutoMoveLogic(target)); }

    /// <summary>자동이동 로직</summary>
    private IEnumerator AutoMoveLogic(Vector3 target)
    {
        _isCanOperation = false;

        _controller2D.ChangeState(EPlayerState2D.Move);
        _controller3D.ChangeState(EPlayerState3D.Move);

        while (true)
        {
            Vector3 directionToTarget = target - RootObject3D.transform.position;
            directionToTarget.y = 0f;
            directionToTarget.Normalize();

            if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
                _controller2D.Move(directionToTarget);
            else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
                _controller3D.Move(directionToTarget);

            target.y = transform.position.y;

            if (Vector3.Distance(RootObject3D.transform.position, target) <= 0.5f)
            {
                RootObject2D.transform.position = target;
                RootObject3D.transform.position = target;

                break;
            }

            yield return null;
        }

        _controller2D.ChangeState(EPlayerState2D.Idle);
        _controller3D.ChangeState(EPlayerState3D.Idle);

        _isCanOperation = true;
    }
}
