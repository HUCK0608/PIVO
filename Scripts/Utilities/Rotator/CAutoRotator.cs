using UnityEngine;

public class CAutoRotator : MonoBehaviour
{
    [SerializeField]
    private bool _x = false, _y =false, _z = false;

    [SerializeField]
    private float _speedX = 0f, _speedY = 0f, _speedZ = 0f;

    private void Update()
    {
        if (_x)
            transform.Rotate(Vector3.right, _speedX * Time.deltaTime, Space.World);
        if (_y)
            transform.Rotate(Vector3.up, _speedY * Time.deltaTime, Space.World);
        if (_z)
            transform.Rotate(Vector3.forward, _speedZ * Time.deltaTime, Space.World);
    }
}
