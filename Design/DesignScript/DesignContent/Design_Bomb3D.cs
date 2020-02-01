using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb3D : MonoBehaviour
{
    [HideInInspector]
    public Design_BombController Controller;

    GameObject Corgi;
    

    public void BeginPlay()
    {
        Corgi = CPlayerManager.Instance.RootObject3D;
    }





    /*------------------
        Function
    ------------------*/

    Vector3 GetCorgiForward()
    {
        return Corgi.transform.forward;
    }

    public void AttachForDistance()
    {
        float MinDistance = 2.5f;
        float AllowAngle = 0.6f;

        if (Vector3.Distance(Corgi.transform.position, Controller.transform.position) < MinDistance)
        {
            Controller.transform.parent = null;
            Vector3 B2PNormal = (Controller.transform.position - Corgi.transform.position).normalized;
            Vector3 CompareForward = Corgi.transform.forward - B2PNormal;
            float CompareAngle = Mathf.Abs(CompareForward.x + CompareForward.z);

            if (AllowAngle > CompareAngle)
                Controller.AttachCorgi();
        }
    }
    

}
