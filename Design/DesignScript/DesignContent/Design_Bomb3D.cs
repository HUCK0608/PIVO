using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb3D : MonoBehaviour
{
    [HideInInspector]
    public Design_BombController Controller;

    GameObject Bomb;
    GameObject Corgi;
    

    bool bAttachCorgi;
    float MinDistance;
    float ExplosionDistance;
    float AllowAngle;
    public void BeginPlay()
    {

        bAttachCorgi = false;
        MinDistance = 2.5f;
        AllowAngle = 0.6f;
        ExplosionDistance = 15f;

        Corgi = CPlayerManager.Instance.RootObject3D;
        Bomb = this.transform.parent.gameObject;
    }

    void Update()
    {
        if (bAttachCorgi)
            DownBomb();
        else
            AttachForDistance();

        //Explosion();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.GetComponent<Design_BrokenTile>())
            Destroy(other.transform.parent.gameObject);
    }





    /*------------------
        Function
    ------------------*/

    Vector3 GetCorgiForward()
    {
        return Corgi.transform.forward;
    }

    Vector3 GetBomb2PlayerForward()
    {
        Vector3 BminusP = Bomb.transform.position - Corgi.transform.position;
        Vector3 B2PNormal = BminusP.normalized;

        return B2PNormal;
    }

    void AttachForDistance()
    {
        if (Vector3.Distance(Corgi.transform.position, Bomb.transform.position) < MinDistance)
        {
            if (Input.GetKeyDown(Controller.InteractionKey))
            {
                transform.parent.parent = null;
                Vector3 CompareForward = GetCorgiForward() - GetBomb2PlayerForward();
                float CompareAngle = Mathf.Abs(CompareForward.x + CompareForward.z);

                if (AllowAngle > CompareAngle)
                    AttachCorgi();
            }
        }
    }

    void AttachCorgi()
    {
        bAttachCorgi = true;
        Bomb.GetComponent<Design_BombController>().DisableBomb();
        Bomb.transform.position = Corgi.transform.position + Vector3.up * 4f;

        Bomb.transform.parent = Corgi.transform;
    }

    void DownBomb()
    {
        if (Input.GetKeyDown(Controller.InteractionKey))
        {
            Bomb.GetComponent<Design_BombController>().EnableBomb();

            float BoxCastSizeF = 0.2f;
            Vector3 BoxCastSize = new Vector3(BoxCastSizeF, BoxCastSizeF, BoxCastSizeF);
            Vector3 BoxCastStartPoint = Corgi.transform.position + new Vector3(0, 0.5f, 0);
            float BoxCastDistance = 1.2f;

            RaycastHit hit;

            if (Physics.BoxCast(BoxCastStartPoint, BoxCastSize, Corgi.transform.forward, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
            {
                if (hit.transform.gameObject.layer == 9)
                {
                    Vector3 BoxCastStartPoint2 = Corgi.transform.position + new Vector3(0, 2.5f, 0);
                    if (Physics.BoxCast(BoxCastStartPoint2, BoxCastSize, Corgi.transform.forward, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
                    {
                        Bomb.GetComponent<Design_BombController>().DisableBomb();
                        Debug.Log("2층에 뭐가 있어서 내려놓을 수 없음");
                    }
                    else
                    {
                        bAttachCorgi = false;
                        Vector3 DownValue = new Vector3(0, 3, 0);

                        Bomb.transform.position = Corgi.transform.position + GetCorgiForward() + DownValue;
                        Bomb.transform.parent = null;
                        StartCoroutine("UseGravity");
                    }
                }

            }
            else
            {
                bAttachCorgi = false;
                Vector3 DownValue = new Vector3(0, 1, 0);

                Bomb.transform.position = Corgi.transform.position + GetCorgiForward() + DownValue;
                Bomb.transform.parent = null;
                StartCoroutine("UseGravity");
            }

            if (!bAttachCorgi)
            {
                RaycastHit hit2;
                if (Physics.Raycast(transform.position, Vector3.down, out hit2, BoxCastDistance))
                {
                    transform.parent.parent = hit2.transform;
                }
            }

        }
    }

    void Explosion()
    {
        if (Input.GetKeyDown(Controller.ExplosionKey) && Vector3.Distance(Corgi.transform.position, Bomb.transform.position) < ExplosionDistance)
        {
            Bomb.transform.parent = null;
            StartCoroutine("ExplosionBoom");
        }
    }

    IEnumerator ExplosionBoom()
    {
        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomInstance = Instantiate(Controller.BoomEffect, transform.position + AddPosition, transform.rotation);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;

        float BoomSize = 8;
        GetComponent<BoxCollider>().size = new Vector3(BoomSize, BoomSize, BoomSize);
        GetComponent<BoxCollider>().center = GetComponent<BoxCollider>().center + new Vector3(0, BoomSize/2, 0);
        yield return new WaitForSeconds(2f);

        Controller.ParentBombSpawn.SpawnBomb();
        Destroy(BoomInstance);
        Destroy(Bomb);
    }

    IEnumerator UseGravity()
    {
        if (!bAttachCorgi)
        {
            while (true)
            {

                float BoxCastSizeF = 0.2f;
                Vector3 BoxCastSize = new Vector3(BoxCastSizeF, BoxCastSizeF, BoxCastSizeF);
                RaycastHit hit;
                float BoxCastDistance = 1f;

                if (Physics.BoxCast(Bomb.transform.position, BoxCastSize, transform.up * -1, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
                    break;
                else
                    Bomb.transform.position = Bomb.transform.position + new Vector3(0, -0.2f, 0);

                yield return new WaitForSeconds(Time.deltaTime);

            }
        }
    }

}
