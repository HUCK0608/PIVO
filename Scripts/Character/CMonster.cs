﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMonster : CWorldObject
{
    private GameObject _rootObject2D = null;
    /// <summary>최상위 2D 오브젝트</summary>
    public GameObject RootObject2D { get { return _rootObject2D; } }

    /// <summary>캡슐 이펙트</summary>
    [SerializeField]
    private GameObject _effectCapusle = null;

    protected override void Awake()
    {
        base.Awake();

        _rootObject2D = RootObject.transform.Find("Root2D").gameObject;

        Change3D();
    }

    public override void Change2D()
    {
        if (IsCanChange2D)
            StartCoroutine(Change2DLogic());
        else
            RootObject3D.SetActive(false);
    }

    private IEnumerator Change2DLogic()
    {
        _effectCapusle.SetActive(false);
        _effectCapusle.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        RootObject3D.SetActive(false);
        RootObject2D.transform.parent = RootObject.transform;
        RootObject2D.transform.eulerAngles = Vector3.zero;
        RootObject3D.transform.parent = RootObject2D.transform;
        RootObject2D.SetActive(true);
    }

    public override void Change3D()
    {
        RootObject2D.SetActive(false);
        RootObject3D.transform.parent = RootObject.transform;
        RootObject2D.transform.parent = RootObject3D.transform;
        RootObject3D.SetActive(true);

        IsCanChange2D = false;
    }

    public override void ShowOnBlock() { }
    public override void ShowOffBlock() { }
}
