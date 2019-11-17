using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ViewRectPrototype : MonoBehaviour
{
    public float MaxPressTime = 0.5f;
    public float SetRectSpeed = 1.5f;
    public float IncreaseSpeed = 0.12f;
    public float MaxSize = 20f;

    float DstRectSize, CurRectSize;
    float PressTime;
    float DefaultSpeed;
    int ViewRectDir;
    bool bViewRectSize;
    Vector3 DefaultPos;

    void Start()
    {
        DstRectSize = 0;
        CurRectSize = 0;
        ViewRectDir = 0;
        PressTime = 0;
        bViewRectSize = false;
        DefaultPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (bViewRectSize)
                bViewRectSize = false;
            else
                bViewRectSize = true;
        }

        if (bViewRectSize)
            InitViewRectSize();

    }

    void InitViewRectSize()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
            ViewRectDir = 1;
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            ViewRectDir = -1;
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) ||
                    Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            IncreaseSpeed = DefaultSpeed;

            if (CheckPressTime())
            {
                if (ViewRectDir > 0)
                    DstRectSize = Mathf.Ceil(CurRectSize);
                else
                    DstRectSize = Mathf.Floor(CurRectSize);
            }

            PressTime = 0;
            ViewRectDir = 0;
        }

        if (ViewRectDir != 0)
        {
            if (CheckPressTime() && Mathf.Abs(DstRectSize) < MaxSize)
                DstRectSize += ViewRectDir;
        }

        SetViewRectSize();
    }


    void SetViewRectSize()
    {
        //CurRectSize = Mathf.MoveTowards(CurRectSize, DstRectSize, Time.deltaTime * SetRectSpeed);
        CurRectSize = Mathf.Lerp(CurRectSize, DstRectSize, Time.deltaTime * SetRectSpeed);
        SetRectSpeed += IncreaseSpeed;

        float scalex = transform.localScale.x;
        float scaley = transform.localScale.y;
        transform.localScale = new Vector3(scalex, scaley, CurRectSize);

        float posx = DefaultPos.x;
        float posy = DefaultPos.y;
        float posz = DefaultPos.z;
        float Addz = CurRectSize / 2;
        transform.position = new Vector3(posx, posy, posz + Addz);
    }

    bool CheckPressTime()
    {
        bool ReturnValue = false;

        if (PressTime > MaxPressTime)
            ReturnValue = true;
        else if (PressTime == 0)
            ReturnValue = true;
        else
            ReturnValue = false;

        PressTime += Time.deltaTime;

        return ReturnValue;
    }
}
