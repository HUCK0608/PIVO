using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Design_CubeBroController : Design_WorldObjectController
{

    public float DistanceMinimal;
    public float FlipValue;

    GameObject TextObject;
    GameObject ViewChangeEffect;

    Vector3 TextOriginPos;



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

        if (IsCanChange2D)
            SpawnEffect(true);

        StartCoroutine(SetText2DPosition());
    }

    public override void DesignChange3D()
    {
        base.DesignChange3D();

        SpawnEffect(false);

        StartCoroutine(SetText3DPosition());
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
        else if (WorldManager.CurrentWorldState == EWorldState.Changing)
            TextObject.SetActive(false);

    }

    IEnumerator SetText3DPosition()
    {
        float TargetValue = 1 / 0.3f;
        float SaveDeltaSeconds = 0f;

        Vector3 TargetPosition = TextObject.transform.position;

        while (true)
        {
            SaveDeltaSeconds += Time.deltaTime;
            if (SaveDeltaSeconds > 1)
            {
                Mathf.Clamp(SaveDeltaSeconds, 0, 1);
                TextObject.transform.position = Vector3.Lerp(TargetPosition, TextOriginPos, SaveDeltaSeconds * TargetValue);
                break;
            }

            TextObject.transform.position = Vector3.Lerp(TargetPosition, TextOriginPos, SaveDeltaSeconds * TargetValue);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;

    }

    IEnumerator SetText2DPosition()
    {
        float TargetValue = 1 / 0.3f;
        float SaveDeltaSeconds = 0f;
        float TargetPosX = TextObject.transform.Find("Dialogue_Bubble").Find("BubblePoint").position.x;

        if (TextObject.transform.Find("Dialogue_Bubble").GetComponent<SpriteRenderer>().flipX)
            TargetPosX = Mathf.Abs(TargetPosX - (transform.position.x + FlipValue));
        else
            TargetPosX = -Mathf.Abs(TargetPosX - (transform.position.x - FlipValue - 1));

        Vector3 TargetPosition = new Vector3(TextOriginPos.x + TargetPosX, TextOriginPos.y, TextOriginPos.z);

        while (true)
        {
            SaveDeltaSeconds += Time.deltaTime;
            if (SaveDeltaSeconds > 1)
            {
                Mathf.Clamp(SaveDeltaSeconds, 0, 1);
                TextObject.transform.position = Vector3.Lerp(TextOriginPos, TargetPosition, SaveDeltaSeconds * TargetValue);
                break;
            }

            TextObject.transform.position = Vector3.Lerp(TextOriginPos, TargetPosition, SaveDeltaSeconds * TargetValue);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return null;

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

