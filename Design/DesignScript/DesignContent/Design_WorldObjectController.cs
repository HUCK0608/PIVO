using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_WorldObjectController : CWorldObject
{
    /// <summary>메쉬 렌더러</summary>
    private MeshRenderer _meshRenderer;
    /// <summary>기존 머테리얼</summary>
    private Material _defaultMaterial;
    /// <summary>기존 라이트맵 인덱스</summary>
    private int _defaultLightmapIndex;

    private GameObject _rootObject2D = null;
    private CWorldManager _worldManager = null;
    public GameObject RootObject2D { get { return _rootObject2D; } }
    public CWorldManager WorldManager { get { return _worldManager; } }

    public bool NoMeshRenderer;

    protected override void Awake()
    {
        base.Awake();
        _rootObject2D = RootObject.transform.Find("Root2D").gameObject;
        if (!NoMeshRenderer)
        {
            _meshRenderer = RootObject3D.GetComponent<MeshRenderer>();
            _defaultLightmapIndex = _meshRenderer.lightmapIndex;
        }
    }

    protected override void Start()
    {
        base.Start();

        _worldManager = CWorldManager.Instance.GetComponent<CWorldManager>();
        BeginPlay();
    }

    public override void Change2D()
    {
        if (IsCanChange2D)
        {
            StartCoroutine(WaitChangeWorldCoroutine());
            if (RootObject2D.GetComponent<SpriteRenderer>())
            {
                StartCoroutine(WaitChangeWorldForSprite());
            }
            else
            {
                RootObject2D.SetActive(true);                
                RootObject3D.SetActive(true);                
            }
        }
        else
        {
            RootObject2D.SetActive(false);
            RootObject3D.SetActive(false);
        }



        if (!NoMeshRenderer)
        {
            if (IsCanChange2D)
            {
                _meshRenderer.lightmapIndex = -1;

                if (IsUse2DTexture)
                    _meshRenderer.material.SetFloat("_IsUse2DTexture", 1f);
            }
            else
                _meshRenderer.enabled = false;
        }

        DesignChange2D();
    }

    public override void Change3D()
    {
        StartCoroutine(WaitChangeWorldCoroutine());
        RootObject2D.SetActive(false);
        RootObject3D.SetActive(true);

        IsCanChange2D = false;


        if (!NoMeshRenderer)
        {
            if (IsCanChange2D)
            {
                _meshRenderer.lightmapIndex = _defaultLightmapIndex;
                IsCanChange2D = false;

                if (IsUse2DTexture)
                    _meshRenderer.material.SetFloat("_IsUse2DTexture", 0f);
            }
            else
                _meshRenderer.enabled = true;
        }

        DesignChange3D();
    }

    public override void ShowOnBlock()
    {
        if (!NoMeshRenderer)
        {
            if (_defaultMaterial == null)
                _defaultMaterial = _meshRenderer.material;

            _meshRenderer.material = BlockMaterial;
        }
    }
    public override void ShowOffBlock()
    {
        if (!NoMeshRenderer)
        {
            if (_defaultMaterial != null)
                _meshRenderer.material = _defaultMaterial;
        }
    }

    IEnumerator WaitChangeWorldForSprite()
    {
        yield return new WaitForSeconds(0.3f);

        RootObject2D.SetActive(true);
        RootObject3D.SetActive(false);
    }

    IEnumerator WaitChangeWorldCoroutine()
    {
        if (WorldManager != null)
        {
            yield return new WaitUntil(() => WorldManager.CurrentWorldState != EWorldState.Changing);

            WaitChangeWorld(WorldManager.CurrentWorldState);
        }

        yield return null;
    }

    public virtual void BeginPlay() { }
    public virtual void WaitChangeWorld(EWorldState CurState) { }
    public virtual void DesignChange2D() { }
    public virtual void DesignChange3D() { }
}
