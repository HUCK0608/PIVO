using UnityEngine;

public class CAutoChangeView2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player))
            CWorldManager.Instance.ChangeWorld();
    }
}
