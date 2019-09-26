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
    


    bool bAttachCorgi;

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

    }

    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);

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


    /*
     * 폭탄을 들고 있는 상태로는 시점전환을 할 수 없다.
     * -> 폭탄을 들고 있어서 마법을 못씀
     * 
     * 시점전환을 실행 중일 때는 폭탄을 터트릴 수 없다.
     * -> 마법에 집중하고 있는 상태여서 폭탄을 못씀
     */
}
