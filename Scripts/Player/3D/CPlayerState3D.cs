using UnityEngine;

public class CPlayerState3D : CPlayerState
{
    private CPlayerController3D _controller3D;
    /// <summary>플레이어 컨트롤러 3D</summary>
    public CPlayerController3D Controller3D { get { return _controller3D; } }

    protected virtual void Awake() { _controller3D = CPlayerManager.Instance.Controller3D; }

    public override void InitState() { Controller3D.ChangeAnimation(); }
}
