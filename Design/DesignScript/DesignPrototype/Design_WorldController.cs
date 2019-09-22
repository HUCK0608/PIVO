using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_WorldController : MonoBehaviour
{
    private CWorldManager WorldManager;
    private GameObject Actor3D, Actor2D;

    private EWorldState BeforeState = EWorldState.View3D;

    private bool bState3D, bState2D, OutViewRect, IsChanging;

    void Start()
    {
        WorldManager = GameObject.Find("World").GetComponent<CWorldManager>();
        Actor3D = transform.Find("Root3D").gameObject;
        Actor2D = transform.Find("Root2D").gameObject;

        bState3D = true;
        bState2D = false;
        OutViewRect = true;

    }

    void Update()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.Changing)
            {
                if (BeforeState == EWorldState.View3D && !IsChanging)
                {
                    if (bState3D)//2D로 바꾸기
                    {
                        if (Actor2D.GetComponent<SpriteRenderer>())
                            SetAllActorActive(true, false);

                        BeforeState = EWorldState.View2D;
                        bState2D = true;
                        bState3D = false;

                        if (OutViewRect)
                            SetAllActorActive(false, false);
                        else
                            Actor2D.SetActive(true);
                    }
                }
                else if (BeforeState == EWorldState.View2D && !IsChanging)
                {
                    if (bState2D)//3D로 바꾸기
                    {
                        BeforeState = EWorldState.View3D;
                        Actor3D.SetActive(true);
                        Actor2D.SetActive(false);
                        bState3D = true;
                        bState2D = false;
                        OutViewRect = true;
                    }
                }

                IsChanging = true;
            }
            else
                IsChanging = false;
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

    void SetAllActorActive(bool A, bool B)
    {
        Actor3D.SetActive(A);
        Actor2D.SetActive(B);
    }

}
