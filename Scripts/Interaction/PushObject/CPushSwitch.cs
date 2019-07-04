using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPushSwitch : MonoBehaviour
{
    /// <summary>플레이어가 진입했을 때 실행</summary>
    public void EnterPlayer()
    {
        StartCoroutine(InitSwitchLogic());
    }

    /// <summary>스위치 Init 로직</summary>
    private IEnumerator InitSwitchLogic()
    {

        while(true)
        {
            yield return null;
        }
    }

    /// <summary>스위치 상호작용 로직</summary>
    private IEnumerator InteractionSwitchLogic()
    {
        yield return null;
    }

    /// <summary>플레이어가 벗어낫을 때 실행</summary>
    public void ExitPlayer()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            CUIManager.Instance.SetActiveInteractionUI(true);
            StartCoroutine(InitSwitchLogic());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            CUIManager.Instance.SetActiveInteractionUI(false);
            StopAllCoroutines();
        }
    }
}
