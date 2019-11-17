using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_LookTarget : MonoBehaviour
{
    public GameObject TargetObject;

    void Update()
    {
        if (TargetObject != null)
            transform.LookAt(TargetObject.transform);
    }
}
