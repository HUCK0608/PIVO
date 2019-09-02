using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectVisableController : MonoBehaviour
{
    [SerializeField]
    private CWorldObject _worldObject = null;

    private void Start()
    {
        CWorldManager.Instance.AddEffectVisableController(this);
    }

    public void Change2D()
    {
        if (!_worldObject.IsCanChange2D)
            gameObject.SetActive(false);
    }

    public void Change3D()
    {
        if (!_worldObject.IsCanChange2D)
            gameObject.SetActive(true);
    }
}
