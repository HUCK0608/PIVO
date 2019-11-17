using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_WorldObjectController : CWorldObject
{
    private GameObject _rootObject2D = null;
    private CWorldManager _worldManager = null;
    public GameObject RootObject2D { get { return _rootObject2D; } }
    public CWorldManager WorldManager { get { return _worldManager; } }

    protected override void Awake()
    {
        base.Awake();
        _rootObject2D = RootObject.transform.Find("Root2D").gameObject;
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

        DesignChange2D();
    }

    public override void Change3D()
    {
        StartCoroutine(WaitChangeWorldCoroutine());
        RootObject2D.SetActive(false);
        RootObject3D.SetActive(true);

        IsCanChange2D = false;

        DesignChange3D();
    }

    public override void ShowOnBlock() { }
    public override void ShowOffBlock() { }

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
