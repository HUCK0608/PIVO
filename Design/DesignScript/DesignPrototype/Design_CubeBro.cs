using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_CubeBro : MonoBehaviour
{
    public Material[] CubeBroTexture;
    public bool bUseDialogue;
    public float DistanceMinimal;

    CWorldManager WorldManager;
    GameObject Corgi;
    GameObject Text3D;
    GameObject Cube2D, Cube3D;
    MeshRenderer CubeBroMat;
    Animator Anim;
    
    float WaitDialogue;
    bool bUseCoroutine;
    bool bState3D, bState2D, OutViewRect;

    void Start()
    {
        //DistanceMinimal = 15f;
        WaitDialogue = 10f;

        Text3D = transform.Find("BillboardTEXT").Find("New Text").gameObject;
        Text3D.SetActive(false);

        Corgi = GameObject.Find("PlayerGroup").transform.Find("Root3D").gameObject;
        SetWorldPreference();

        WorldManager = GameObject.Find("World").GetComponent<CWorldManager>();
    }

    void Update()
    {
        if (!bUseCoroutine && bUseDialogue)
            CheckDistance();

        SetWorld();
    }

    void SetWorldPreference()
    {
        bState3D = true;
        bState2D = false;
        OutViewRect = true;
        //Cube2D = transform.Find("Root2D").gameObject;
        Cube3D = transform.Find("cube bro-rigging").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            OutViewRect = true;
        }
    }

    void SetWorld()
    {
        if (WorldManager)
        {
            if (WorldManager.CurrentWorldState == EWorldState.View2D)
            {
                if (bState3D)//2D로 바꾸기
                {
                    //Set3DRender(false);
                    bState2D = true;
                    bState3D = false;

                    if (OutViewRect)
                    {
                        Set3DRender(false);
                        //Cube2D.SetActive(false);
                    }
                    //else
                        //Cube2D.SetActive(true);
                }
            }
            else
            {
                if (bState2D)//3D로 바꾸기
                {
                    Set3DRender(true);
                    //Cube2D.SetActive(false);
                    bState3D = true;
                    bState2D = false;
                    OutViewRect = true;
                }
            }
        }
    }

    void Set3DRender(bool bState)
    {
        Cube3D.SetActive(bState);
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
            //StartCoroutine("ShowSubtitle");
            Text3D.SetActive(true);
        }
        else
            Text3D.SetActive(false);

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
