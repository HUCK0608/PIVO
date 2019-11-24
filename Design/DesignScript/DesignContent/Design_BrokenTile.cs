using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BrokenTile : Design_WorldObjectController
{
    public void DestroyBrokenTile()
    {
        CWorldManager.Instance.RemoveWorldObject(this);
        Destroy(gameObject);
    }
}
