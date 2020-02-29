using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopState2D_Dead : CSoopState2D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _stunEmoticon = null;

    public override void InitState()
    {
        base.InitState();
        
        CPlayerManager.Instance.RemoveDetectionSoop(Controller2D.Manager.gameObject);
    }
    private void Update()
    {
        if(false == CSoopManager._isCanUseEmoticon)
        {
            if (_stunEmoticon.gameObject.activeSelf)
                _stunEmoticon.gameObject.SetActive(false);
        }
        else
        {
            if (!_stunEmoticon.gameObject.activeSelf)
                _stunEmoticon.gameObject.SetActive(true);
        }

        if (_stunEmoticon.gameObject.activeSelf)
        {
            _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
            _stunEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        }

        AnimatorStateInfo currentAnimtorStateInfo = Controller2D.Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimtorStateInfo.IsName("Dead"))
            Controller2D.ChangeAnimation();
    }

    public override void EndState()
    {
        base.EndState();

        _stunEmoticon.gameObject.SetActive(false);
    }
}
