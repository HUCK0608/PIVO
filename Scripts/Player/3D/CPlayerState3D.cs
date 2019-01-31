using UnityEngine;

public class CPlayerState3D : CPlayerState
{
    private static string _animParameterPath = "CurrentState";

    private CPlayerController3D _controller3D;
    /// <summary>플레이어 컨트롤러 3D</summary>
    public CPlayerController3D Controller3D { get { return _controller3D; } }

    private Animator _animator;

    protected virtual void Awake()
    {
        _controller3D = CPlayerManager.Instance.Controller3D;

        _animator = GetComponent<Animator>();
    }

    public override void InitState()
    {
        _animator.SetInteger(_animParameterPath, (int)_controller3D.CurrentState);
    }
}
