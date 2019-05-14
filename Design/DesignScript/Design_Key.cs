using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Key : MonoBehaviour
{
    [HideInInspector]
    public GameObject DoorManager;

    [HideInInspector]
    public Vector3 TargetPos;

    [HideInInspector]
    public float AttachSpeed;
    void Start()
    {
    }

    void Update()
    {
        RotateKey();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            StartCoroutine("AttachDoor");
        }
    }

    void RotateKey()
    {
        transform.Rotate(Vector3.up);
        transform.Rotate(Vector3.right);
    }
    
    IEnumerator AttachDoor()
    {
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, 0.2f);
            if (transform.position == TargetPos)
            {
                DoorManager.GetComponent<Design_DoorManager>().AddKeyNum();           
                break;
            }

            yield return null;
        }
    }
}
