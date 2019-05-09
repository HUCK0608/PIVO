using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_DoorManager : MonoBehaviour
{
    public GameObject DoorInDoor, DoorInDoor2, KeyPrefab;
    public float AttachKeySpeed;

    [Header("RegistKeyObject")]
    public GameObject[] KeyObject = new GameObject[] { };
    
    private GameObject[] KeyPosArray = new GameObject[4];
    private int KeyNum;
    private bool IsOpen;


    //InitializeValue








    private void Start()
    {
        SetKeyPos();
        SetKeyScript();
        SetDoorState();
    }

    private void Update()
    {
        OpenDoorAnim();
    }


    //EventFunction








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
        SetDoorState();
    }
}
