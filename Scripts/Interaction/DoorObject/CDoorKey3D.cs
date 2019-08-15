using UnityEngine;

public class CDoorKey3D : MonoBehaviour
{
    private CDoorKey _key = null;

    private void Awake()
    {
        _key = GetComponentInParent<CDoorKey>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
            _key.GetKey();
    }
}
