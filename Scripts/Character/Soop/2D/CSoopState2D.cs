using UnityEngine;

public class CSoopState2D : CSoopState
{
    private CSoopController2D _controller = null;
    /// <summary>숲숲이 2D 컨트롤러</summary>
    public CSoopController2D Controller2D { get { return _controller; } }

    protected virtual void Awake()
    {
        _controller = GetComponentInParent<CSoopController2D>();
    }

    public override void InitState() { _controller.ChangeAnimation(); }
}
