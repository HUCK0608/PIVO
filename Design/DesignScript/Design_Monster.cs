using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Design_Monster : MonoBehaviour
{
    public float MoveSpeed;

    bool bCheckPlayer;
    bool bWaitAnimation;
    bool bThrowCheck;
    int PhaseNum;
    string CorgiState;

    Text ProtoDesc;
    GameObject Corgi, Monster2D;
    Animator MonsterAnimator;

    Vector3 MonsterPos, MonsterRot, PlayerPos, ThrowPos, LookTargetPos;
    void Start()
    {
        InitializeValue();
        SetMonster2D(false);
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



    void InitializeValue()
    {
        bCheckPlayer = false;
        bThrowCheck = false;
        PhaseNum = 0;
        MonsterPos = transform.position;
        MonsterRot = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
        MonsterAnimator = GetComponentInChildren<Animator>();
        ProtoDesc = GameObject.Find("ProtoDesc").GetComponent<Text>();
        Monster2D = transform.parent.transform.Find("2D").gameObject;

        Vector3 ThrowPosXZ = transform.Find("ThrowPos").transform.position;
        ThrowPos = new Vector3(ThrowPosXZ.x, transform.position.y, ThrowPosXZ.z);
    }

    void SetProtoDesc(string DescContent)
    {
        ProtoDesc.text = DescContent;
    }

    void SetMonster2D(bool Active)
    {
        Monster2D.SetActive(Active);
    }








    void ControlCorgi()
    {
        if (CorgiState == "Stop")
        {
            Corgi.transform.position = PlayerPos;
        }
        else if (CorgiState == "RaiseUp")
        {
            Corgi.transform.position = transform.position + new Vector3(0f, 2f, 1.5f);
            Corgi.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
        else if (CorgiState == "Throw")
        {
            Corgi.transform.position = ThrowPos + new Vector3(0, 0, 1.5f);
            Corgi.transform.rotation = Quaternion.Euler(90, 0, 0);
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
            if (transform.position == ThrowPos)
            {
                MonsterAnimator.SetBool("IsRun", false);
                if (!bThrowCheck)
                    StartCoroutine("WaitThrow");
                bThrowCheck = true;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, ThrowPos, MoveSpeed * 0.1f);
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
                SetProtoDesc("끝");
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, MonsterPos, MoveSpeed * 0.1f);
                MonsterAnimator.SetBool("IsRun", true);
            }
        }
        
    }




    IEnumerator WaitAnimation()
    {
        SetProtoDesc("숲숲이의 놀라는 애니메이션을 기다립니다.");
        yield return new WaitForSeconds(1.5f);

        SetProtoDesc("코기는 못움직이며, 숲숲이가 코기에게 달려갑니다.");
        bWaitAnimation = true;
    }

    IEnumerator WaitMoment()
    {
        yield return new WaitForSeconds(1f);
        SetProtoDesc("코기를 들고 지정해놓은 던질 위치로 데리고 갑니다.");
        PhaseNum = 2;
        bWaitAnimation = true;
        LookTargetPos = new Vector3(ThrowPos.x, transform.position.y, ThrowPos.z);
    }

    IEnumerator WaitThrow()
    {
        yield return new WaitForSeconds(1f);
        CorgiState = "Throw";
        SetProtoDesc("코기를 내동댕이 칩니다.");

        yield return new WaitForSeconds(0.5f);
        LookTargetPos = MonsterPos;
        PhaseNum = 3;
        SetProtoDesc("다시 원래 자리로 돌아갑니다.");
    }
}
