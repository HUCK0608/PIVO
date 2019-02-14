using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiDesignScript : MonoBehaviour
{
    private bool bOnPushRemote = false;
    private bool bUsePushRemote = false;
    GameObject PushRemoteObject;
    Vector3 InteractionStopPos;

    void Start()
    {
        
    }

    void Update()
    {
        UsePushRemote();
        MovePushCube();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PushRemoteScript>() != null)
        {
            bOnPushRemote = true;
            PushRemoteObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PushRemoteScript>() != null)
        {
            bOnPushRemote = false;
            bUsePushRemote = false;
            PushRemoteObject = null;
        }
    }



    //EventFunction//



    void UsePushRemote()
    {
        if (bOnPushRemote && Input.GetKeyDown(KeyCode.A) && bUsePushRemote == false)
        {
            bUsePushRemote = true;
            InteractionStopPos = transform.parent.transform.position;
        }
        else if (bOnPushRemote && Input.GetKeyDown(KeyCode.A) && bUsePushRemote == true)
        {
            bUsePushRemote = false;
            InteractionStopPos = transform.parent.transform.position;
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

                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(false);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    PushRemoteObject.GetComponent<PushRemoteScript>().CheckPushDir(true);
                }
            }
        }
    }
}
