using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_RandomTileManager : MonoBehaviour
{
    List<GameObject> TileGroup = new List<GameObject>();
    List<GameObject> MoveTileGroup = new List<GameObject>();
    float PosX = -999f;
    float TargetPosY;
    float MoveSpeed, RandomRange, WaitTileTime;
    

    void Start()
    {
        MoveSpeed = 0.8f;
        RandomRange = 5f;
        WaitTileTime = 0.15f;
        
        TargetPosY = transform.position.y;
        for (int i = 0; i < transform.childCount; i++)
        {
            TileGroup.Add(transform.GetChild(i).gameObject);
        }
        StartCoroutine("TileSelect");
    }

    void Update()
    {
        if (MoveTileGroup.Count > 1)
        {
            foreach (var Value in MoveTileGroup)
            {
                Vector3 ValuePos = Value.transform.position;
                if (Value.transform.position.y < TargetPosY)
                {
                    Vector3 TargetPos = new Vector3(ValuePos.x, TargetPosY, ValuePos.z);
                    Value.transform.position = Vector3.MoveTowards(ValuePos, TargetPos, MoveSpeed);
                }                    
            }
        }
    }

    IEnumerator TileSelect()
    {
        while (true)
        {
            bool AllClear = false;
            int AllValueCheck = 0;
            foreach (var Value in TileGroup)
            {
                if (PosX == -999)
                {
                    PosX = Value.transform.position.x;
                }
                
                bool NotArray = true;
                foreach (var V in MoveTileGroup)
                {
                    if (Value == V)
                        NotArray = false;
                }

                if (NotArray)
                {
                    if (PosX > Value.transform.position.x && Value.transform.position.y != TargetPosY)
                        PosX = Value.transform.position.x;
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
                    if (PosX == Value.transform.position.x)
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
                break;

            PosX += 2;
            yield return new WaitForSeconds(WaitTileTime);
        }
    }
}
