﻿using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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

    private Vector3 _lastGroundPosition = Vector3.zero;
    /// <summary>마지막 땅 위치</summary>
    public Vector3 LastGroundPosition { get { return _lastGroundPosition; } set { _lastGroundPosition = value; } }

    private bool _isCanOperation = true;
    /// <summary>조작이 가능한지 여부</summary>
    public bool IsCanOperation { get { return _isCanOperation; }  set { _isCanOperation = value; } }

    /// <summary>탐지 숲숲이 리스트</summary>
    private List<GameObject> _detectionSoopList = new List<GameObject>();

    private bool m_bInitialize = false;

    /// <summary>탐지 숲숲이 등록</summary>
    public void RegisterDetectionSoop(GameObject detectionSoop)
    {
        if (true == _detectionSoopList.Contains(detectionSoop))
            return;

        _detectionSoopList.Add(detectionSoop);
    }

    /// <summary>탐지 숲숲이 해제</summary>
    public void RemoveDetectionSoop(GameObject detectionSoop)
    {
        if (false == _detectionSoopList.Contains(detectionSoop))
            return;

        _detectionSoopList.Remove(detectionSoop);
    }

    /// <summary>숲숲이 탐지범위안에 있는지 여부</summary>
    public bool IsOnSoopDetection { get { return _detectionSoopList.Count == 0 ? false : true; } }

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

        m_bInitialize = true;
    }

    public override void Change2D()
    {
        base.Change2D();

        _effect.MoveDustEffect_ChangeState(EWorldState.View2D);
    }

    public override void Change3D()
    {
        base.Change3D();

        // 땅이아니면 Holding 상태로 변경
        if (true == m_bInitialize && !Controller3D.IsGrounded())
            Controller3D.ChangeState(EPlayerState3D.Holding);

        _effect.MoveDustEffect_ChangeState(EWorldState.View3D);
    }

    /// <summary>자동 이동 시작</summary>
    public void StartAutoMove(Vector3 target, float forceMoveCheckTime = 0f) { StartCoroutine(AutoMoveLogic(target, forceMoveCheckTime)); }

    /// <summary>자동이동 로직</summary>
    private IEnumerator AutoMoveLogic(Vector3 target, float forceMoveCheckTime = 0f)
    {
        _isCanOperation = false;

        _controller2D.ChangeState(EPlayerState2D.Move);
        _controller3D.ChangeState(EPlayerState3D.Move);

        float addTime = 0f;

        while (true)
        {
            Vector3 directionToTarget = target - RootObject3D.transform.position;
            directionToTarget.y = 0f;
            directionToTarget.Normalize();

            if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
                _controller2D.Move(directionToTarget);
            else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
                _controller3D.Move(directionToTarget);

            target.y = RootObject3D.transform.position.y;

            if (Vector3.Distance(RootObject3D.transform.position, target) <= 0.3f)
            {
                RootObject2D.transform.position = target;
                RootObject3D.transform.position = target;

                break;
            }

            addTime += Time.deltaTime;
            if (0 != forceMoveCheckTime && addTime >= forceMoveCheckTime)
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
