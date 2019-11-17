using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Design_CubeBroController : Design_WorldObjectController
{

    public float DistanceMinimal;

    GameObject TextObject;
    GameObject ViewChangeEffect;

    Vector3 TextOriginPos;

    float TargetValue;



    public override void BeginPlay()
    {
        base.BeginPlay();

        ViewChangeEffect = transform.Find("ViewChange_Effect").gameObject;
        ViewChangeEffect.SetActive(false);

        TextObject = transform.Find("BillboardTEXT").Find("TextObject").gameObject;
        TextObject.transform.forward = Camera.main.transform.forward;
        TextOriginPos = TextObject.transform.position;
        TextObject.SetActive(false);

        if (DistanceMinimal == 0)
            DistanceMinimal = 5;

        Change3D();
    }

    public override void DesignChange2D()
    {
        base.DesignChange2D();

        TargetValue = -1;
        if (IsCanChange2D)
            SpawnEffect(true);

        StartCoroutine(SetTextObjectPosition());
    }

    public override void DesignChange3D()
    {
        base.DesignChange3D();

        SpawnEffect(false);

        StartCoroutine(SetTextObjectPosition());
    }

    public override void WaitChangeWorld(EWorldState CurState)
    {
        base.WaitChangeWorld(CurState);

        if (CurState == EWorldState.View2D)
            SetCubeAnimation2D();
    }







    void Update()
    {
        TextObject.transform.forward = Camera.main.transform.forward;

        if (WorldManager.CurrentWorldState == EWorldState.View3D)
        {
            GameObject Corgi3D = CPlayerManager.Instance.RootObject3D;
            if (Vector3.Distance(transform.position, Corgi3D.transform.position) < DistanceMinimal && CPlayerManager.Instance.gameObject.activeSelf)
                TextObject.SetActive(true);
            else
                TextObject.SetActive(false);
        }
        else if (WorldManager.CurrentWorldState == EWorldState.View2D)
        {
            GameObject Corgi2D = CPlayerManager.Instance.RootObject2D;
            if (Vector2.Distance(transform.position, Corgi2D.transform.position) < DistanceMinimal)
            {
                if (IsCanChange2D && CPlayerManager.Instance.gameObject.activeSelf)
                    TextObject.SetActive(true);
                else
                    TextObject.SetActive(false);
            }
            else
                TextObject.SetActive(false);
        }

    }

    IEnumerator SetTextObjectPosition()
    {
        float Modify = 2.9f;

        if (TargetValue >= 0)
        {
            while (true)
            {
                if (TextObject.transform.position.x < TextOriginPos.x + TargetValue)
                    TextObject.transform.position += new Vector3(Time.deltaTime * Modify, 0, 0);
                else
                    break;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else
        {
            while (true)
            {
                if (TextObject.transform.position.x > TextOriginPos.x + TargetValue)
                    TextObject.transform.position -= new Vector3(Time.deltaTime * Modify, 0, 0);
                else
                    break;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }       
    }

    void SetCubeAnimation2D()
    {
        Animator Anim = RootObject2D.GetComponent<Animator>();
        ECubeColor CurColor = GetComponent<Design_CubeMaterialChange>().CubeColor;

        if (CurColor == ECubeColor.Mint)
            Anim.SetInteger("AnimSelect", 1);
        else if (CurColor == ECubeColor.Red)
            Anim.SetInteger("AnimSelect", 2);
        else if (CurColor == ECubeColor.Purple)
            Anim.SetInteger("AnimSelect", 3);
    }

    void SpawnEffect(bool bState)
    {
        ViewChangeEffect.SetActive(bState);
    }

}

