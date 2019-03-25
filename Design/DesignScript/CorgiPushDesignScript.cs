using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiPushDesignScript : MonoBehaviour
{
    private bool bOnPushRemote = false;
    private bool bUsePushRemote = false;
    GameObject PushRemoteObject;
    GameObject CenterDown1, CenterDown2;
    Vector3 InteractionStopPos;

    void Start()
    {
    }

    void Update()
    {
        //UsePushRemote();
        //MovePushCube();
    }



    //EventFunction//



    void UsePushRemote()
    {
        if (bOnPushRemote && Input.GetKeyDown(KeyCode.A) && bUsePushRemote == false)
        {
            bUsePushRemote = true;
            InteractionStopPos = transform.parent.transform.position;
            CenterDown2.SetActive(true);
            CenterDown1.SetActive(false);
        }
        else if (bOnPushRemote && Input.GetKeyDown(KeyCode.A) && bUsePushRemote == true)
        {
            bUsePushRemote = false;
            InteractionStopPos = transform.parent.transform.position;
            CenterDown2.SetActive(false);
        }
    }

    void MovePushCube()
    {
        if (bUsePushRemote)
        {
            if (PushRemoteObject)
            {
                //코기이동 멈추기
                transform.parent.transform.position = InteractionStopPos;

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(KeyCode.LeftArrow);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(KeyCode.RightArrow);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(KeyCode.UpArrow);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(KeyCode.DownArrow);
                }
            }
        }
    }
}