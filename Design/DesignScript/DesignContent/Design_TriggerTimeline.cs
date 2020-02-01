using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_TriggerTimeline : CWorldObject
{
    public override void Change2D()
    {
        transform.Find("StageClear").GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public override void Change3D()
    {
        transform.Find("StageClear").GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    public override void ShowOffBlock()
    {
    }

    public override void ShowOnBlock()
    {
    }
}
