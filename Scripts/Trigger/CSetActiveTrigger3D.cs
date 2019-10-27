using System.Collections;
using UnityEngine;

public class CSetActiveTrigger3D : MonoBehaviour
{
    [SerializeField]
    private GameObject _target = null;

    [SerializeField]
    private bool _value = true;

    [SerializeField]
    private float _activateDelay = 0f;
    private bool _isActivate = true;

    private void Awake()
    {
        if (!_activateDelay.Equals(0f))
            StartCoroutine(DelayLogic());
    }

    private IEnumerator DelayLogic()
    {
        _isActivate = false;

        yield return new WaitForSeconds(_activateDelay);

        _isActivate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActivate && other.gameObject.layer.Equals(CLayer.Player))
            _target.SetActive(_value);
    }
}
