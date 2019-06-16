using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_FollowTarget : MonoBehaviour
{
    public GameObject Target;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Target.transform.position;
    }
}
