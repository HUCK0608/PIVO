using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMultiObject2D3D : CWorldObject
{
    /// <summary>메쉬 렌더러 리스트</summary>
    private List<MeshRenderer> _meshRenderes;
    /// <summary>기존 머테리얼 리스트</summary>
    private List<Material> _defaultMaterials;
    /// <summary>콜라이더2D 리스트</summary>
    private List<Collider2D> _collider2Ds;
    /// <summary>기존 라이트맵 인덱스</summary>
    private List<int> _defaultLightmapIndex;

    protected override void Awake()
    {
        base.Awake();

        _meshRenderes = new List<MeshRenderer>();
        _defaultMaterials = new List<Material>();
        _collider2Ds = new List<Collider2D>();
        _defaultLightmapIndex = new List<int>();

        _meshRenderes.AddRange(RootObject.GetComponentsInChildren<MeshRenderer>());
        _collider2Ds.AddRange(RootObject.GetComponentsInChildren<Collider2D>());
        foreach (Collider2D c in _collider2Ds)
            c.enabled = false;
        foreach (MeshRenderer m in _meshRenderes)
        {
            _defaultMaterials.Add(m.material);
            _defaultLightmapIndex.Add(m.lightmapIndex);
        }
    }

    public override void Change2D()
    {
        if(IsCanChange2D)
        {
            foreach (Collider2D c in _collider2Ds)
                c.enabled = true;

            foreach (MeshRenderer m in _meshRenderes)
            {
                m.lightmapIndex = -1;

                if (IsUse2DTexture)
                    m.material.SetFloat("_IsUse2DTexture", 1f);
            }
        }
        else
        {
            foreach (MeshRenderer m in _meshRenderes)
                m.enabled = false;
        }
    }

    public override void Change3D()
    {
        if(IsCanChange2D)
        {
            foreach (Collider2D c in _collider2Ds)
                c.enabled = false;

            for(int i = 0; i < _meshRenderes.Count; i++)
            {
                _meshRenderes[i].lightmapIndex = _defaultLightmapIndex[i];

                if (IsUse2DTexture)
                    _meshRenderes[i].material.SetFloat("_IsUse2DTexture", 0f);
            }
        }
        else
        {
            foreach (MeshRenderer m in _meshRenderes)
                m.enabled = true;
        }
    }

    public override void ShowOnBlock()
    {
        foreach (MeshRenderer m in _meshRenderes)
            m.material = BlockMaterial;
    }

    public override void ShowOffBlock()
    {
        for (int i = 0; i < _meshRenderes.Count; i++)
            _meshRenderes[i].material = _defaultMaterials[i];
    }
}
