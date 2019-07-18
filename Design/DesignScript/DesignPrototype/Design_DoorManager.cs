using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_DoorManager : MonoBehaviour
{
    public CWorldManager WorldManager;
    public GameObject DoorInDoor, DoorInDoor2, KeyPrefab;
    public float AttachKeySpeed;

    [Header("RegistKeyObject")]
    public GameObject[] KeyObject = new GameObject[] { };
    
    private GameObject[] KeyPosArray = new GameObject[4];
    private GameObject Door2D, Door3D;
    private int KeyNum;
    private bool IsOpen;
    private bool bState3D, bState2D, OutViewRect;


    //InitializeValue








    private void Start()
    {
        SetKeyPos();
        SetKeyScript();
        SetDoorState();
        bState3D = true;
        bState2D = false;
        OutViewRect = true;
        Door2D = transform.Find("Root2D").gameObject;
        Door3D = transform.Find("Root3D").gameObject;
    }

    private void Update()
    {
        OpenDoorAnim();
        SetWorld();
    }


    //EventFunction






    void SetWorld()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View2D)
            {
                if (bState3D)//2D로 바꾸기
                {
                    //Set3DRender(false);
                    bState2D = true;
                    bState3D = false;

                    if (OutViewRect)
                    {
                        Set3DRender(false);
                        Door2D.SetActive(false);
                    }
                    else
                        Door2D.SetActive(true);
                }
            }
            else
            {
                if (bState2D)//3D로 바꾸기
                {
                    Set3DRender(true);
                    Door2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    OutViewRect = true;
                }
            }
        }
    }

    void SetKeyPos()
    {
        int KeyObjectNum = 0;
        KeyNum = 4 - KeyObject.Length;

        for (int i = 0; i < 4; i++)
        {
            KeyPosArray[i] = transform.Find("KeyPos " + "(" + i + ")").gameObject;

            if (i < KeyNum)
            {
                InitializeKeyPrefab(i);
            }
            else
            {
                Design_Key KeyScript = KeyObject[KeyObjectNum].GetComponent<Design_Key>();
                KeyScript.TargetPos = KeyPosArray[i].transform.position;
                KeyScript.AttachSpeed = AttachKeySpeed;
                KeyObjectNum++;
            }
        }
    }

    void SetKeyScript()
    {
        foreach (var Value in KeyObject)
        {
            Value.GetComponent<Design_Key>().DoorManager = gameObject;
        }
    }

    void InitializeKeyPrefab(int LocationNum)
    {        
        GameObject InstObject = Instantiate(KeyPrefab, KeyPosArray[LocationNum].transform, false);
        InstObject.transform.localPosition = Vector3.zero;
    }

    void SetDoorState()
    {
        if (KeyNum == 4)
            OpenDoor();
    }

    void OpenDoorAnim()
    {
        if (IsOpen)
        {
            DoorInDoor.transform.position += new Vector3(0, -0.05f, 0);
            DoorInDoor2.transform.position += new Vector3(0, -0.05f, 0);
        }

        if (DoorInDoor.transform.localPosition.y < -9f)
            IsOpen = false;
    }

    void OpenDoor()
    {
        GetComponentInChildren<BoxCollider>().isTrigger = true;
        IsOpen = true;
    }

    public void AddKeyNum()
    {
        KeyNum++;
        SetDoorPattern();
        SetDoorState();
    }
    void Set3DRender(bool bState)
    {
        Door3D.SetActive(bState);
        foreach (var v in KeyPosArray)
        {
            if (v.transform.childCount > 0)
                v.transform.GetChild(0).gameObject.SetActive(bState);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = true;
        }
    }

    void SetDoorPattern()
    {
        StartCoroutine("AddPattern");
    }

    IEnumerator AddPattern()
    {
        float CurPatternFill = 0;
        float TargetFill = 0;

        if (KeyNum == 1)
        {
            CurPatternFill = 0;
            TargetFill = 1;
        }
        else if (KeyNum == 2)
        {
            CurPatternFill = 1;
            TargetFill = 1.6f;
        }
        else if (KeyNum == 3)
        {
            CurPatternFill = 1.6f;
            TargetFill = 2f;
        }
        else if (KeyNum == 4)
        {
            CurPatternFill = 2f;
            TargetFill = 6f;
        }

        GameObject PatternObject = Door3D.transform.Find("Activate_Door").Find("Activate_Door_Pattern").gameObject;
        Material PatternMat = PatternObject.GetComponent<Renderer>().material;

        while (true)
        {
            CurPatternFill += 0.008f;
            PatternMat.SetFloat("_PatternFill", CurPatternFill);

            if (CurPatternFill >= TargetFill)
                break;

            yield return null;
        }
    }
}
