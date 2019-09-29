using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombController : Design_WorldController
{
    public GameObject BoomEffect;

    [HideInInspector]
    public KeyCode InteractionKey, ExplosionKey;
    [HideInInspector]
    public Design_BombSpawn ParentBombSpawn;

    Design_Bomb3D Actor3DClass;
    Design_Bomb2D Actor2DClass;

    EWorldState CurrentState;
    float ExplosionDistance;
    [HideInInspector]
    public bool bUseBomb = false;



    public override void BeginPlay()
    {
        base.BeginPlay();
        
        Actor3DClass = Actor3D.GetComponent<Design_Bomb3D>();
        Actor3DClass.Controller = GetComponent<Design_BombController>();
        Actor3DClass.BeginPlay();

        Actor2DClass = Actor2D.GetComponent<Design_Bomb2D>();
        Actor2DClass.Controller = GetComponent<Design_BombController>();
        Actor2DClass.BeginPlay();
        Actor2DClass.enabled = false;

        InteractionKey = KeyCode.X;
        ExplosionKey = KeyCode.C;

        CurrentState = WorldManager.CurrentWorldState;
        ExplosionDistance = 15f;
    }

    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);

        CurrentState = CurState;
        if (CurState == EWorldState.View2D)
        {
            Actor2DClass.enabled = true;
            Actor3DClass.enabled = false;
        }
        else if (CurState == EWorldState.View3D)
        {
            Actor3DClass.enabled = true;
            Actor2DClass.enabled = false;
        }
    }







    public override void OnTick()
    {
        base.OnTick();

        Explosion();
    }








    void Explosion()
    {
        bool bCondition = false;
        if (CurrentState == EWorldState.View2D)
        {
            Vector3 Corgi3DPos = CPlayerManager.Instance.RootObject3D.transform.position;
            if (Vector3.Distance(Corgi3DPos, this.transform.position) < ExplosionDistance)
                bCondition = true;
        }
        else if(CurrentState == EWorldState.View3D)
        {
            Vector3 Corgi2DPos = CPlayerManager.Instance.RootObject2D.transform.position;
            if (Vector2.Distance(Corgi2DPos, this.transform.position) < ExplosionDistance)
                bCondition = true;
        }

        if (Input.GetKeyDown(ExplosionKey) && bCondition && bUseBomb)
        {
            this.transform.parent = null;
            StartCoroutine("ExplosionCoroutine");
        }
    }

    IEnumerator ExplosionCoroutine()
    {
        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomInstance = Instantiate(BoomEffect, transform.position + AddPosition, transform.rotation);
        Actor3D.GetComponent<MeshRenderer>().enabled = false;
        bUseBomb = false;

        if (CurrentState == EWorldState.View3D)
        {
            Actor3D.GetComponent<BoxCollider>().isTrigger = true;

            float BoomSize3D = 8;
            Actor3D.GetComponent<BoxCollider>().size = new Vector3(BoomSize3D, BoomSize3D, BoomSize3D);
            Actor3D.GetComponent<BoxCollider>().center = Actor3D.GetComponent<BoxCollider>().center + new Vector3(0, BoomSize3D / 2, 0);
        }

        else if (CurrentState == EWorldState.View2D)
        {
            Actor2D.GetComponent<BoxCollider2D>().isTrigger = true;

            float BoomSize2D = 8;
            Actor2D.GetComponent<BoxCollider2D>().size = new Vector3(BoomSize2D, BoomSize2D, BoomSize2D);
            Actor2D.GetComponent<BoxCollider2D>().offset = Actor2D.GetComponent<BoxCollider2D>().offset + new Vector2(0, BoomSize2D / 2);
        }

        yield return new WaitForSeconds(2);

        this.ParentBombSpawn.SpawnBomb();
        Destroy(BoomInstance);
        Destroy(this.gameObject);
    }



    /*
     * 폭탄을 들고 있는 상태로는 시점전환을 할 수 없다.
     * -> 폭탄을 들고 있어서 마법을 못씀
     * 
     * 시점전환을 실행 중일 때는 폭탄을 터트릴 수 없다.
     * -> 마법에 집중하고 있는 상태여서 폭탄을 못씀
     */
}
