using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class CWand : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector Intro2PlayerDirector = null;
    [SerializeField]
    private GameObject Guide3DMove = null;
    private void Awake()
    {
        Intro2PlayerDirector.gameObject.SetActive(false);

        if (!CUIManager_Title._isUseTitle)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(CLayer.Player))
        {
            StartCoroutine(PlayIntroSequence());
        }
    }

    IEnumerator PlayIntroSequence()
    {
        Intro2PlayerDirector.gameObject.SetActive(true);
        Intro2PlayerDirector.Play();
        float TargetDuration = Mathf.Floor((float)Intro2PlayerDirector.duration);
        CPlayerManager.Instance.IsCanOperation = false;
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.DownIdle);
        CPlayerManager.Instance.Controller2D.Move(Vector3.zero);

        yield return new WaitUntil(() => Intro2PlayerDirector.time >= TargetDuration/2);

        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        CWorldManager.Instance.ChangeWorld();
        CPlayerManager.Instance.Controller2D.IsUseGravity = true;
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Idle);

        yield return new WaitUntil(() => Intro2PlayerDirector.time >= TargetDuration);
        CPlayerManager.Instance.IsCanOperation = true;
        CPlayerManager.Instance.Controller2D.IsUseGravity = true;
        CUIManager.Instance.IsCanOperation = true;
        Guide3DMove.SetActive(true);

        gameObject.SetActive(false);
    }
}
