using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombSpawn : Design_WorldObjectController
{
    public GameObject Bomb;

    bool bFirstTime;
    GameObject CurBomb;
    GameObject LeftDoor, RightDoor;

    public override void BeginPlay()
    {
        base.BeginPlay();

        bFirstTime = true;

        LeftDoor = RootObject3D.transform.Find("BombSpawnDoor_L").gameObject;
        RightDoor = RootObject3D.transform.Find("BombSpawnDoor_R").gameObject;

        SpawnBomb();
    }

    public void SpawnBomb()
    {
        if (WorldManager.CurrentWorldState != EWorldState.Changing)
        {
            if (bFirstTime)
            {
                CurBomb = Instantiate(Bomb, transform);
                bFirstTime = false;
            }

            CurBomb.transform.parent = transform;
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
        float TargetPositionY = transform.position.y + 2f;
        float TargetLeftDoorX = transform.position.x - 1.2f;
        float TargetDefaultLeftDoorX = LeftDoor.transform.position.x;

        while (true)
        {
            float Modifier = 0.05f;
            LeftDoor.transform.Translate(Vector3.left * Modifier);
            RightDoor.transform.Translate(Vector3.left * Modifier);
            yield return new WaitForSeconds(Time.deltaTime);

            if (LeftDoor.transform.position.x < TargetLeftDoorX)
                break;
        }

        while (CurBomb.transform.position.y < TargetPositionY)
        {
            CurBomb.transform.position = CurBomb.transform.position + new Vector3(0, 0.1f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
            CurBomb.GetComponent<Design_BombController>().IsCanChange2D = IsCanChange2D;
        }

        CurBomb.GetComponent<Design_BombController>().bUseBomb = true;

        while (true)
        {
            float Modifier = 0.05f;
            LeftDoor.transform.Translate(Vector3.right * Modifier);
            RightDoor.transform.Translate(Vector3.right * Modifier);
            yield return new WaitForSeconds(Time.deltaTime);

            if (LeftDoor.transform.position.x > TargetDefaultLeftDoorX)
                break;
        }
    }
}
