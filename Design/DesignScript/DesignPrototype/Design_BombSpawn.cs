using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombSpawn : Design_WorldController
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
        if (bShow && WorldManager.CurrentWorldState != EWorldState.Changing)
        {
            CurBomb = Instantiate(Bomb, transform);
            CurBomb.transform.parent = null;
            CurBomb.GetComponent<Design_BombController>().ParentBombSpawn = this;
            CurBomb.GetComponent<Design_BombController>().bUseBomb = false;

            if (bState3D)
            {
                CurBomb.GetComponent<Design_BombController>().SetWorldStateCustom(EWorldState.View3D);
                CurBomb.GetComponent<Design_BombController>().CurrentState = EWorldState.View3D;
            }
            else
            {
                CurBomb.GetComponent<Design_BombController>().SetWorldStateCustom(EWorldState.View2D);
                CurBomb.GetComponent<Design_BombController>().CurrentState = EWorldState.View2D;
            }

            StartCoroutine("RiseBomb");
        }
        else
            StartCoroutine("WaitShow");
    }

    IEnumerator WaitShow()
    {
        yield return new WaitUntil(() => bShow);
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
