using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_SetWorldObject : MonoBehaviour
{
    CWorldManager WorldManager;
    GameObject Object2D, Object3D;
    bool bState3D, bState2D, OutViewRect;

    void Start()
    {

        bState3D = true;
        bState2D = false;
        OutViewRect = true;
        Object2D = transform.Find("Root2D").gameObject;
        Object3D = transform.Find("Root3D").gameObject;
        WorldManager = GameObject.Find("World").GetComponent<CWorldManager>();
    }

    void Update()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View2D)
            {
                if (bState3D)//2D로 바꾸기
                {
                    //Set3DRender(false);
                    bState2D = true;
                    bState3D = false;

                    if (OutViewRect)
                    {
                        Set3DRender(false);
                        //Object2D.SetActive(false);
                    }
                    //else
                    //Object2D.SetActive(true);
                }
            }
            else
            {
                if (bState2D)//3D로 바꾸기
                {
                    Set3DRender(true);
                    //Object2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    OutViewRect = true;
                }
            }
        }
    }

    void Set3DRender(bool bState)
    {
        Object3D.SetActive(bState);
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
}
