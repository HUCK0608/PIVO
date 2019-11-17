using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Key : MonoBehaviour
{
    private CWorldManager WorldManager;
    private GameObject Key2D;
    private bool bState3D, bState2D, OutViewRect;

    [HideInInspector]
    public GameObject DoorManager;

    [HideInInspector]
    public Vector3 TargetPos;

    [HideInInspector]
    public float AttachSpeed;


    void Start()
    {
        bState3D = true;
        bState2D = false;
        OutViewRect = true;
        Key2D = transform.Find("2D").gameObject;
        WorldManager = GameObject.Find("World").GetComponent<CWorldManager>();
    }

    void Update()
    {
        SetWorld();
        RotateKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            StartCoroutine("AttachDoor");
        }
        else if (other.gameObject.layer == 8)
        {
            OutViewRect = false;
        }
    }



    void SetWorld()
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
                        Key2D.SetActive(false);
                    }
                    else
                        Key2D.SetActive(true);
                }
            }
            else
            {
                if (bState2D)//3D로 바꾸기
                {
                    Set3DRender(true);
                    Key2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    OutViewRect = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = true;
        }
    }

    void RotateKey()
    {
        transform.Rotate(Vector3.up);
        transform.Rotate(Vector3.right);
    }
    
    IEnumerator AttachDoor()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.2f);
            if (transform.position == TargetPos)
            {
                DoorManager.GetComponent<Design_DoorManager>().AddKeyNum();           
                break;
            }

            yield return null;
        }
    }

    void Set3DRender(bool bState)
    {
        GetComponent<MeshRenderer>().enabled = bState;
    }
}
