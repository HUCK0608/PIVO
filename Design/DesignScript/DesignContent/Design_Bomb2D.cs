using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb2D : MonoBehaviour
{
    [HideInInspector]
    public Design_BombController Controller;

    GameObject Corgi;

    public void BeginPlay()
    {
        Corgi = CPlayerManager.Instance.RootObject2D;
    }





    /*---------------
    //Function
    ----------------*/
    public void AttachForDistance()
    {
        float MinDistance = 2.5f;

        if (Vector2.Distance(Corgi.transform.position, Controller.transform.position) < MinDistance)
        {
            Controller.AttachCorgi();
        }
    }


}
