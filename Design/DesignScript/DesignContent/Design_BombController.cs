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

    float BoxSize;

    [HideInInspector]
    public bool bAttachCorgi;
    [HideInInspector]
    public bool bUseBomb = false;
    [HideInInspector]
    public bool IsEnabled = false;
    [HideInInspector]
    public bool bAttach = false;

    private bool _isActiveInteractionUI = false;

    [SerializeField]
    private SoundRandomPlayer_SFX _boomSoundRandomPlayer = null;
    private AudioSource _bombFireAudioSource = null;

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

        InteractionKey = CKeyManager.InteractionKey;
        ExplosionKey = CKeyManager.BombInteractionKey;

        Corgi = CPlayerManager.Instance;

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
        if (CPlayerManager.Instance != null)
            Explosion();

        if(IsEnabled)
        {
            if (bAttachCorgi)
                CUIManager.Instance.SetActiveBombExplosionUI(false);
            else
                CUIManager.Instance.SetActiveBombExplosionUI(true);
        }

        if (bUseBomb&& CWorldManager.Instance.CurrentWorldState == EWorldState.View3D)                  
            CheckInteractionUI();

        if (bAttach)
            AttachForDistance();
    }

    private void CheckInteractionUI()
    {
        if(_isActiveInteractionUI)
        {
            if(CWorldManager.Instance.CurrentWorldState != EWorldState.View3D)
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                _isActiveInteractionUI = false;
            }
            else if(Vector3.Distance(CPlayerManager.Instance.RootObject3D.transform.position, transform.position) > 2.5f)
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                _isActiveInteractionUI = false;
            }
            else if(EPlayerState3D.Idle != CPlayerManager.Instance.Controller3D.CurrentState &&
                    EPlayerState3D.Idle2 != CPlayerManager.Instance.Controller3D.CurrentState &&
                    EPlayerState3D.Idle3 != CPlayerManager.Instance.Controller3D.CurrentState &&
                    EPlayerState3D.Move != CPlayerManager.Instance.Controller3D.CurrentState)
            {
                CUIManager.Instance.SetActiveInteractionUI(false);
                _isActiveInteractionUI = false;
            }
        }
        else
        {
            if (EPlayerState3D.Idle == CPlayerManager.Instance.Controller3D.CurrentState ||
                EPlayerState3D.Idle2 == CPlayerManager.Instance.Controller3D.CurrentState ||
                EPlayerState3D.Idle3 == CPlayerManager.Instance.Controller3D.CurrentState ||
                EPlayerState3D.Move == CPlayerManager.Instance.Controller3D.CurrentState)
            {
                if (Vector3.Distance(CPlayerManager.Instance.RootObject3D.transform.position, transform.position) <= 2.5f)
                {
                    CUIManager.Instance.SetActiveInteractionUI(true);
                    _isActiveInteractionUI = true;
                }
            }
        }
    }


    void AttachForDistance()
    {
        if (Input.GetKeyDown(InteractionKey) && bUseBomb)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View3D)
            {
                ParentBombSpawn.RefreshDestroyActor();
                Actor3DClass.AttachForDistance();
            }
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

        if (null == _bombFireAudioSource)
            _bombFireAudioSource = SoundManager.Instance.PlaySFX(ESFXType.BombFire_0, true);
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
                bCondition = true;
        }
        else if(WorldManager.CurrentWorldState == EWorldState.View2D)
        {
            Vector3 Corgi2DPos = CPlayerManager.Instance.RootObject2D.transform.position;
            if (IsCanChange2D)
                bCondition = true;
        }

        if (Input.GetKeyDown(ExplosionKey) && bCondition && bUseBomb && IsEnabled
            && !CPlayerManager.Instance.GetComponentInChildren<CPlayerState3D_Holding>().enabled)
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
        if(null != _bombFireAudioSource)
        {
            SoundManager.Instance.Stop(_bombFireAudioSource);
            _bombFireAudioSource = null;
            _boomSoundRandomPlayer.Play();
        }

        StartCoroutine(ExplosionV2());
        SetIgnitionFireEffect(false);
        DisableBomb();
    }

    IEnumerator ExplosionV2()
    {
        CUIManager.Instance.SetActiveBombExplosionUI(false);
        RootObject3D.GetComponent<MeshRenderer>().enabled = false;
        IgnitionFireEffect.SetActive(false);
        bUseBomb = false;

        GetComponent<BoxCollider>().isTrigger = true;
        RootObject3D.GetComponent<BoxCollider>().isTrigger = true;
        RootObject2D.GetComponent<BoxCollider2D>().isTrigger = true;

        var IsDestroyActor = new List<GameObject>();

        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomEffectInstance = Instantiate(BoomEffect, transform.position + AddPosition, transform.rotation);

        foreach (var DestroyActor in ParentBombSpawn.destroyObject)
        {
            if (DestroyActor.GetComponentInChildren<MeshRenderer>() != null)
            {
                var Position = DestroyActor.transform.position;
                if (Mathf.Abs(Position.x - transform.position.x) < BoxSize / 2)
                {
                    if (Position.y >= transform.position.y && Mathf.Abs(Position.y - transform.position.y) < BoxSize - 3f)
                    {
                        if (WorldManager.CurrentWorldState == EWorldState.View2D)
                        {
                            DestroyBrokenTile(DestroyActor);
                            IsDestroyActor.Add(DestroyActor);
                        }
                        else
                        {
                            if (Mathf.Abs(Position.z - transform.position.z) < 4)
                            {
                                DestroyBrokenTile(DestroyActor);
                                IsDestroyActor.Add(DestroyActor);
                            }
                        }
                    }
                    else if (Position.y < transform.position.y && Mathf.Abs(Position.y - transform.position.y) < BoxSize - 5f)
                    {
                        if (WorldManager.CurrentWorldState == EWorldState.View2D)
                        {
                            DestroyBrokenTile(DestroyActor);
                            IsDestroyActor.Add(DestroyActor);
                        }
                        else
                        {
                            if (Mathf.Abs(Position.z - transform.position.z) < 4)
                            {
                                DestroyBrokenTile(DestroyActor);
                                IsDestroyActor.Add(DestroyActor);
                            }
                        }
                    }
                }
            }
        }

        foreach (var RemoveArray in IsDestroyActor)
            ParentBombSpawn.destroyObject.Remove(RemoveArray);

        yield return new WaitForSeconds(2.0f);

        transform.position = ParentBombSpawn.transform.position;
        transform.rotation = ParentBombSpawn.transform.rotation;
        RootObject3D.GetComponent<MeshRenderer>().enabled = true;
        ParentBombSpawn.SpawnBomb();

        Destroy(BoomEffectInstance);

        GetComponent<BoxCollider>().isTrigger = false;
        RootObject3D.GetComponent<BoxCollider>().isTrigger = false;
        RootObject2D.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    void DestroyBrokenTile(GameObject other)
    {
        if (other.GetComponentInChildren<Design_BrokenTile>())
        {
            if (!bUseBomb)
                other.GetComponentInChildren<Design_BrokenTile>().DestroyBrokenTile();
        }
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

        //폭탄을 밀기타일 위에 올려놓고 밀어야해서 아래있는 타일에 어태치시킴
        //transform.parent = ParentBombSpawn.transform;
        corgiController3D.ChangeState(EPlayerState3D.PutObjectEnd);
        yield return new WaitUntil(() => corgiController3D.Animator.GetCurrentAnimatorStateInfo(0).IsName("PutObjectEnd"));

        Vector3 startPoint = transform.position;
        RaycastHit hit;
        Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity);

        Vector3 putPoint = transform.position;
        bool isFall = false;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, float.PositiveInfinity))
        {
            putPoint = hit.point + Vector3.up;
            transform.parent = hit.transform;
        }
        else
        {
            putPoint = transform.position + (Vector3.down * 4f);
            transform.parent = null;
            isFall = true;
        }

        float putTime = 0.1f;
        float oneDivAnimationTime = 1f / putTime;
        float addTime = 0f;

        yield return new WaitForSeconds(0.3f);
        while (true)
        {
            addTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPoint, putPoint, Mathf.Clamp(oneDivAnimationTime * addTime, 0f, 1f));

            if (transform.position.Equals(putPoint) || oneDivAnimationTime * addTime > 15)
                break;

            yield return null;
        }

        bAttachCorgi = false;

        if (isFall)
            BeginExplosion();
        else
            EnableBomb();
    }
}
