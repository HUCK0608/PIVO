using UnityEngine;

public class CSingleObject2D3D : CSingleObject3D
{
    /// <summary>콜라이더2D</summary>
    private Collider2D _collider2D;

    protected override void Awake()
    {
        base.Awake();

        _collider2D = transform.parent.GetComponentInChildren<Collider2D>();

        _collider2D.enabled = false;
    }

    public override void Change2D()
    {
        if (IsCanChange2D)
            _collider2D.enabled = true;

        base.Change2D();
    }

    public override void Change3D()
    {
        if (IsCanChange2D)
            _collider2D.enabled = false;

        base.Change3D();
    }
}
