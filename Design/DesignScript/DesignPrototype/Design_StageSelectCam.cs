using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_StageSelectCam : MonoBehaviour
{
    public GameObject TargetActor;
    public GameObject FollowCamActor;
    public GameObject DialogueCamActor;
    Transform DialogueCamTrnasform;
    Transform FollowCamTransform;
    float MoveSpeed;
    bool bUseFollowTarget;
    void Start()
    {
        bUseFollowTarget = true;
        DialogueCamActor.SetActive(false);
        FollowCamActor.SetActive(true);
        FollowCamTransform = FollowCamActor.transform;
        DialogueCamTrnasform = DialogueCamActor.transform;
        MoveSpeed = 0.3f;
    }

    void Update()
    {
        if (bUseFollowTarget)
            FollowTarget();
        else
            DialogueCam();
    }

    void FollowTarget()
    {
        DialogueCamActor.SetActive(false);
        FollowCamActor.SetActive(true);

        if (transform.position != FollowCamTransform.position)
            transform.position = Vector3.MoveTowards(transform.position, FollowCamTransform.position, MoveSpeed);
        else
            transform.position = TargetActor.transform.position;
    }

    void DialogueCam()
    {
        DialogueCamActor.SetActive(true);
        FollowCamActor.SetActive(false);

        if (transform.position != DialogueCamTrnasform.position)
            transform.position = Vector3.MoveTowards(transform.position, DialogueCamTrnasform.position, MoveSpeed);
    }

    public void UseDialogueCamera()
    {
        if (bUseFollowTarget)
            bUseFollowTarget = false;
    }

    public void FollowCharacterCamera()
    {
        if (!bUseFollowTarget)
            bUseFollowTarget = true;
    }
}
