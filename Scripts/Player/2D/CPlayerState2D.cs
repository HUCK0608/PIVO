using UnityEngine;

public class CPlayerState2D : CPlayerState
{
    private static string _animParameterPath = "CurrentState";

    private CPlayerController2D _controller2D;
    /// <summary>플레이어 컨트롤러 2D</summary>
    public CPlayerController2D Controller2D { get { return _controller2D; } }

    private Animator _animator;

    protected virtual void Awake()
    {
        _controller2D = CPlayerManager.Instance.Controller2D;

        _animator = GetComponent<Animator>();
    }

    public override void InitState()
    {
        _animator.SetInteger(_animParameterPath, (int)_controller2D.CurrentState);
    }
}
