using UnityEngine;

public class CBiscuit2D : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player) && collision.tag.Equals("Player"))
            transform.GetComponentInParent<CBiscuit>().DestroyBiscuit();
    }
}
