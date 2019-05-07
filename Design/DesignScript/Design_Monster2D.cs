using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Monster2D : MonoBehaviour
{
    private GameObject Corgi, Monster3D;
    private float LookValue, RayLength;

    //InitializeValue



    void Start()
    {
        InitializeValue();
    }

    void Update()
    {
        CheckRaycast();
    }

    //EventFunction







    void InitializeValue()
    {
        LookValue = transform.parent.forward.z;
        RayLength = transform.parent.Find("3D").GetComponent<BoxCollider>().size.x - 1f;
    }

    void CheckRaycast()
    {
        Vector2 LookVector = new Vector2(-LookValue, 0);
        Vector3 OriginPos = transform.position + Vector3.up;
        RaycastHit2D hit = Physics2D.Raycast(OriginPos, LookVector, RayLength);

        if (hit)
        {
            if (hit.collider.gameObject.layer == 10)
            {
                Debug.Log("bCheckPlayer");
            }
        }        
    }
}
