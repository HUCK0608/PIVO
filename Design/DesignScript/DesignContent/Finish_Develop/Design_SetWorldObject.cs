using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_SetWorldObject : MonoBehaviour
{
    CWorldManager WorldManager;
    GameObject Object2D, Object3D;
    bool StateCall, OutViewRect;
    float bUseRectCheck;
    EWorldState BefState;

    void Start()
    {
        StateCall = false;
        OutViewRect = true;
        bUseRectCheck = 0;
        BefState = EWorldState.View3D;
        Object2D = transform.Find("Root2D").gameObject;
        Object3D = transform.Find("Root3D").gameObject;
        WorldManager = GameObject.Find("World").GetComponent<CWorldManager>();
    }

    void Update()
    {
        ChangingState();
        RefreshRectCheck();
    }












    void RefreshRectCheck()
    {
        if (!OutViewRect)
        {
            float WaitTime = 0.5f;
            if (bUseRectCheck < WaitTime)
            {
                bUseRectCheck += Time.deltaTime;
            }
            else if (bUseRectCheck > WaitTime)
            {
                OutViewRect = true;
                bUseRectCheck = 0;
            }

        }
    }

    void ChangingState()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.Changing && StateCall)
            {

                if (!OutViewRect)
                {
                    //시점전환 박스에 들어옴
                    if (BefState == EWorldState.View2D)
                    {
                        //2D -> 3D
                        BefState = EWorldState.View3D;
                        Set3DRender(true);
                        Set2DRender(false);
                    }
                    else if (BefState == EWorldState.View3D)
                    {
                        //3D -> 2D
                        BefState = EWorldState.View2D;
                        Set3DRender(false);
                        Set2DRender(true);
                    }
                }
                else
                {
                    //시점전환 박스에 들어오지 않음
                    if (BefState == EWorldState.View2D)
                    {
                        //2D -> 3D
                        BefState = EWorldState.View3D;
                        Set3DRender(true);
                        Set2DRender(false);
                    }
                    else if (BefState == EWorldState.View3D)
                    {
                        //3D -> 2D
                        BefState = EWorldState.View2D;
                        Set3DRender(false);
                        Set2DRender(false);
                    }
                }

                OutViewRect = true;
                StateCall = false;

            }
            else if (WorldManager.CurrentWorldState != EWorldState.Changing)
            {

                if (!StateCall)
                    StateCall = true;

            }
        }
    }

    void Set3DRender(bool bState)
    {
        Object3D.SetActive(bState);
    }

    void Set2DRender(bool bState)
    {
        Object2D.SetActive(bState);
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

    private void OnTriggerStay(Collider other)
    {
        bUseRectCheck = 0;
    }
}
