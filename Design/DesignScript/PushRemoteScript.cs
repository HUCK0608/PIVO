using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveVH { Vertical, Horizontal }

public class PushRemoteScript : MonoBehaviour
{
    public GameObject PushCube;
    public MoveVH MoveDirection;

    private bool bOnPushRemote = false;
    private bool bUsePushRemote = false;
    GameObject CenterDown1, CenterDown2, Corgi;
    Vector3 InteractionStopPos;
    Quaternion InteractionStopRot;
    List<GameObject> PushTile = new List<GameObject>();

    void Start()
    {
        FindPushTile();
        InitializeWidget();
    }

    void Update()
    {
        UsePushRemote();
        MovePushCube();
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnterEvent(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerExitEvent(other);
    }



    //EventFunction//





    void UsePushRemote()
    {
        if (bOnPushRemote && Input.GetKeyDown(KeyCode.X) && bUsePushRemote == false)
        {
            bUsePushRemote = true;
            InteractionStopPos = Corgi.transform.position;
            InteractionStopRot = Corgi.transform.rotation;
            //CenterDown2.SetActive(true);
            //CenterDown1.SetActive(false);
        }
        else if (bOnPushRemote && Input.GetKeyDown(KeyCode.X) && bUsePushRemote == true)
        {
            bUsePushRemote = false;
            InteractionStopPos = Corgi.transform.position;
            InteractionStopRot = Corgi.transform.rotation;
            //CenterDown2.SetActive(false);
        }
    }

    void MovePushCube()
    {
        if (bUsePushRemote)
        {
            if (Corgi)
            {
                //코기이동 멈추기
                Corgi.transform.position = InteractionStopPos;
                Corgi.transform.rotation = InteractionStopRot;

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CheckPushDir(KeyCode.LeftArrow);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CheckPushDir(KeyCode.RightArrow);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    CheckPushDir(KeyCode.UpArrow);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CheckPushDir(KeyCode.DownArrow);
                }
            }
        }
    }


    void TriggerEnterEvent(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CPlayerController3D>() != null)
        {
            bOnPushRemote = true;
            Corgi = other.gameObject;
            //CenterDown1.SetActive(true);
            //CenterDown2.SetActive(false);
        }
    }

    void TriggerExitEvent(Collider other)
    {
        if (other.gameObject.GetComponentInParent<CPlayerController3D>() != null)
        {
            bOnPushRemote = false;
            bUsePushRemote = false;
            Corgi = null;
            //CenterDown1.SetActive(false);
        }
    }

    void InitializeWidget()
    {
        //CenterDown1 = GameObject.Find("CenterDown (2)");

        //CenterDown2 = GameObject.Find("CenterDown (1)");
    }

    public void CheckPushDir(KeyCode MoveDir)
    {
        bool FirstCondition = false;
        RaycastHit hit;
        Vector3 OriginRight = PushCube.transform.position + new Vector3(2, 0, 0);
        Vector3 OriginBack = PushCube.transform.position + new Vector3(0, 0, -2);
        Vector3 OriginLeft = PushCube.transform.position + new Vector3(-2, 0, 0);
        Vector3 OriginForward = PushCube.transform.position + new Vector3(0, 0, 2);

        if (MoveDirection == MoveVH.Vertical)
        {
            if (MoveDir == KeyCode.LeftArrow || MoveDir == KeyCode.UpArrow)
            {
                if (Physics.Raycast(OriginForward, Vector3.down, out hit, 2f))
                {
                    FirstCondition = CheckPushTile(hit.transform.parent.gameObject);
                }

                if (Physics.Raycast(OriginRight, Vector3.down, out hit, 2f) && FirstCondition == false)
                {
                    CheckPushTile(hit.transform.parent.gameObject);
                }
            }
            else if (MoveDir == KeyCode.RightArrow || MoveDir == KeyCode.DownArrow)
            {
                if (Physics.Raycast(OriginBack, Vector3.down, out hit, 2f))
                {
                    FirstCondition = CheckPushTile(hit.transform.parent.gameObject);
                }

                if (Physics.Raycast(OriginLeft, Vector3.down, out hit, 2f) && FirstCondition == false)
                {
                    CheckPushTile(hit.transform.parent.gameObject);
                }
            }
        }
        else if (MoveDirection == MoveVH.Horizontal)
        {

            if (MoveDir == KeyCode.LeftArrow || MoveDir == KeyCode.DownArrow)
            {
                if (Physics.Raycast(OriginLeft, Vector3.down, out hit, 2f))
                {
                    FirstCondition = CheckPushTile(hit.transform.parent.gameObject);
                }

                if (Physics.Raycast(OriginBack, Vector3.down, out hit, 2f) && FirstCondition == false)
                {
                    CheckPushTile(hit.transform.parent.gameObject);
                }
            }
            else if (MoveDir == KeyCode.RightArrow || MoveDir == KeyCode.UpArrow)
            {
                if (Physics.Raycast(OriginRight, Vector3.down, out hit, 2f))
                {
                    FirstCondition = CheckPushTile(hit.transform.parent.gameObject);
                }

                if (Physics.Raycast(OriginForward, Vector3.down, out hit, 2f) && FirstCondition == false)
                {
                    CheckPushTile(hit.transform.parent.gameObject);
                }
            }
        }
    }

    bool CheckPushTile(GameObject hitObject)
    {
        foreach (var InstacePushTile in PushTile)
        {
            if (hitObject == InstacePushTile)
            {
                PushCube.transform.position= hitObject.transform.position + new Vector3(0, 2, 0);
                return true;
            }
        }
        return false;
    }

    void FindPushTile()
    {
        RaycastHit hit;
        Vector3 OriginPos = PushCube.transform.position;
        if (Physics.Raycast(OriginPos, Vector3.down, out hit, 2f))
        {
            PushTile.Add(hit.transform.parent.gameObject);
            while (FindFourDirectionTile())
            { }
        }
    }

    bool FindFourDirectionTile()
    {
        bool ReturnValue = false;

        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                if (CheckTile(Vector3.left))
                {
                    ReturnValue = true;
                    break;
                }
            }
            else if (i == 1)
            {
                if (CheckTile(Vector3.right))
                {
                    ReturnValue = true;
                    break;
                }
            }
            else if (i == 2)
            {
                if (CheckTile(Vector3.forward))
                {
                    ReturnValue = true;
                    break;
                }
            }
            else if (i == 3)
            {
                if (CheckTile(Vector3.back))
                {
                    ReturnValue = true;
                    break;
                }
            }
        }

        return ReturnValue;
    }

    bool CheckTile(Vector3 Direction)
    {
        bool ReturnValue = false;
        int PushTileLength = PushTile.Count;
        Vector3 OriginPos = PushTile[PushTileLength - 1].transform.position;
        RaycastHit hit;

        if (Physics.Raycast(OriginPos, Direction, out hit, 2f))
        {
            string ParentName = hit.transform.parent.name;
            string[] SplitTXT = ParentName.Split(' ');

            if (SplitTXT[0] == "PushTile")
            {
                bool IsReturn = true;
                foreach (var InstancePushTile in PushTile)
                {
                    if (InstancePushTile == hit.transform.parent.gameObject)
                    {
                        IsReturn = false;
                        break;
                    }
                }

                if (IsReturn)
                {
                    PushTile.Add(hit.transform.parent.gameObject);
                    ReturnValue = true;
                }
            }
        }

        return ReturnValue;
    }
}
