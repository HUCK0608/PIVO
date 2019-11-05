using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_ConveySelector : Design_Convey
{
    private bool IsON;

    public void PowerON()
    {
        IsON = true;
    }

    public void PowerOFF()
    {
        IsON = false;
    }
    
}
