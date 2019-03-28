using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.up);
        transform.Rotate(Vector3.right);
    }
}
