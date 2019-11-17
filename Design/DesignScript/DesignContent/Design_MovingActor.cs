using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum MovingType { Once, Repeat }
public enum MovingType { Vertical_Z, Horizontal_X }

public class Design_MovingActor : MonoBehaviour
{
    List<Vector3> MovePosArray = new List<Vector3>();

    float DefaultWait;
    int TargetNum;
    bool SwitchOn;

    public bool SetEnabled;
    public float MoveSpeed;
    public float[] WaitTime;
    public MovingType[] MoveSet;



    //InitializeValue



    void Start()
    {
        InitializeValue();
        InitializeMovePos();
    }

    void Update()
    {
        MoveActor();
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
        SwitchOn = false;
        MoveSpeed = MoveSpeed * 0.1f;
        DefaultWait = 0.2f;
    }







    //ProcessMoveActor



    void MoveActor()
    {
        if (SwitchOn)
        {
            Vector3 FirstTarget = MovePosArray[TargetNum];
            Vector3 SecondTarget = MovePosArray[TargetNum];            

            if (MoveSet.Length != 0)
            {
                if (MoveSet[TargetNum-1] == MovingType.Horizontal_X)
                    FirstTarget = new Vector3(MovePosArray[TargetNum].x, transform.position.y, transform.position.z);
                else
                    FirstTarget = new Vector3(transform.position.x, transform.position.y, MovePosArray[TargetNum].z);

                SecondTarget = new Vector3(MovePosArray[TargetNum].x, transform.position.y, transform.position.z);
            }

            if (transform.position != FirstTarget)
                transform.position = Vector3.MoveTowards(transform.position, FirstTarget, MoveSpeed);
            else if (transform.position != SecondTarget)
                transform.position = Vector3.MoveTowards(transform.position, SecondTarget, MoveSpeed);
            else if (transform.position != MovePosArray[TargetNum])
                transform.position = Vector3.MoveTowards(transform.position, MovePosArray[TargetNum], MoveSpeed);
            else
                SwitchOn = false;

        }
    }

    public void OnMovingActor()
    {
        StartCoroutine(OnceNum());
    }







    IEnumerator OnceNum()
    {
        TargetNum++;

        if (WaitTime.Length != 0)
        {
            if (WaitTime[TargetNum-1] < 0)
                yield return new WaitForSeconds(DefaultWait);
            else
                yield return new WaitForSeconds(WaitTime[TargetNum-1]);
        }
        else
            yield return new WaitForSeconds(DefaultWait);

        SwitchOn = true;

    }
}
