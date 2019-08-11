using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMultiObject2D3D : CWorldObject
{
    /// <summary>메쉬 렌더러 리스트</summary>
    private List<MeshRenderer> _meshRenderes;
    /// <summary>기존 머테리얼 리스트</summary>
    private List<Material> _defaultMaterials;
    /// <summary>기존 라이트맵 인덱스</summary>
    private List<int> _defaultLightmapIndex;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Change2D()
    {
    }

    public override void Change3D()
    {
    }

    public override void ShowOnBlock()
    {
    }

    public override void ShowOffBlock()
    {
    }
}
