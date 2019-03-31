using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_MoveSwitch : MonoBehaviour
{
    bool SwitchBool = false;

    public GameObject MovingActor;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && MovingActor && !SwitchBool)
        {
            MovingActor.GetComponent<Design_MovingActor>().IsEnabled = true;
            SwitchBool = true;
        }
    }
}
