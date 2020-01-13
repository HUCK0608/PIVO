using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClear_CamScript : MonoBehaviour
{
    void Start()
    {
        CPlayerManager.Instance.RootObject3D.transform.position = transform.Find("CenterTile").transform.position + (Vector3.up * 1);
    }
}
