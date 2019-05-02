using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Key : MonoBehaviour
{
    [HideInInspector]
    public GameObject DoorManager;

    [HideInInspector]
    public bool AttachCorgi;

    [HideInInspector]
    public bool AttachDoor;
    void Start()
    {
    }

    void Update()
    {
        transform.Rotate(Vector3.up);
        transform.Rotate(Vector3.right);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            DoorManager.GetComponent<Design_DoorManager>().AttachCube(gameObject, other.gameObject);
        }
    }
}
