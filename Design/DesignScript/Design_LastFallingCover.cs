using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_LastFallingCover : MonoBehaviour
{
    [SerializeField]
    private GameObject Tile = null;

    private void Update()
    {
        var xValue = 30f;
        var yValue = 3f;

        var PlayerPosition = CPlayerManager.Instance.RootObject3D.transform.position;

        if (Mathf.Abs(transform.position.x - PlayerPosition.x) < xValue)
        {
            if (Mathf.Abs(transform.position.y - PlayerPosition.y) < yValue)
            {
                CPlayerManager.Instance.Stat.Hp -= 1;
                CPlayerManager.Instance.RootObject3D.transform.position = Tile.transform.position + (Vector3.up * 2f);
            }
        }
    }
}
