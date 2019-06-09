using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_Monster3D : MonoBehaviour
{
    public float MoveSpeed;

    bool bCheckPlayer;
    bool bWaitAnimation;
    bool bThrowCheck;
    float ZAxisValue;
    int PhaseNum;
    string CorgiState;

    GameObject Corgi, Monster2D;
    Animator MonsterAnimator;

    Vector3 MonsterPos, MonsterRot, PlayerPos, ThrowMonsterPos, LookTargetPos, ThrowCorgiPos;
    void Start()
    {
    }

    void Update()
    {
        if (bCheckPlayer)
            ControlCorgi();

        if (bWaitAnimation)
            OnMonster();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10 && !bCheckPlayer)
        {
            bCheckPlayer = true;
            Corgi = other.gameObject;
            PlayerPos = Corgi.transform.position;
            StartCoroutine("WaitAnimation");
            LookTargetPos = new Vector3(PlayerPos.x, transform.position.y, PlayerPos.z);
            CorgiState = "Stop";
            PhaseNum = 1;
        }
    }



    //EventFunction



    public void InitializeValue()
    {
        bCheckPlayer = false;
        bThrowCheck = false;
        PhaseNum = 0;
        MonsterPos = transform.position;
        MonsterRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        MonsterAnimator = GetComponentInChildren<Animator>();
        Monster2D = transform.parent.transform.Find("2D").gameObject;


        Vector3 ThrowPosXZ = transform.Find("ThrowPos").transform.position;
        ThrowCorgiPos = new Vector3(ThrowPosXZ.x, transform.position.y, ThrowPosXZ.z);

        if (transform.position.z < ThrowCorgiPos.z)
            ZAxisValue = -1;
        else
            ZAxisValue = 1;

        ThrowMonsterPos = ThrowCorgiPos + new Vector3(0, 0, ZAxisValue*1.5f);
    }









    void ControlCorgi()
    {
        if (CorgiState == "Stop")
        {
            Corgi.transform.position = PlayerPos;
        }
        else if (CorgiState == "RaiseUp")
        {
            Corgi.transform.position = transform.position + new Vector3(0f, 2f, -ZAxisValue*1.5f);
            Corgi.transform.rotation = Quaternion.Euler(ZAxisValue*90, 0, 0);
        }
        else if (CorgiState == "Throw")
        {
            Corgi.transform.position = ThrowCorgiPos;
            Corgi.transform.rotation = Quaternion.Euler(-ZAxisValue*90, 0, 0);
        }
    }

    void SetMonsterLook()
    {
        transform.LookAt(LookTargetPos);
    }


    void OnMonster()
    {
        //SetMonsterLook();
        if (PhaseNum == 1)
        {
            if (transform.position == PlayerPos)
            {
                MonsterAnimator.SetBool("IsRun", false);
                CorgiState = "RaiseUp";
                bWaitAnimation = false;
                StartCoroutine("WaitMoment");
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerPos, MoveSpeed * 0.1f);
                MonsterAnimator.SetBool("IsRun", true);
            }
        }
        else if (PhaseNum == 2)
        {
            if (transform.position == ThrowMonsterPos)
            {
                MonsterAnimator.SetBool("IsRun", false);
                if (!bThrowCheck)
                    StartCoroutine("WaitThrow");
                bThrowCheck = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, ThrowMonsterPos, MoveSpeed * 0.1f);
                MonsterAnimator.SetBool("IsRun", true);
            }
        }
        else if (PhaseNum == 3)
        {
            if (transform.position == MonsterPos)
            {
                MonsterAnimator.SetBool("IsRun", false);
                Corgi.transform.rotation = Quaternion.Euler(0, 0, 0);
                Corgi.transform.position += Vector3.up;
                bWaitAnimation = false;
                bCheckPlayer = false;
                bThrowCheck = false;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, MonsterPos, MoveSpeed * 0.1f);
                MonsterAnimator.SetBool("IsRun", true);
            }
        }
        
    }

    public void SetCollisionSize(Vector3 CollisionSize)
    {
        BoxCollider BoxCollision = GetComponent<BoxCollider>();
        BoxCollision.size = CollisionSize;
        BoxCollision.center = new Vector3(-CollisionSize.x/2 + 1, 1, 0);
    }




    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(1.5f);

        bWaitAnimation = true;
    }

    IEnumerator WaitMoment()
    {
        yield return new WaitForSeconds(1f);
        PhaseNum = 2;
        bWaitAnimation = true;
        LookTargetPos = new Vector3(ThrowMonsterPos.x, transform.position.y, ThrowMonsterPos.z);
    }

    IEnumerator WaitThrow()
    {
        yield return new WaitForSeconds(1f);
        CorgiState = "Throw";

        yield return new WaitForSeconds(0.5f);
        LookTargetPos = MonsterPos;
        PhaseNum = 3;
    }
}
