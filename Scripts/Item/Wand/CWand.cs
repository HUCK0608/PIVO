using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWand : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player))
        {
            CWorldManager.Instance.ChangeWorld();
            CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Idle);
            gameObject.SetActive(false);
        }
    }
}
