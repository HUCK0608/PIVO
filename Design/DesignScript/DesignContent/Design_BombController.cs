using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombController : Design_WorldObjectController
{
    public GameObject BoomEffect;

    [HideInInspector]
    public KeyCode InteractionKey, ExplosionKey;
    [HideInInspector]
    public Design_BombSpawn ParentBombSpawn;

    GameObject IgnitionFireEffect;
    CPlayerManager Corgi;
    Design_Bomb3D Actor3DClass;
    Design_Bomb2D Actor2DClass;

    Vector3 ColliderSize, ColliderOffset;
    Vector3 Collider2DSize, Collider2DOffset;

    float ExplosionDistance;
    float BoxSize;

    [HideInInspector]
    public bool bAttachCorgi;
    [HideInInspector]
    public bool bUseBomb = false;
    [HideInInspector]
    public bool IsEnabled = false;
    [HideInInspector]
    public bool bAttach = false;


    protected override void Awake()
    {
        base.Awake();

        Actor3DClass = RootObject3D.GetComponent<Design_Bomb3D>();
        Actor3DClass.Controller = GetComponent<Design_BombController>();
        Actor3DClass.BeginPlay();

        Actor2DClass = RootObject2D.GetComponent<Design_Bomb2D>();
        Actor2DClass.Controller = GetComponent<Design_BombController>();
        Actor2DClass.BeginPlay();
        Actor2DClass.enabled = false;

        IgnitionFireEffect = transform.Find("IgnitionFireGroup").gameObject;
        DisableBomb();

        ColliderSize = RootObject3D.GetComponent<BoxCollider>().size;
        ColliderOffset = RootObject3D.GetComponent<BoxCollider>().center;

        Collider2DSize = RootObject2D.GetComponent<BoxCollider2D>().size;
        Collider2DOffset = RootObject2D.GetComponent<BoxCollider2D>().offset;

        InteractionKey = CKeyManager.InteractionKey;
        ExplosionKey = CKeyManager.BombInteractionKey;

        Corgi = CPlayerManager.Instance;

        ExplosionDistance = 12.5f;
        BoxSize = 8f;
    }

    public override void DesignChange2D()
    {
        base.DesignChange2D();

        Actor2DClass.enabled = true;
        Actor3DClass.enabled = false;

        if (!IsCanChange2D)
            SetIgnitionFireEffect(false);
    }

    public override void DesignChange3D()
    {
        base.DesignChange3D();

        Actor3DClass.enabled = true;
        Actor2DClass.enabled = false;

        if (IsEnabled)
            SetIgnitionFireEffect(true);
    }

    public override void Change3D()
    {
        if (bUseBomb)
            base.Change3D();
    }

    public override void Change2D()
    {
        if (bUseBomb)
            base.Change2D();
    }

    void Update()
    {
        Explosion();

        //if (bAttachCorgi)
        //    DownBomb();
        if (bAttach)
            AttachForDistance();
    }






    void DownBomb()
    {
        if (Input.GetKeyDown(InteractionKey))
        {
            EnableBomb();
            if (WorldManager.CurrentWorldState == EWorldState.View3D)
                Actor3DClass.DownBomb();
            else if (WorldManager.CurrentWorldState == EWorldState.View2D)
                Actor2DClass.DownBomb();
        }
    }

    void AttachForDistance()
    {
        if (Input.GetKeyDown(InteractionKey) && bUseBomb)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View3D)
                Actor3DClass.AttachForDistance();
        }            
    }

    public void AttachCorgi()
    {
        if (!Corgi.Controller3D.CurrentState.Equals(EPlayerState3D.Idle) && !Corgi.Controller3D.CurrentState.Equals(EPlayerState3D.Move))
            return;

        if (bAttachCorgi)
            return;

        StartCoroutine(PutBombInitLogic());
    }

    public void EnableBomb()
    {
        IsEnabled = true;
        SetIgnitionFireEffect(true);
    }

    public void DisableBomb()
    {
        IsEnabled = false;
        bAttach = false;
        SetIgnitionFireEffect(false);
    }

    public void SetIgnitionFireEffect(bool bState)
    {
        IgnitionFireEffect.SetActive(bState);
    }
    
    void Explosion()
    {
        bool bCondition = false;
        if (WorldManager.CurrentWorldState == EWorldState.View3D)
        {
            Vector3 Corgi3DPos = CPlayerManager.Instance.RootObject3D.transform.position;
            if (Vector3.Distance(Corgi3DPos, this.transform.position) < ExplosionDistance)
                bCondition = true;
        }
        else if(WorldManager.CurrentWorldState == EWorldState.View2D)
        {
            Vector3 Corgi2DPos = CPlayerManager.Instance.RootObject2D.transform.position;
            if (Vector2.Distance(Corgi2DPos, this.transform.position) < ExplosionDistance)
            {
                if (IsCanChange2D)
                    bCondition = true;
            }
        }

        if (Input.GetKeyDown(ExplosionKey) && bCondition && bUseBomb && IsEnabled)
        {
            if (this.transform.parent != null)
            {
                if (this.transform.parent.gameObject != CPlayerManager.Instance.RootObject3D && this.transform.parent.gameObject != CPlayerManager.Instance.RootObject2D)
                {
                    this.transform.parent = null;
                    BeginExplosion();
                }
            }
            else
            {
                if (WorldManager.CurrentWorldState == EWorldState.View2D)
                    BeginExplosion();
            }
        }
    }

    public void BeginExplosion()
    {
        StartCoroutine(ExplosionCoroutine());
        SetIgnitionFireEffect(false);
        DisableBomb();
    }

    IEnumerator ExplosionCoroutine()
    {
        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomInstance = Instantiate(BoomEffect, transform.position + AddPosition, transform.rotation);
        RootObject3D.GetComponent<MeshRenderer>().enabled = false;
        IgnitionFireEffect.SetActive(false);
        bUseBomb = false;

        if (WorldManager.CurrentWorldState == EWorldState.View3D)
        {
            RootObject3D.GetComponent<BoxCollider>().isTrigger = true;

            RootObject3D.GetComponent<BoxCollider>().enabled = false;
            RootObject3D.GetComponent<BoxCollider>().enabled = true;

            RootObject3D.GetComponent<BoxCollider>().size = new Vector3(BoxSize, BoxSize - 1f, BoxSize);
            RootObject3D.GetComponent<BoxCollider>().center = RootObject3D.GetComponent<BoxCollider>().center + new Vector3(0, BoxSize / 2 - 4f, 0);

            yield return new WaitForSeconds(0.5f);

            RootObject3D.GetComponent<BoxCollider>().enabled = false;
        }

        else if (WorldManager.CurrentWorldState == EWorldState.View2D)
        {
            RootObject2D.GetComponent<BoxCollider2D>().isTrigger = true;

            RootObject2D.GetComponent<BoxCollider2D>().enabled = false;
            RootObject2D.GetComponent<BoxCollider2D>().enabled = true;

            RootObject2D.GetComponent<BoxCollider2D>().size = new Vector3(BoxSize, BoxSize - 1f, BoxSize);
            RootObject2D.GetComponent<BoxCollider2D>().offset = RootObject2D.GetComponent<BoxCollider2D>().offset + new Vector2(0, BoxSize / 2 - 4f);

            yield return new WaitForSeconds(0.5f);

            RootObject2D.GetComponent<BoxCollider2D>().enabled = false;
        }

        yield return new WaitForSeconds(1.5f);

        transform.position = ParentBombSpawn.transform.position;
        transform.rotation = ParentBombSpawn.transform.rotation;
        ParentBombSpawn.SpawnBomb();

        Destroy(BoomInstance);

        RootObject2D.GetComponent<BoxCollider2D>().size = Collider2DSize;
        RootObject2D.GetComponent<BoxCollider2D>().offset = Collider2DOffset;
        RootObject3D.GetComponent<BoxCollider>().size = ColliderSize;
        RootObject3D.GetComponent<BoxCollider>().center = ColliderOffset;

        RootObject3D.GetComponent<MeshRenderer>().enabled = true;

        yield return new WaitUntil(() => WorldManager.CurrentWorldState != EWorldState.Changing);

        if (WorldManager.CurrentWorldState == EWorldState.View3D)
            RootObject3D.GetComponent<BoxCollider>().enabled = true;
        else if (WorldManager.CurrentWorldState == EWorldState.View2D)
            RootObject2D.GetComponent<BoxCollider2D>().enabled = true;
    }

    private IEnumerator PutBombInitLogic()
    {
        bAttachCorgi = true;

        CPlayerController3D corgiController3D = Corgi.Controller3D;
        corgiController3D.ChangeState(EPlayerState3D.PutObjectInit);
        yield return new WaitUntil(() => corgiController3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("PutObjectInit"));

        float animationTime = corgiController3D.Animator.GetCurrentAnimatorStateInfo(0).length;
        Vector3 startPoint = transform.position;
        Vector3 putPoint = corgiController3D.transform.position + Vector3.up * 4f + corgiController3D.transform.forward * 2f;
        float oneDivAnimationTime = 1f / (animationTime - 0.1f);
        float addTime = 0f;
        while(true)
        {
            addTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint, putPoint, Mathf.Clamp(oneDivAnimationTime * addTime, 0f, 1f));

            if (transform.position.Equals(putPoint))
                break;

            yield return null;
        }

        transform.parent = corgiController3D.transform;
        yield return new WaitUntil(() => corgiController3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("PutObjectIdle"));

        StartCoroutine(PutBombEndLogic());
    }

    private IEnumerator PutBombEndLogic()
    {
        CPlayerController3D corgiController3D = Corgi.Controller3D;

        yield return new WaitUntil(() => Input.GetKeyDown(InteractionKey) && !corgiController3D.CurrentState.Equals(EPlayerState3D.PutObjectFalling));

        transform.parent = ParentBombSpawn.transform;
        corgiController3D.ChangeState(EPlayerState3D.PutObjectEnd);
        yield return new WaitUntil(() => corgiController3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("PutObjectEnd"));

        Vector3 startPoint = transform.position;
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity);
        Vector3 putPoint = hit.point + Vector3.up;
        float putTime = 0.1f;
        float oneDivAnimationTime = 1f / putTime;
        float addTime = 0f;

        yield return new WaitForSeconds(0.3f);
        while(true)
        {
            addTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint, putPoint, Mathf.Clamp(oneDivAnimationTime * addTime, 0f, 1f));

            if (transform.position.Equals(putPoint))
                break;

            yield return null;
        }

        EnableBomb();
        bAttachCorgi = false;
    }

    /*
     * 폭탄을 들고 있는 상태로는 시점전환을 할 수 없다.
     * -> 폭탄을 들고 있어서 마법을 못씀
     * 
     * 시점전환을 실행 중일 때는 폭탄을 터트릴 수 없다.
     * -> 마법에 집중하고 있는 상태여서 폭탄을 못씀
     */
}
