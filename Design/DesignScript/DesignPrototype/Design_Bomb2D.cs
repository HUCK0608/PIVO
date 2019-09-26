﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb2D : MonoBehaviour
{
    [HideInInspector]
    public Design_BombController Controller;

    GameObject Corgi;
    GameObject Bomb;

    bool bAttachCorgi;
    float MinDistance;
    float ExplosionDistance;
    public void BeginPlay()
    {

        bAttachCorgi = false;
        MinDistance = 2.5f;
        ExplosionDistance = 15f;

        Corgi = CPlayerManager.Instance.RootObject2D;
        Bomb = this.transform.parent.gameObject;

    }

    void Update()
    {

        if (bAttachCorgi)
            DownBomb();
        else
            AttachForDistance();

        Explosion();

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.Find("Root3D"))
        {
            if (other.transform.parent.GetComponent<Design_BrokenTile>())
                Destroy(other.transform.parent.gameObject);
        }
    }




    /*---------------
    //Function
    ----------------*/

    void AttachForDistance()
    {
        if (Vector2.Distance(Corgi.transform.position, Bomb.transform.position) < MinDistance)
        {
            if (Input.GetKeyDown(Controller.InteractionKey))
            {
                AttachCorgi();
            }
        }
    }

    void AttachCorgi()
    {
        bAttachCorgi = true;
        Vector3 TargetPos = new Vector3(Corgi.transform.position.x, Corgi.transform.position.y, Bomb.transform.position.z);
        Bomb.transform.position = TargetPos + Vector3.up * 4f;

        Bomb.transform.parent = Corgi.transform;
    }
    void DownBomb()
    {
        
        if (Input.GetKeyDown(Controller.InteractionKey))
        {
            float BoxSizeF = 1.2f;
            float ScaleX = Corgi.transform.localScale.x;
            Vector2 CastPos = (Vector2)Corgi.transform.position + new Vector2(ScaleX * 1.5f, 1f);
            Vector2 CastSize = new Vector2(BoxSizeF, BoxSizeF);
            Vector2 CastDir = new Vector2(ScaleX, 0);
            float CastDistance = 0.3f;

            RaycastHit2D hit = Physics2D.BoxCast(CastPos, CastSize, 0, CastDir, CastDistance);

            Debug.DrawRay(CastPos, CastDir * CastDistance, Color.red, 0.1f);
            if (hit)
            {
                Debug.Log(hit.transform.parent.name);
                Vector2 CastPos2 = CastPos + new Vector2(0, 1);
                RaycastHit2D hit2 = Physics2D.BoxCast(CastPos2, CastSize, 0, CastDir, CastDistance);

                if (hit2)
                    Debug.Log("2층에 뭐가 있어서 내려놓을 수 없음");
                else
                {
                    bAttachCorgi = false;
                    Vector3 DownValue = new Vector3(ScaleX * 1.5f, 3, 0);

                    Bomb.transform.position = Corgi.transform.position + DownValue;
                    Bomb.transform.parent = null;
                }
            }
            else
            {
                bAttachCorgi = false;
                Vector3 DownValue = new Vector3(ScaleX * 1.5f, 1, 0);

                Bomb.transform.position = Corgi.transform.position + DownValue;
                Bomb.transform.parent = null;
            }
        }
    }

    void Explosion()
    {
        if (Input.GetKeyDown(Controller.ExplosionKey) && Vector2.Distance(Corgi.transform.position, Bomb.transform.position) < ExplosionDistance)
        {
            Bomb.transform.parent = null;
            StartCoroutine("ExplosionBoom");
        }
    }

    IEnumerator ExplosionBoom()
    {
        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomInstance = Instantiate(Controller.BoomEffect, transform.position + AddPosition, transform.rotation);        
        Controller.Actor3D.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = true;

        float BoomSize = 8;
        GetComponent<BoxCollider2D>().size = new Vector3(BoomSize, BoomSize, BoomSize);
        GetComponent<BoxCollider2D>().offset = GetComponent<BoxCollider2D>().offset + new Vector2(0, BoomSize / 2);
        yield return new WaitForSeconds(2f);

        Controller.ParentBombSpawn.SpawnBomb();

        Destroy(BoomInstance);
        Destroy(Bomb);
    }
}
