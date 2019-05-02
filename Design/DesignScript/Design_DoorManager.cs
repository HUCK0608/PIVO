using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_DoorManager : MonoBehaviour
{
    public GameObject DoorInDoor, DoorInDoor2;
    public GameObject KeyPrefab;
    
    [Header("ForOpenDoor")]
    public GameObject[] ChildKey = new GameObject[] { };

    private bool IsOpen;
    private int KeyNum;
    private int AttachCount;
    private GameObject[] KeyPosArray = new GameObject[4];
    private List<GameObject> KeyObject = new List<GameObject>();    

    bool bDoorAnim;

    void Start()
    {
        SetKey();
        SetKeyPos();
        SetDoor();
    }

    void Update()
    {
        OpenDoorAnim();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == 10 && !IsOpen)
        {
            foreach (var Value in ChildKey)
            {
                Design_Key Script = Value.GetComponent<Design_Key>();
                if (Script.AttachCorgi && !Script.AttachDoor)
                {
                    KeyNum++;
                    AttachCount--;
                    Value.SetActive(false);
                    Script.AttachDoor = true;
                }
            }
            RefreshDoor();
        }
    }



    //EventFunction//



    void SetKey()
    {
        foreach (var Value in ChildKey)
        {
            Value.GetComponent<Design_Key>().DoorManager = gameObject;
        }

        KeyNum = 4 - ChildKey.Length;
    }

    void SetKeyPos()
    {
        for (int i = 0; i < 4; i++)
        {
            KeyPosArray[i] = GameObject.Find("KeyPos " + "(" + i + ")");
        }
    }

    void OpenDoorAnim()
    {
        if (bDoorAnim)
        {
            DoorInDoor.transform.position += new Vector3(0, -0.05f, 0);
            DoorInDoor2.transform.position += new Vector3(0, -0.05f, 0);
        }

        if (DoorInDoor.transform.localPosition.y < -9f)
            bDoorAnim = false;
    }

    void OpenDoor()
    {
        GetComponentInChildren<BoxCollider>().isTrigger = true;
        bDoorAnim = true;
        IsOpen = true;
    }

    void SetDoor()
    {
        if (KeyNum == 4)
        {
            OpenDoor();
        }
        else
        {
            for (int i = 0; i < KeyNum; i++)
            {
                GameObject InstObject = Instantiate(KeyPrefab, KeyPosArray[i].transform, false);
                InstObject.transform.localPosition = Vector3.zero;
                KeyObject.Add(InstObject);
            }
        }
    }

    void RefreshDoor()
    {
        if (KeyNum == 4)
        {
            OpenDoor();
        }
        ClearKeyObject();
        Debug.Log(KeyNum);
        for (int i = 0; i < KeyNum; i++)
        {
            GameObject InstObject = Instantiate(KeyPrefab, KeyPosArray[i].transform, false);
            InstObject.transform.localPosition = Vector3.zero;
            KeyObject.Add(InstObject);
        }
    }

    void ClearKeyObject()
    {
        foreach(var Value in KeyObject)
        {
            Value.SetActive(false);
        }

        KeyObject.Clear();
    }

    public void AttachCube(GameObject Key, GameObject Corgi)
    {
        AttachCount++;
        Key.GetComponent<Design_Key>().AttachCorgi = true;
        Key.transform.parent = Corgi.transform;
        Key.transform.localPosition = new Vector3(0, 0.75f, -1.2f * AttachCount);
    }
}
