using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveVH { Vertical, Horizontal }

public class PushRemoteScript : MonoBehaviour
{
    public GameObject PushCube;
    public MoveVH MoveDirection;
    List<GameObject> PushTile = new List<GameObject>();

    void Start()
    {
        FindPushTile();
    }

    void Update()
    {
        
    }



    //EventFunction//



    public void CheckPushDir(KeyCode MoveDir)
    {
        bool FirstCondition = false;
        RaycastHit hit;
        Vector3 OriginRight = PushCube.transform.position + new Vector3(2, 0, 0);
        Vector3 OriginBack = PushCube.transform.position + new Vector3(0, 0, -2);
        Vector3 OriginLeft = PushCube.transform.position + new Vector3(-2, 0, 0);
        Vector3 OriginForward = PushCube.transform.position + new Vector3(0, 0, 2);

        if (MoveDirection == MoveVH.Horizontal)
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
        else if (MoveDirection == MoveVH.Vertical)
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
