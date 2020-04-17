using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_RandomTileManager : MonoBehaviour
{
    List<GameObject> TileGroup = new List<GameObject>();
    List<GameObject> MoveTileGroup = new List<GameObject>();
    float XorZ = -9999;
    float TargetPosY;
    float MoveSpeed, RandomRange, WaitTileTime;

    bool bUseTileSelect = false;
    float WaitTileSelect;

    public bool bUseZaxis;


    void Start()
    {
        MoveSpeed = 60f;//100f;//0.8f
        RandomRange = 5f;//5f
        WaitTileTime = 0.15f;//0.15f

        TargetPosY = transform.position.y;
        for (int i = 0; i < transform.childCount; i++)
        {
            TileGroup.Add(transform.GetChild(i).gameObject);
        }
        bUseTileSelect = true;
    }

    void Update()
    {
        if (bUseTileSelect)
        {
            if (WaitTileSelect > WaitTileTime)
            {
                TileSelectTick();
                WaitTileSelect = 0;
            }
            else
                WaitTileSelect += Time.deltaTime;
        }


        if (MoveTileGroup.Count > 1)
        {
            foreach (var Value in MoveTileGroup)
            {
                Vector3 ValuePos = Value.transform.position;
                if (Value.transform.position.y < TargetPosY)
                {
                    Vector3 TargetPos = new Vector3(ValuePos.x, TargetPosY, ValuePos.z);
                    Value.transform.position = Vector3.MoveTowards(ValuePos, TargetPos, MoveSpeed * Time.deltaTime);
                }                    
            }
        }
    }

    void TileSelectTick()
    {
        bool AllClear = false;
        int AllValueCheck = 0;
        float AddValue;

        if (bUseZaxis)
            AddValue = -2;
        else
            AddValue = 2;

        foreach (var Value in TileGroup)
        {
            if (XorZ == -9999)
            {
                if (bUseZaxis)
                    XorZ = Value.transform.position.z;
                else
                    XorZ = Value.transform.position.x;
            }

            bool NotArray = true;
            foreach (var V in MoveTileGroup)
            {
                if (Value == V)
                    NotArray = false;
            }

            if (NotArray)
            {
                if (bUseZaxis)
                {
                    if (XorZ < Value.transform.position.z && Value.transform.position.y != TargetPosY)
                        XorZ = Value.transform.position.z;
                }
                else
                {
                    if (XorZ > Value.transform.position.x && Value.transform.position.y != TargetPosY)
                        XorZ = Value.transform.position.x;
                }
            }


            if (Value.transform.position.y == TargetPosY)
                AllValueCheck++;

            if (AllValueCheck == TileGroup.Count)
                AllClear = true;
        }
        if (!AllClear)
        {
            foreach (var Value in TileGroup)
            {
                float CompareValue;
                if (bUseZaxis)
                    CompareValue = Value.transform.position.z;
                else
                    CompareValue = Value.transform.position.x;

                if (XorZ == CompareValue)
                {
                    MoveTileGroup.Add(Value);

                    Vector3 ValuePos = Value.transform.position;
                    float RandomYValue = ValuePos.y + Random.Range(-RandomRange, RandomRange);
                    Vector3 RandomPos = new Vector3(ValuePos.x, RandomYValue, ValuePos.z);
                    Value.transform.position = RandomPos;
                }
            }
        }
        else
        {
            bUseTileSelect = false;
        }

        XorZ += AddValue;
    }

}
