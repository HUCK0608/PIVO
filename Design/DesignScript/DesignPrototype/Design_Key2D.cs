using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Key2D : MonoBehaviour
{
    Design_Key KeyScript;
    private void Start()
    {
        KeyScript = transform.parent.GetComponent<Design_Key>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            KeyScript.StartCoroutine("AttachDoor");
        }
    }
}
