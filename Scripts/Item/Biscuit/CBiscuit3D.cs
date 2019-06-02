using UnityEngine;

public class CBiscuit3D : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
            transform.GetComponentInParent<CBiscuit>().DestroyBiscuit();
    }
}
