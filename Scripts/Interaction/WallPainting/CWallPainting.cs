using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWallPainting : MonoBehaviour
{
    [SerializeField]
    private EPaintingType _paintingType = EPaintingType.Snow;

    [Header("페인팅 인덱스 (1이 처음)")]
    [SerializeField]
    private int _paintingIndex = 0;

    bool _isPlayerOnTrigger = false;

    private IEnumerator WallPaintingLogic()
    {
        CUIManager.Instance.SetActiveInteractionUI(true);

        bool isOnInteractionUI = true;

        while(_isPlayerOnTrigger && CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
        {
            if (!CUIManager.Instance.IsOnWallPainting)
            {
                if (CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.Idle) || 
                    CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.Move) ||
                    CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.Idle2) ||
                    CPlayerManager.Instance.Controller3D.CurrentState.Equals(EPlayerState3D.Idle3))
                {
                    if (!isOnInteractionUI)
                    {
                        isOnInteractionUI = true;
                        CUIManager.Instance.SetActiveInteractionUI(isOnInteractionUI);
                    }

                    if (Input.GetKeyDown(CKeyManager.InteractionKey))
                    {
                        CUIManager.Instance.SetActivePainting(true, _paintingType, _paintingIndex);
                    }
                }
                else if (isOnInteractionUI)
                {
                    isOnInteractionUI = false;
                    CUIManager.Instance.SetActiveInteractionUI(isOnInteractionUI);
                }
            }
            else
            {
                if (Input.GetKeyDown(CKeyManager.InteractionKey) || Input.GetKeyDown(CKeyManager.InteractionCancelKey))
                {
                    CUIManager.Instance.SetActivePainting(false);
                }

                if (isOnInteractionUI)
                {
                    isOnInteractionUI = false;
                    CUIManager.Instance.SetActiveInteractionUI(isOnInteractionUI);
                }
            }

            yield return null;
        }

        if (isOnInteractionUI)
            CUIManager.Instance.SetActiveInteractionUI(false);
    }

    public void PlayerOnTrigger()
    {
        _isPlayerOnTrigger = true;

        StartCoroutine(WallPaintingLogic());
    }

    public void PlayerOnTriggerExit()
    {
        _isPlayerOnTrigger = false;
    }
}
