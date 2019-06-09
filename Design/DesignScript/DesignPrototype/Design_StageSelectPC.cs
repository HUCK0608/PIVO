using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_StageSelectPC : MonoBehaviour
{
    List<GameObject> MovePosArray = new List<GameObject>();
    int CurMovePos = 0;
    int MaxMovePos = 3;
    void Start()
    {
        InitializeMovePos();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CurMovePos < MaxMovePos)
                CurMovePos++;
            MoveChar(MovePosArray[CurMovePos]);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CurMovePos > 0)
                CurMovePos--;
            MoveChar(MovePosArray[CurMovePos]);
        }
    }

    void InitializeMovePos()
    {
        for (int i = 0; i < MaxMovePos+1; i++)
        {
            MovePosArray.Add(GameObject.Find("MovePos (" + i + ")"));
        }

        Vector3 InitPos = MovePosArray[0].transform.position;
        transform.position = new Vector3(InitPos.x, transform.position.y, InitPos.z);
    }

    void MoveChar(GameObject PosActor)
    {
        Vector3 InitPos = PosActor.transform.position;
        transform.position = new Vector3(InitPos.x, transform.position.y, InitPos.z);
    }

    //코루틴으로 다시 만들어야 할 듯
}
