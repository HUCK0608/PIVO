using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MonsterController : MonoBehaviour
{
    public CWorldManager WorldManager;
    public Vector2 CollisionSize;

    private GameObject Monster3D, Monster2D;
    private bool bState3D, bState2D, OutViewRect;
    void Start()
    {
        Monster3D = transform.Find("3D").gameObject;
        Monster2D = transform.Find("2D").gameObject;

        Monster3D.GetComponent<Design_Monster3D>().InitializeValue();
        Monster2D.GetComponent<Design_Monster2D>().InitializeValue();

        bState3D = true;
        bState2D = false;
        OutViewRect = true;

        Vector3 SetCollisionSize = new Vector3(CollisionSize.x*2, 1, CollisionSize.y*2);
        Monster3D.GetComponent<Design_Monster3D>().SetCollisionSize(SetCollisionSize);
    }

    void Update()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View2D)
            {
                if (bState3D)//2D로 바꾸기
                {
                    Monster3D.SetActive(false);
                    bState2D = true;
                    bState3D = false;

                    if (OutViewRect)
                        Monster2D.SetActive(false);
                    else
                        Monster2D.SetActive(true);
                }
            }
            else
            {
                if (bState2D)//3D로 바꾸기
                {
                    Monster3D.SetActive(true);
                    Monster2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    OutViewRect = true;
                }
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
}
