using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EConveyDirection{ None, Left, Right, Up, Down }
public class Design_Convey : Design_WorldController
{
    public void ConveyPower(bool Is3D, EConveyDirection EConveyDir)
    {
        Vector3 ConveyDir = Vector3.zero;
        float RayDistance = 5f;

        if (EConveyDir == EConveyDirection.Left)
            ConveyDir = Vector3.left;
        else if (EConveyDir == EConveyDirection.Right)
            ConveyDir = Vector3.right;
        else if (EConveyDir == EConveyDirection.Up)
            ConveyDir = Vector3.up;
        else if (EConveyDir == EConveyDirection.Down)
            ConveyDir = Vector3.down;


        if (Is3D)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, ConveyDir, out hit, RayDistance))
                hit.transform.parent.GetComponent<Design_ConveySelector>().PowerON();
        }
        else
        {
            RaycastHit2D[] hit2D = Physics2D.RaycastAll(transform.position, ConveyDir, RayDistance);
            foreach (var Value in hit2D)
            {
                if (Value.transform.parent.gameObject != gameObject)
                {
                    Value.transform.parent.GetComponent<Design_ConveySelector>().PowerON();
                    break;
                }
            }
        }
    }

}
