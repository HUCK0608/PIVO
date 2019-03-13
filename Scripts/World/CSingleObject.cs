using System;
using UnityEngine;

public class CSingleObject : CWorldObject
{
    /// <summary>메쉬 렌더러</summary>
    private MeshRenderer _meshRenderer;
    /// <summary>콜라이더2D</summary>
    private Collider2D _collider2D;
    /// <summary>기존 머테리얼</summary>
    private Material _defaultMaterial;
    /// <summary>기존 라이트맵 인덱스</summary>
    private int _defaultLightmapIndex;

    protected override void Awake()
    {
        base.Awake();

        _meshRenderer = RootObject3D.GetComponentInChildren<MeshRenderer>();
        _collider2D = RootObject2D.GetComponentInChildren<Collider2D>();
        //_defaultMaterial = _meshRenderer.material;
        _defaultLightmapIndex = _meshRenderer.lightmapIndex;

        _collider2D.enabled = false;
    }

    public override void Change2D()
    {
        if (IsCanChange2D)
        {
            _collider2D.enabled = true;
            _meshRenderer.lightmapIndex = -1;
        }
        else
            _meshRenderer.enabled = false;
    }

    public override void Change3D()
    {
        if (IsCanChange2D)
        {
            _collider2D.enabled = false;
            _meshRenderer.lightmapIndex = _defaultLightmapIndex;
            IsCanChange2D = false;
        }
        else
            _meshRenderer.enabled = true;
    }

    public override void ShowOnBlock()
    {
        if (_defaultMaterial == null)
            _defaultMaterial = _meshRenderer.material;

        _meshRenderer.material = BlockMaterial;
    }

    public override void ShowOffBlock()
    {
        if(_defaultMaterial != null)
            _meshRenderer.material = _defaultMaterial;
    }
}
