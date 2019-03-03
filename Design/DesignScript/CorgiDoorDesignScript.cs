using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorgiDoorDesignScript : MonoBehaviour
{
    [HideInInspector]
    public int KeyNum;

    public List<GameObject> KeyObject = new List<GameObject>();

    void Start()
    {
        KeyNum = 0;
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<DoorManagerScript>() != null)
        {
            DoorManagerScript DoorManager = other.GetComponent<DoorManagerScript>();
            DoorManager.KeyNum = DoorManager.KeyNum + KeyNum;
            DoorManager.RefreshDoor();
            KeyNum = 0;
            DestroyKeyObject();
        }
    }



    //EventFunction//



    void DestroyKeyObject()
    {
        foreach (var Value in KeyObject)
        {
            Destroy(Value);
        }

        KeyObject.Clear();
    }
}
