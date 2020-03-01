using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BrokenTile : Design_WorldObjectController
{
    [SerializeField]
    private SoundRandomPlayer_SFX _brokenSoundRandomPlayer = null;

    public void DestroyBrokenTile()
    {
        CWorldManager.Instance.RemoveWorldObject(this);
        _brokenSoundRandomPlayer.Play();
        Destroy(gameObject);
    }
}
