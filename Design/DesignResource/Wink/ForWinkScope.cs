using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForWinkScope : MonoBehaviour
{
    public Transform baseBlack;

    private Transform Base;
    private Transform Point_TopCenter;
    private Transform Point_BottomCenter;
    private Transform Point_RightCenter;
    private Transform Point_LeftCenter;

    private Transform Border_TopCenter;
    private Transform Border_BottomCenter;
    private Transform Border_RightCenter;
    private Transform Border_LeftCenter;

    public float ScopeSpeed = 1f;
    public float StopScopeSize = 0.06f;
    public float StopScopeTime = 2f;
    public float LastScopeSpeed = 2f;

    private bool bStopRunScope = false;

    void Start()
    {
        Base = transform.Find("Base");
        Point_TopCenter = Base.transform.Find("Point_TopCenter");
        Point_BottomCenter = Base.transform.Find("Point_BottomCenter");
        Point_RightCenter = Base.transform.Find("Point_RightCenter");
        Point_LeftCenter = Base.transform.Find("Point_LeftCenter");

        Border_TopCenter = transform.Find("Border_TopCenter");
        Border_BottomCenter = transform.Find("Border_BottomCenter");
        Border_RightCenter = transform.Find("Border_RightCenter");
        Border_LeftCenter = transform.Find("Border_LeftCenter");

        StartCoroutine(RunScope());
    }

    IEnumerator RunScope()
    {
        var LerpValue = 0f;
        var CurScopeSpeed = ScopeSpeed;
        while (LerpValue <= 1)
        {

            if (Base.localScale.x <= StopScopeSize && !bStopRunScope)
            {
                yield return new WaitForSeconds(StopScopeTime);
                bStopRunScope = true;
                CurScopeSpeed = LastScopeSpeed;
            }

            LerpValue += Time.deltaTime * CurScopeSpeed;
            Base.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, Mathf.Clamp(LerpValue, 0, 1));

            AttachActor(Border_BottomCenter, Point_TopCenter);
            AttachActor(Border_TopCenter, Point_BottomCenter);
            AttachActor(Border_LeftCenter, Point_RightCenter);
            AttachActor(Border_RightCenter, Point_LeftCenter);

            yield return new WaitForFixedUpdate();
        }

        baseBlack.gameObject.SetActive(true);
    }

    IEnumerator StopTimer()
    {
        yield return new WaitForSeconds(StopScopeTime);
    }

    void AttachActor(Transform Child, Transform Parent)
    {
        Child.position = Parent.position;
    }
}
