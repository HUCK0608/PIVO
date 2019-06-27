﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Design_Monster2D : MonoBehaviour
{
    [HideInInspector]
    public float MoveSpeed;

    GameObject Corgi, Monster3D;
    RaycastHit2D CorgiHit;
    Vector3 PlayerPos, MonsterPos, MoveToPlayerPos, ThrowMonsterPos, OriginPos, LookVector;
    string CorgiState;
    float LookValue, RayLength, ThrowCorgiPosX;
    bool bUseAction;


    //InitializeValue



    void Start()
    {
    }

    void Update()
    {
        if (CheckZAxis())
            CheckRaycast();

        if (bUseAction)
            ControlCorgi();
    }

    void CheckRaycast()
    {
        CorgiHit = Physics2D.Raycast(OriginPos, LookVector, RayLength);

        if (CorgiHit)
        {
            if (CorgiHit.collider.gameObject.layer == 10 && !bUseAction)
            {
                Corgi = CorgiHit.collider.gameObject;
                PlayerPos = Corgi.transform.position;
                MoveToPlayerPos = new Vector3(PlayerPos.x, PlayerPos.y, transform.position.z);
                StartCoroutine("MonsterAction00");
                bUseAction = true;
                //CorgiState = "Stop";
                Debug.Log("CollisionCheck");
            }
        }
    }


    //EventFunction







    public void InitializeValue()
    {
        LookValue = transform.parent.forward.z;
        RayLength = transform.parent.Find("3D").GetComponent<BoxCollider>().size.x - 3f;
        MonsterPos = transform.position;

        ThrowCorgiPosX = transform.Find("ThrowPos").transform.position.x;
        ThrowMonsterPos = new Vector3(ThrowCorgiPosX + LookValue*1.5f, transform.position.y, transform.position.z);
        gameObject.SetActive(false);

        LookVector = new Vector2(-LookValue * 1.5f, 0);
        OriginPos = transform.position + Vector3.up + (Vector3)LookVector;
    }

    bool CheckZAxis()
    {
        RaycastHit hit;
        Vector3 RayOrigin = transform.position + Vector3.up;
        bool ReturnValue = true;

        if (Physics.Raycast(RayOrigin, Vector3.forward, out hit, 100))
        {
            if (hit.collider.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                ReturnValue = false;
            }
        }

        if (Physics.Raycast(RayOrigin, Vector3.back, out hit, 100))
        {
            if (hit.collider.gameObject.GetComponent<MeshRenderer>().enabled)
            {
                ReturnValue = false;
            }
        }
        
        return ReturnValue;
    }

    void ControlCorgi()
    {
        if (CorgiState == "Stop")
        {
            Corgi.transform.position = PlayerPos;
        }
        else if (CorgiState == "RaiseUp")
        {
            Vector3 Pos = transform.position;
            Vector3 CorgiPos = Corgi.transform.position;
            Corgi.transform.position = new Vector3(Pos.x, Pos.y, CorgiPos.z) + new Vector3(LookValue*1.2f, 3f, 0f);
            Corgi.transform.rotation = Quaternion.Euler(0, 0, LookValue*90);
        }
        else if (CorgiState == "Throw")
        {
            Corgi.transform.position = new Vector3(ThrowCorgiPosX, Corgi.transform.position.y, Corgi.transform.position.z);
            Corgi.transform.rotation = Quaternion.Euler(0, 0, LookValue*90);
        }
    }

    void FinishMonsterAction()
    {
        bUseAction = false;

        CorgiState = "None";
        Corgi.transform.rotation = Quaternion.Euler(0, 0, 0);
        Corgi.transform.position += Vector3.up;
    }


    IEnumerator MonsterAction00()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            PlayerPos = Corgi.transform.position;
            MoveToPlayerPos = new Vector3(PlayerPos.x, PlayerPos.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, MoveToPlayerPos, MoveSpeed * 0.1f);

            if (transform.position == MoveToPlayerPos)
            {
                StartCoroutine("MonsterAction01");
                break;
            }

            if (!CorgiHit)
            {
                StartCoroutine("MonsterAction02");
                break;
            }

            yield return null;
        }
    }

    IEnumerator MonsterAction01()
    {
        CorgiState = "RaiseUp";
        yield return new WaitForSeconds(1f);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, ThrowMonsterPos, MoveSpeed * 0.1f);

            if (transform.position == ThrowMonsterPos)
            {
                StartCoroutine("MonsterAction02");
                CorgiState = "Throw";
                break;
            }
            yield return null;
        }
    }

    IEnumerator MonsterAction02()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, MonsterPos, MoveSpeed * 0.1f);

            if (transform.position == MonsterPos)
            {
                FinishMonsterAction();
                break;
            }
            yield return null;
        }
    }
}
