using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_CubeBro : MonoBehaviour
{
    public GameObject Corgi;
    public Material[] CubeBroTexture;
    public bool bUseDialogue;

    GameObject Text3D;
    MeshRenderer CubeBroMat;
    Animator Anim;
    float DistanceMinimal;
    float WaitDialogue;
    bool bUseCoroutine;
    void Start()
    {
        DistanceMinimal = 8f;
        WaitDialogue = 10f;

        Text3D = transform.Find("BillboardTEXT").Find("New Text").gameObject;
        Text3D.SetActive(false);
    }

    void Update()
    {
        if (!bUseCoroutine && bUseDialogue)
            CheckDistance();
    }

    public void ChangeTexture(int CubeTextureNumber)
    {
        GameObject HasTextureObject = transform.Find("cube bro-rigging").Find("cube_bro").gameObject;
        Material SetMaterial = CubeBroTexture[CubeTextureNumber];
        HasTextureObject.GetComponent<SkinnedMeshRenderer>().material = SetMaterial;
    }

    public void SetAnimState(string State)
    {
        Anim = transform.Find("cube bro-rigging").GetComponent<Animator>();
        if (State == "Run")
        {
            Anim.SetTrigger("IsRun");
        }
    }

    void CheckDistance()
    {
        float Distance = Vector3.Distance(Corgi.transform.position, transform.position);

        if (Distance < DistanceMinimal)
        {
            StartCoroutine("ShowSubtitle");
        }

    }

    IEnumerator ShowSubtitle()
    {
        bUseCoroutine = true;
        Text3D.SetActive(true);

        yield return new WaitForSeconds(2f);

        Text3D.SetActive(false);

        yield return new WaitForSeconds(WaitDialogue);

        bUseCoroutine = false;
    }
}
