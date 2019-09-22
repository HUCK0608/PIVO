using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb : MonoBehaviour
{
    public GameObject BoomEffect;
    [HideInInspector]
    public Design_BombSpawn ParentBombSpawn;
    GameObject Corgi;

    bool bAttachCorgi;
    float MinDistance;
    float ExplosionDistance;
    float AllowAngle;
    void Start()
    {

        bAttachCorgi = false;
        MinDistance = 2.5f;
        AllowAngle = 0.6f;
        ExplosionDistance = 15f;

        Corgi = GameObject.Find("PlayerGroup").transform.Find("Root3D").gameObject;

    }

    void Update()
    {
        if (bAttachCorgi)
            DownBomb();
        else
            AttachForDistance();

        Explosion();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Design_BrokenTile>())
            Destroy(other.gameObject);
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
        Vector3 BminusP = transform.position - Corgi.transform.position;
        Vector3 B2PNormal = BminusP.normalized;

        return B2PNormal;
    }

    void AttachForDistance()
    {
        if (Vector3.Distance(Corgi.transform.position, transform.position) < MinDistance)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
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
        transform.position = Corgi.transform.position + Vector3.up * 4f;

        transform.parent = Corgi.transform;
    }

    void DownBomb()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float BoxCastSizeF = 0.2f;
            Vector3 BoxCastSize = new Vector3(BoxCastSizeF, BoxCastSizeF, BoxCastSizeF);
            Vector3 BoxCastStartPoint = Corgi.transform.position + new Vector3(0, 0.5f, 0);
            float BoxCastDistance = 1.5f;

            RaycastHit hit;

            if (Physics.BoxCast(BoxCastStartPoint, BoxCastSize, Corgi.transform.forward, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
            {
                if (hit.transform.gameObject.layer == 9)
                {
                    Vector3 BoxCastStartPoint2 = Corgi.transform.position + new Vector3(0, 2.5f, 0);
                    if (Physics.BoxCast(BoxCastStartPoint2, BoxCastSize, Corgi.transform.forward, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
                        Debug.Log("위에 뭐가 있어서 내려놓을 수 없음");
                    else
                    {
                        bAttachCorgi = false;
                        Vector3 DownValue = new Vector3(0, 3, 0);

                        transform.position = Corgi.transform.position + GetCorgiForward() + DownValue;
                        transform.parent = null;
                        StartCoroutine("UseGravity");
                    }
                }

            }
            else
            {
                bAttachCorgi = false;
                Vector3 DownValue = new Vector3(0, 1, 0);

                transform.position = Corgi.transform.position + GetCorgiForward() + DownValue;
                transform.parent = null;
                StartCoroutine("UseGravity");
            }

        }
    }

    void Explosion()
    {
        if (Input.GetKeyDown(KeyCode.C) && Vector3.Distance(Corgi.transform.position, transform.position) < ExplosionDistance)
        {
            transform.parent = null;
            StartCoroutine("ExplosionBoom");
        }
    }

    IEnumerator ExplosionBoom()
    {
        Vector3 AddPosition = new Vector3(0, 0, -0.5f);
        GameObject BoomInstance = Instantiate(BoomEffect, transform.position + AddPosition, transform.rotation);
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().isTrigger = true;

        float BoomSize = 8;
        GetComponent<BoxCollider>().size = new Vector3(BoomSize, BoomSize, BoomSize);
        GetComponent<BoxCollider>().center = GetComponent<BoxCollider>().center + new Vector3(0, BoomSize/2, 0);
        yield return new WaitForSeconds(2f);

        ParentBombSpawn.SpawnBomb();
        Destroy(BoomInstance);
        Destroy(this.gameObject);
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

                if (Physics.BoxCast(transform.position, BoxCastSize, transform.up * -1, out hit, Quaternion.Euler(0, 0, 0), BoxCastDistance))
                    break;
                else
                    transform.position = transform.position + new Vector3(0, -0.2f, 0);

                yield return new WaitForSeconds(Time.deltaTime);

            }
        }
    }

}
