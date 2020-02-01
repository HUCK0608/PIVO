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

        _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
        _stunEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        _stunEmoticon.gameObject.SetActive(true);

    }
    private void Update()
    {
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
