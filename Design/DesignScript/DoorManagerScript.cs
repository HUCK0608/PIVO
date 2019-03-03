using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManagerScript : MonoBehaviour
{
    public int KeyNum;
    public GameObject DoorInDoor;
    public GameObject KeyPrefab;

    private GameObject[] KeyPosArray = new GameObject[4];
    private List<GameObject> KeyObject = new List<GameObject>();

    bool bDoorAnim;

    void Start()
    {
        SetKeyPos();
        SetDoor();
    }

    void Update()
    {
        OpenDoorAnim();
    }



    //EventFunction//



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
            DoorInDoor.transform.position += new Vector3(0, -0.05f, 0);

        if (DoorInDoor.transform.localPosition.y < -9f)
            bDoorAnim = false;
    }

    void OpenDoor()
    {
        GetComponentInChildren<BoxCollider2D>().isTrigger = true;
        GetComponentInChildren<BoxCollider>().isTrigger = true;
        bDoorAnim = true;
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

    public void RefreshDoor()
    {
        if (KeyNum == 4)
        {
            OpenDoor();
        }
        ClearKeyObject();
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
            Destroy(Value);
        }

        KeyObject.Clear();
    }
}
