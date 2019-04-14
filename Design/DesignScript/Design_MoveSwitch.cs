using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MoveSwitch : MonoBehaviour
{
    bool SwitchBool = false;

    public GameObject[] MovingActor = new GameObject[] { };

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && MovingActor[0] && !SwitchBool)
        {
            foreach (var V in MovingActor)
            {
                V.GetComponent<Design_MovingActor>().OnMovingActor();
            }
            SwitchBool = true;
        }
    }
}
