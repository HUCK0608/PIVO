using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MonsterController : MonoBehaviour
{
    public CWorldManager WorldManager;
    private GameObject Monster3D, Monster2D;
    private bool bState3D, bState2D;
    void Start()
    {
        Monster3D = transform.Find("3D").gameObject;
        Monster2D = transform.Find("2D").gameObject;
        bState3D = true;
        bState2D = false;
    }

    void Update()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View2D)
            {
                if (bState3D)
                {
                    Monster3D.SetActive(false);
                    Monster2D.SetActive(true);
                    bState2D = true;
                    bState3D = false;
                    Debug.Log("Is2D");
                }
            }
            else
            {
                if (bState2D)
                {
                    Monster3D.SetActive(true);
                    Monster2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    Debug.Log("Is3D");
                }
            }
        }
    }
}
