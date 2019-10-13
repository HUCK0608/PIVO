public class CSoopState3D : CSoopState
{
    private CSoopController3D _controller = null;
    /// <summary>숲숲이 3D 컨트롤러</summary>
    public CSoopController3D Controller3D { get { return _controller; } }

    protected virtual void Awake()
    {
        _controller = GetComponentInParent<CSoopController3D>();
    }

    public override void InitState() { _controller.ChangeAnimation(); }
}
