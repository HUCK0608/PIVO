using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_StageSelectCam : MonoBehaviour
{
    public GameObject TargetActor;
    bool bUseFollowTarget;
    void Start()
    {
        
    }

    void Update()
    {
        if (bUseFollowTarget)
            FollowTarget();
    }

    void FollowTarget()
    {
        transform.position = TargetActor.transform.position;
    }
}
