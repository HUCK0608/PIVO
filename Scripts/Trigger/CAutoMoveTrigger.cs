using UnityEngine;

public class CAutoMoveTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            CPlayerManager.Instance.StartAutoMove(transform.position);
        }
    }
}
