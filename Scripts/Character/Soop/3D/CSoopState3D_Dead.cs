using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopState3D_Dead : CSoopState3D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _stunEmoticon = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection(Controller3D.Manager.Stat.IsSoopDirectionRight ? Vector3.right : Vector3.left);

        _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
        _stunEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        _stunEmoticon.gameObject.SetActive(true);
    }

    private void Update()
    {
        _stunEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);

        AnimatorStateInfo currentAnimtorStateInfo = Controller3D.Animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimtorStateInfo.IsName("Dead"))
            Controller3D.ChangeAnimation();
    }

    public override void EndState()
    {
        base.EndState();

        _stunEmoticon.gameObject.SetActive(false);
    }
}
