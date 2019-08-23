using UnityEngine;

public class CDoorKey2D : MonoBehaviour
{
    private CDoorKey _key = null;

    private void Awake()
    {
        _key = GetComponentInParent<CDoorKey>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player))
            _key.GetKey();
    }
}
