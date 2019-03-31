using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Design_FindTileError : MonoBehaviour
{
    public bool bCheckOverlap = false;
    public bool bCheckRot = false;
    public bool bCheckPos = false;
    public bool bRoundPos = false;
    public bool bCheckBool = false;

    void Update()
    {
        CheckTilePos();
        CheckTileRot();
        CheckOverlap();
        SetRoudPos();
        CheckBoolean();
    }


    void SetRoudPos()
    {
        if (bRoundPos)
        {
            int ChildNum = transform.childCount;

            for (int i = 0; i < ChildNum; i++)
            {
                float X = transform.GetChild(i).transform.position.x;
                float Y = transform.GetChild(i).transform.position.y;
                float Z = transform.GetChild(i).transform.position.z;

                X = Mathf.Round(X);
                Y = Mathf.Round(Y);
                Z = Mathf.Round(Z);

                transform.GetChild(i).transform.position = new Vector3(X, Y, Z);
            }
            bRoundPos = false;
            Debug.Log("Finish");
        }
    }


    void CheckOverlap()
    {
        if (bCheckOverlap)
        {
            int ChildNum = transform.childCount;

            for (int i = 0; i < ChildNum; i++)
            {
                for (int j = 0; j < ChildNum; j++)
                {
                    if (i != j && transform.GetChild(i).transform.position == transform.GetChild(j).transform.position)
                    {
                        Debug.Log(transform.GetChild(i).name + " : " + transform.GetChild(j).name);
                    }
                }
            }
            bCheckOverlap = false;
            Debug.Log("Finish");
        }
    }

    void CheckTileRot()
    {
        if (bCheckRot)
        {
            int ChildNum = transform.childCount;

            for (int i = 0; i < ChildNum; i++)
            {
                bool IsError = false;
                float X = transform.GetChild(i).transform.rotation.x;
                float Y = transform.GetChild(i).transform.rotation.y;
                float Z = transform.GetChild(i).transform.rotation.z;

                if (X != 0)
                    IsError = true;
                else if (Y != 0)
                    IsError = true;
                else if (Z != 0)
                    IsError = true;

                if (IsError)
                {
                    Debug.Log(transform.GetChild(i).name);
                }
            }
            bCheckRot = false;
            Debug.Log("Finish");
        }
    }


    void CheckTilePos()
    {
        if (bCheckPos)
        {
            int ChildNum = transform.childCount;

            for (int i = 0; i < ChildNum; i++)
            {
                bool IsError = false;
                float X = transform.GetChild(i).transform.position.x;
                float Y = transform.GetChild(i).transform.position.y;
                float Z = transform.GetChild(i).transform.position.z;

                if (X % 2 != 0)
                    IsError = true;
                else if (Y % 2 != 0)
                    IsError = true;
                else if (Z % 2 != 0)
                    IsError = true;

                if (IsError)
                {
                    Debug.Log(transform.GetChild(i).name);
                }
            }
            bCheckPos = false;
            Debug.Log("Finish");
        }
    }

    void CheckBoolean()
    {
        if(bCheckBool)
        {
            int ChildNum = transform.childCount;

            for (int i = 0; i < ChildNum; i++)
            {
                bool IsError = false;
                List<bool> CheckBoolean = new List<bool>();
                CheckBoolean.Add(transform.GetChild(i).GetComponent<Design_MaterialChange>().RotPlus);
                CheckBoolean.Add(transform.GetChild(i).GetComponent<Design_MaterialChange>().RotMinus);
                CheckBoolean.Add(transform.GetChild(i).GetComponent<Design_MaterialChange>().RotZero);
                

                foreach (var BoolValue in CheckBoolean)
                {
                    if (BoolValue)
                        IsError = true;
                }

                if (IsError)
                {
                    Debug.Log(transform.GetChild(i).name);
                }
            }
            bCheckBool = false;
            Debug.Log("Finish");
        }
    }

}
