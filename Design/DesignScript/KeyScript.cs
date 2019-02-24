using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    GameObject Corgi;

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
        if (Corgi == null)
        {
            Corgi = other.gameObject;
            transform.parent = Corgi.transform;
            transform.localPosition = Vector3.zero + new Vector3(0, 1, -2);
        }
    }
}
