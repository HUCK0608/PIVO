using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MovingActor : MonoBehaviour
{
    List<Vector3> MovePosArray = new List<Vector3>();

    int TargetNum;

    void Start()
    {
        InitializeMovePos();
    }

    void Update()
    {
        ProcessMoveActor();
    }


    //EventFunction



    void ProcessMoveActor()
    {
        CheckTargetNum();
        OneMove();
    }

    void InitializeMovePos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.Find("Pos (" + i + ")") != null)
            {
                MovePosArray.Add(transform.Find("Pos (" + i + ")").gameObject.transform.position);
                Debug.Log(MovePosArray[i]);
            }
        }
    }

    void CheckTargetNum()
    {
        foreach (var v in MovePosArray)
        {
            if (v == transform.position)
            {
                TargetNum++;
                if (TargetNum > MovePosArray.Count-1)
                    TargetNum = 0;
                break;
            }
        }
    }

    void OneMove()
    {
        transform.position = transform.position + Vector3.up;
        //transform.position = Vector3.MoveTowards(transform.position, MovePosArray[TargetNum], 0.2f);
    }
}
