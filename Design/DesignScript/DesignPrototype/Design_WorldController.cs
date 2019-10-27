using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_WorldController : MonoBehaviour
{
    [HideInInspector]
    public CWorldManager WorldManager;
    [HideInInspector]
    public GameObject Actor3D, Actor2D;

    private EWorldState BeforeState = EWorldState.View3D;

    [HideInInspector]
    public bool bState3D, bState2D, OutViewRect, bChanging, bShow;

    public virtual void BeginPlay() { }
    public virtual void OnTick() { }
    public virtual void ChangeWorld(EWorldState CurState) { }
    public virtual void BeginChange() { }

    void Start()
    {
        Initialize();
        BeginPlay();
    }

    void Update()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.Changing)
            {
                SetWorldState(BeforeState);
                bChanging = true;
            }
            else
            {
                bChanging = false;
            }
        }

        OnTick();
    }








    void Initialize()
    {
        WorldManager = CWorldManager.Instance.GetComponent<CWorldManager>();
        Actor3D = transform.Find("Root3D").gameObject;
        Actor2D = transform.Find("Root2D").gameObject;

        bState3D = true;
        bState2D = true;
        bShow = true;
        OutViewRect = true;
        bChanging = false;

        Actor2D.SetActive(false);
    }

    public void SetWorldStateCustom(EWorldState CurState)
    {
        Initialize();
        BeginPlay();

        if (CurState == EWorldState.View3D)
        {
            bState3D = true;
            bState2D = true;
            bShow = true;
            OutViewRect = false;
            bChanging = false;
            BeforeState = EWorldState.View3D;
        }
        else if (CurState == EWorldState.View2D)
        {
            bState3D = true;
            bState2D = true;
            bShow = true;
            OutViewRect = false;
            bChanging = false;
            BeforeState = EWorldState.View2D;
        }
    }

    void SetWorldState(EWorldState CurState)
    {
        if (CurState == EWorldState.View3D && !bChanging)
        {
            if (bState3D)//2D로 바꾸기
            {
                ChangeWorld(EWorldState.View2D);

                BeforeState = EWorldState.View2D;
                bState2D = true;
                bState3D = false;

                if (OutViewRect)
                {
                    bShow = false;
                    SetAllActorActive(false, false);
                }
                else
                {
                    bShow = true;
                    if (Actor2D.GetComponent<SpriteRenderer>())
                        StartCoroutine(WaitAndChangeObject());
                    else
                        SetAllActorActive(true, true);
                }

            }
        }
        else if (CurState == EWorldState.View2D && !bChanging)
        {
            if (bState2D)//3D로 바꾸기
            {
                ChangeWorld(EWorldState.View3D);
                BeforeState = EWorldState.View3D;
                Actor3D.SetActive(true);
                Actor2D.SetActive(false);
                bState3D = true;
                bState2D = false;
                OutViewRect = true;
                bShow = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = true;
        }
    }

    void SetAllActorActive(bool Show3D, bool Show2D)
    {
        Actor3D.SetActive(Show3D);
        Actor2D.SetActive(Show2D);
    }

    IEnumerator WaitAndChangeObject()
    {
        yield return new WaitForSeconds(0.5f);
        SetAllActorActive(false, true);
    }

}
