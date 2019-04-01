using UnityEngine;

public class CPlayerState2D : CPlayerState
{
    private CPlayerController2D _controller2D;
    /// <summary>플레이어 컨트롤러 2D</summary>
    public CPlayerController2D Controller2D { get { return _controller2D; } }

    protected virtual void Awake() { _controller2D = CPlayerManager.Instance.Controller2D; }

    public override void InitState() { Controller2D.ChangeAnimation(); }
}
