using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovingType { Once, Repeat }

public class Design_MovingActor : MonoBehaviour
{
    List<Vector3> MovePosArray = new List<Vector3>();

    int TargetNum;
    int MoveDir;
    bool bWait;

    [HideInInspector]
    public bool IsEnabled;

    public bool SetEnabled;
    public float MoveSpeed;
    public float WaitTime;
    public MovingType CurMoveType;




    //InitializeValue



    void Start()
    {
        InitializeValue();
        InitializeMovePos();
    }

    void Update()
    {
        ProcessMoveActor();
    }




    //EventFunction




    void InitializeMovePos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.Find("Pos (" + i + ")") != null)
            {
                MovePosArray.Add(transform.Find("Pos (" + i + ")").gameObject.transform.position);
            }
        }
    }

    void InitializeValue()
    {
        TargetNum = 0;
        MoveDir = 1;
        bWait = false;
        IsEnabled = SetEnabled;
        MoveSpeed = MoveSpeed * 0.1f;
    }







    //ProcessMoveActor


    void ProcessMoveActor()
    {
        if (IsEnabled)
        {
            MoveActor();
            SetTargetNum(CurMoveType);
        }
    }

    void SetTargetNum(MovingType MoveType)
    {
        if (!bWait)
        {
            foreach (var v in MovePosArray)
            {
                if (v == transform.position)
                {
                    if (MoveType == MovingType.Once)
                        StartCoroutine("OnceNum");
                    else if (MoveType == MovingType.Repeat)
                        StartCoroutine("RepeatNum");

                    bWait = true;
                    break;
                }
            }
        }
    }

    void MoveActor()
    {
        if (!bWait)
        {
            transform.position = Vector3.MoveTowards(transform.position, MovePosArray[TargetNum], MoveSpeed);
        }
    }







    //SetNum

    void SetNum()
    {
        bWait = false;
        TargetNum+= MoveDir;
    }

    void ChangeMoveDir()
    {
        if (TargetNum > MovePosArray.Count - 1)
        {
            MoveDir = -1;
            TargetNum -= 2;
        }
        else if (TargetNum < 0)
        {
            MoveDir = 1;
            TargetNum += 2;
        }
    }

    IEnumerator RepeatNum()
    {
        yield return new WaitForSeconds(WaitTime);
        SetNum();
        ChangeMoveDir();
    }

    IEnumerator OnceNum()
    {
        yield return new WaitForSeconds(WaitTime);
        SetNum();
        IsEnabled = false;

        ChangeMoveDir();
    }
}
