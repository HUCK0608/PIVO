using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWallPainting3D : MonoBehaviour
{
    [SerializeField]
    private CWallPainting _painting = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
            _painting.PlayerOnTrigger();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
            _painting.PlayerOnTriggerExit();
    }
}
