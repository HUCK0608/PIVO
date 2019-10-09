using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_CubeBroController : Design_WorldController
{
    public Material[] CubeBroTexture;
    public float DistanceMinimal;

    GameObject TextObject;

    EWorldState CurWorldState;
    Vector3 TextOriginPos;

    float TargetValue;


    public override void BeginPlay()
    {
        base.BeginPlay();

        CurWorldState = EWorldState.View3D;

        TextObject = transform.Find("BillboardTEXT").Find("TextObject").gameObject;
        TextObject.transform.forward = Camera.main.transform.forward;
        TextOriginPos = TextObject.transform.position;
        TextObject.SetActive(false);

        if (DistanceMinimal == 0)
            DistanceMinimal = 5;

    }

    public override void ChangeWorld(EWorldState CurState)
    {
        base.ChangeWorld(CurState);
        CurWorldState = CurState;

        if (CurWorldState == EWorldState.View2D)
            TargetValue = -1;
        else
            TargetValue = 0;

        StartCoroutine("SetTextObjectPosition");
    }


    public override void OnTick()
    {
        base.OnTick();

        TextObject.transform.forward = Camera.main.transform.forward;

        if (CurWorldState == EWorldState.View3D)
        {
            GameObject Corgi3D = CPlayerManager.Instance.RootObject3D;
            if (Vector3.Distance(transform.position, Corgi3D.transform.position) < DistanceMinimal)
                TextObject.SetActive(true);
            else
                TextObject.SetActive(false);
        }
        else if (CurWorldState == EWorldState.View2D)
        {
            GameObject Corgi2D = CPlayerManager.Instance.RootObject2D;
            if (Vector2.Distance(transform.position, Corgi2D.transform.position) < DistanceMinimal)
                TextObject.SetActive(true);
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
}

