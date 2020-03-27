using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombSpawn : Design_WorldObjectController
{
    public GameObject Bomb;

    bool bFirstTime;
    GameObject CurBomb;
    GameObject LeftDoor, RightDoor;
    
    [HideInInspector]
    public List<GameObject> destroyObject = new List<GameObject>();

    public override void BeginPlay()
    {
        base.BeginPlay();

        bFirstTime = true;

        LeftDoor = RootObject3D.transform.Find("BombSpawnDoor_L").gameObject;
        RightDoor = RootObject3D.transform.Find("BombSpawnDoor_R").gameObject;

        SpawnBomb();
    }

    public void RefreshDestroyActor()
    {
        destroyObject.Clear();
        GameObject _destroyObejct = GameObject.Find("DestroyObject");
        Transform _DestroyObject = null;
        if (_destroyObejct != null)
            _DestroyObject = _destroyObejct.transform;

        if (_DestroyObject != null)
        {
            for (int i = 0; i < _DestroyObject.childCount; i++)
                destroyObject.Add(_DestroyObject.GetChild(i).gameObject);
        }
        else
            Debug.Log("파괴가능한 오브젝트의 그룹 이름을 맞춰주어야 합니다. <DestroyObject>");

    }

    public void SpawnBomb()
    {
        if (!bFirstTime)
        {
            Design_BombController Controller = CurBomb.GetComponent<Design_BombController>();
            Controller.RootObject2D.SetActive(false);
            Controller.RootObject3D.SetActive(false);
        }

        if (WorldManager.CurrentWorldState != EWorldState.Changing)
        {
            if (bFirstTime)
            {
                CurBomb = Instantiate(Bomb, transform.position, Quaternion.identity);
                CurBomb.transform.parent = transform;
                bFirstTime = false;
            }


            CurBomb.transform.parent = transform;
            CurBomb.GetComponent<Design_BombController>().bUseBomb = true;
            CurBomb.GetComponent<Design_BombController>().bAttach = false;
            CurBomb.GetComponent<Design_BombController>().ParentBombSpawn = this;


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
        float TargetPositionY = transform.position.y + 1.8f;
        float TargetLeftDoorX = transform.position.x - 1.2f;
        float TargetDefaultLeftDoorX = LeftDoor.transform.position.x;
        float TargetDefaultRightDoorX = RightDoor.transform.position.x;

        Design_BombController Controller = CurBomb.GetComponent<Design_BombController>();
        Controller.RootObject2D.SetActive(true);
        Controller.RootObject3D.SetActive(true);

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
            CurBomb.GetComponent<Design_BombController>().bAttach = true;
        }

        while (true)
        {
            float Modifier = 0.05f;
            LeftDoor.transform.Translate(Vector3.right * Modifier);
            RightDoor.transform.Translate(Vector3.right * Modifier);
            yield return new WaitForSeconds(Time.deltaTime);

            if (LeftDoor.transform.position.x > TargetDefaultLeftDoorX)
            {
                RightDoor.transform.position = new Vector3(TargetDefaultRightDoorX, RightDoor.transform.position.y, RightDoor.transform.position.z);
                LeftDoor.transform.position = new Vector3(TargetDefaultLeftDoorX, LeftDoor.transform.position.y, RightDoor.transform.position.z);
                break;
            }
        }
    }
}
