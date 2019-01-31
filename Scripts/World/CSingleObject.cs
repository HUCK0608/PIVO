using UnityEngine;

public class CSingleObject : CWorldObject
{
    private MeshRenderer _renderer3D;
    private Collider2D _collider2D;

    protected override void Awake()
    {
        base.Awake();

        _renderer3D = RootObject3D.GetComponentInChildren<MeshRenderer>();
        _collider2D = RootObject2D.GetComponentInChildren<Collider2D>();

        _collider2D.enabled = false;
    }

    public override void Change2D()
    {
        if (IsCanChange2D)
            _collider2D.enabled = true;
        else
            _renderer3D.enabled = false;
    }

    public override void Change3D()
    {
        if (IsCanChange2D)
        {
            _collider2D.enabled = false;
            IsCanChange2D = false;
        }
        else
            _renderer3D.enabled = true;
    }
}
