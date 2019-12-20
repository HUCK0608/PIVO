using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Bomb2D : MonoBehaviour
{
    [HideInInspector]
    public Design_BombController Controller;

    GameObject Corgi;
    GameObject Bomb;

    public void BeginPlay()
    {

        Corgi = CPlayerManager.Instance.RootObject2D;
        Bomb = this.transform.parent.gameObject;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.Find("Root3D"))
        {
            if (other.transform.parent.GetComponent<Design_BrokenTile>())
            {
                if (!Controller.bUseBomb)
                    other.transform.parent.GetComponent<Design_BrokenTile>().DestroyBrokenTile();
            }
        }
    }




    /*---------------
    //Function
    ----------------*/
    public void AttachForDistance()
    {
        float MinDistance = 2.5f;

        if (Vector2.Distance(Corgi.transform.position, Controller.transform.position) < MinDistance)
        {
            Controller.AttachCorgi();
        }
    }

    public void DownBomb()
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
            {
                Bomb.GetComponent<Design_BombController>().DisableBomb();
                Debug.Log("2층에 뭐가 있어서 내려놓을 수 없음");
            }
            else
            {
                Controller.bAttachCorgi = false;
                Vector3 DownValue = new Vector3(ScaleX * 1.5f, 3, 0);
                StartCoroutine(UseGravity());

                Bomb.transform.position = Corgi.transform.position + DownValue;
                Bomb.transform.parent = null;
            }
        }
        else
        {
            Controller.bAttachCorgi = false;
            Vector3 DownValue = new Vector3(ScaleX * 1.5f, 1, 0);
            StartCoroutine(UseGravity());

            Bomb.transform.position = Corgi.transform.position + DownValue;
            Bomb.transform.parent = null;
        }
    }

    IEnumerator UseGravity()
    {
        if (!Controller.bAttachCorgi)
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

                Debug.Log(Vector2.Distance(Corgi.transform.position, transform.position));
                if (Vector2.Distance(Corgi.transform.position, transform.position) > 15)
                {
                    Controller.BeginExplosion();
                    break;                    
                }

            }
        }
    }

}
