using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_SequenceObject : MonoBehaviour
{
    List<GameObject> CubeBroArray = new List<GameObject>();
    List<Vector3> CubePos = new List<Vector3>();

    float WaitSeconds;
    int LoopNum;
    bool OneTime;
    float MoveSpeed;
    float TimeValue;
    void Start()
    {
        LoopNum = 1;
        OneTime = false;
        MoveSpeed = 0.2f;
        TimeValue = 1f;

        StartCoroutine(FirstTimeSet());
        for (int i = 0; i < 13; i++)
        {
            CubeBroArray.Add(transform.Find("CubeBro_Red (" + i + ")").gameObject);
            SetRandomTexture(i);
        }

        for (int i = 12; i > -1; i--)
        {
            CubePos.Add(CubeBroArray[i].transform.position);
        }
    }

    void FixedUpdate()
    {
        if (WaitSeconds > 2.5f)
        {
            if (!OneTime)
            {
                OneTime = true;
                StartCoroutine(Waiting());

                for (int i = 0; i < 13; i++)
                {
                    SetAnimation(i, "Run");
                }
            }
        }
        else
        {
            WaitSeconds += Time.deltaTime * TimeValue;
            for (int i = 0; i < 13; i++)
            {
                int j = i + LoopNum;

                while (j > 12)
                {
                    j -= 13;
                }


                if (j == 0)
                {
                    CubeBroArray[i].transform.position = CubePos[j];
                    SetRandomTexture(i);
                }
                else
                    CubeBroArray[i].transform.position = Vector3.MoveTowards(CubeBroArray[i].transform.position, CubePos[j], MoveSpeed);
            }
        }
    }

    IEnumerator Waiting()
    {
        //yield return new WaitForSeconds(0.24f);
        float targetvalue = 0.24f;
        float curvalue = 0;
        while (true)
        {
            if (curvalue > targetvalue)
                break;

            yield return new WaitForFixedUpdate();
            curvalue += Time.deltaTime;
        }

        LoopNum++;
        WaitSeconds = 0;
        OneTime = false;
    }
     
    void SetRandomTexture(int CubeNum)
    {
        int RandomValue = Random.Range(0, 3);
        CubeBroArray[CubeNum].GetComponent<Design_CubeBro>().ChangeTexture(RandomValue);
    }
     
    void SetAnimation(int CubeNum, string State)
    {
        CubeBroArray[CubeNum].GetComponent<Design_CubeBro>().SetAnimState(State);
    }

    IEnumerator FirstTimeSet()
    {
        TimeValue = 4f;
        MoveSpeed = 10f;
        //yield return new WaitForSeconds(1f);
        float targetvalue = 1f;
        float curvalue = 0;
        while (true)
        {
            if (curvalue > targetvalue)
                break;

            yield return new WaitForFixedUpdate();
            curvalue += Time.deltaTime;
        }
        TimeValue = 1f;
        MoveSpeed = 0.2f;
    }

}
