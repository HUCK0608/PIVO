using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_StageSelectCam : MonoBehaviour
{
    public GameObject TargetActor;
    public GameObject FollowCamActor;
    public GameObject DialogueCamActor;
    public GameObject DialogueUI;
    bool bUseFollowTarget;
    void Start()
    {
        bUseFollowTarget = true;
        DialogueCamActor.SetActive(false);
        FollowCamActor.SetActive(true);
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
        DialogueUI.SetActive(false);
        transform.position = TargetActor.transform.position;
    }

    void DialogueCam()
    {
        DialogueCamActor.SetActive(true);
        FollowCamActor.SetActive(false);
        DialogueUI.SetActive(true);
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
