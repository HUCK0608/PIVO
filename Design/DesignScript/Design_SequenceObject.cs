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
    void Start()
    {
        LoopNum = 1;
        OneTime = false;
        for (int i = 0; i < 13; i++)
        {
            CubeBroArray.Add(transform.Find("CubeBro_Red (" + i + ")").gameObject);
            CubePos.Add(CubeBroArray[i].transform.position);
            SetRandomTexture(i);
        }
    }

    void FixedUpdate()
    {
        if (WaitSeconds > 2.5f)
        {
            if (!OneTime)
            {
                OneTime = true;
                StartCoroutine("Waiting");

                for (int i = 0; i < 13; i++)
                {
                    SetAnimation(i, "Run");
                }
            }
        }
        else
        {
            WaitSeconds += Time.deltaTime;
            for (int i = 0; i < 13; i++)
            {
                int j = i + LoopNum;
                float MoveSpeed = 0.2f;

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
        yield return new WaitForSeconds(0.24f);

        LoopNum++;
        WaitSeconds = 0;
        OneTime = false;
    }

    void SetRandomTexture(int CubeNum)
    {
        int RandomValue = Random.RandomRange(0, 3);
        CubeBroArray[CubeNum].GetComponent<Design_CubeBro>().ChangeTexture(RandomValue);
    }

    void SetAnimation(int CubeNum, string State)
    {
        CubeBroArray[CubeNum].GetComponent<Design_CubeBro>().SetAnimState(State);
    }


}
