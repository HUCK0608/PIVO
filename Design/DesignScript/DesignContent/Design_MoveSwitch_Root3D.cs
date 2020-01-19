using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MoveSwitch_Root3D : CSingleObject3D
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
        else
            transform.Find("InnerMesh").gameObject.SetActive(false);

        base.Change2D();
    }

    public override void Change3D()
    {
        if (IsCanChange2D)
            _collider2D.enabled = false;

        transform.Find("InnerMesh").gameObject.SetActive(true);

        base.Change3D();
    }
}
