using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_StageSelectPC : MonoBehaviour
{
    List<GameObject> MovePosArray = new List<GameObject>();
    Design_StageSelectCam CameraActorComponent;
    int CurMovePos = 0;
    int MaxMovePos = 3;
    Vector3 TargetActorPos;
    bool bMoving = false;
    void Start()
    {
        InitializeMovePos();
        InitializeCameraActor();
    }

    void Update()
    {
        if (!bMoving)
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

            if (CurMovePos == 2)
                CameraActorComponent.UseDialogueCamera();
            else
                CameraActorComponent.FollowCharacterCamera();
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

    void MoveChar(GameObject InputPosActor)
    {
        bMoving = true;
        Vector3 InitPos = InputPosActor.transform.position;
        TargetActorPos = new Vector3(InitPos.x, transform.position.y, InitPos.z);
        StartCoroutine("MoveToPos");
        //transform.position = new Vector3(InitPos.x, transform.position.y, InitPos.z);
    }

    IEnumerator MoveToPos()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetActorPos, 0.3f);

            if (transform.position == TargetActorPos)
            {
                bMoving = false;
                break;
            }

            yield return null;
        }
    }



    void InitializeCameraActor()
    {
        CameraActorComponent = GameObject.Find("Camera_Prototype").GetComponent<Design_StageSelectCam>();
    }
}
