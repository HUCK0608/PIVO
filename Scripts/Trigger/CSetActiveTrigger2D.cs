using UnityEngine;

public class CSetActiveTrigger2D : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;

    [SerializeField]
    private bool _value = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player))
            _target.SetActive(_value);
    }
}
