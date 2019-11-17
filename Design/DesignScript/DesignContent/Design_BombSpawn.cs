using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombSpawn : Design_WorldObjectController
{
    public GameObject Bomb;

    GameObject CurBomb;

    public override void BeginPlay()
    {
        base.BeginPlay();

        SpawnBomb();
    }

    public void SpawnBomb()
    {
        if (WorldManager.CurrentWorldState != EWorldState.Changing)
        {
            CurBomb = Instantiate(Bomb, transform);
            CurBomb.GetComponent<Design_BombController>().ParentBombSpawn = this;
            CurBomb.GetComponent<Design_BombController>().bUseBomb = false;

            if (WorldManager.CurrentWorldState == EWorldState.View3D)
            {
                CurBomb.GetComponent<Design_BombController>().Change3D();
            }
            else
            {
                if (IsCanChange2D)
                {
                    CurBomb.GetComponent<Design_BombController>().IsCanChange2D = true;
                    CurBomb.GetComponent<Design_BombController>().Change2D();
                }
            }

            StartCoroutine(RiseBomb());
        }
        else
        {
            StartCoroutine(WaitShow());
        }
    }

    IEnumerator WaitShow()
    {
        yield return new WaitUntil(() => WorldManager.CurrentWorldState == EWorldState.View3D);
        SpawnBomb();
    }

    IEnumerator RiseBomb()
    {
        CurBomb.transform.position = transform.position;
        float TargetPositionY = transform.position.y + 2;

        while (CurBomb.transform.position.y < TargetPositionY)
        {
            CurBomb.transform.position = CurBomb.transform.position + new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        CurBomb.GetComponent<Design_BombController>().bUseBomb = true;
    }
}
