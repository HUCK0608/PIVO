using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_CubeBro : MonoBehaviour
{
    public GameObject Corgi;

    GameObject Text3D;
    float DistanceMinimal;
    bool bUseCoroutine;
    void Start()
    {
        DistanceMinimal = 8f;

        Text3D = transform.Find("BillboardTEXT").Find("New Text").gameObject;
        Text3D.SetActive(false);
    }

    void Update()
    {
        if (!bUseCoroutine)
            CheckDistance();
    }

    void CheckDistance()
    {
        float Distance = Vector3.Distance(Corgi.transform.position, transform.position);

        if (Distance < DistanceMinimal)
        {
            StartCoroutine("ShowSubtitle");
        }

    }

    IEnumerator ShowSubtitle()
    {
        bUseCoroutine = true;
        Text3D.SetActive(true);

        yield return new WaitForSeconds(2f);

        Text3D.SetActive(false);

        yield return new WaitForSeconds(2f);

        bUseCoroutine = false;
    }
}
