using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_BombSpawn : MonoBehaviour
{
    public GameObject Bomb;

    GameObject CurBomb;
    void Start()
    {
        SpawnBomb();
    }

    public void SpawnBomb()
    {

        CurBomb = Instantiate(Bomb, transform);
        CurBomb.transform.Find("Root3D").GetComponent<Design_Bomb>().ParentBombSpawn = this;
        StartCoroutine("RiseBomb");

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

    }
}
