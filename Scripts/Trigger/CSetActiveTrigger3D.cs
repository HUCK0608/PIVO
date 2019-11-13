using System.Collections;
using UnityEngine;

public class CSetActiveTrigger3D : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;

    [SerializeField]
    private bool _value = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
            _target.SetActive(_value);
    }
}
