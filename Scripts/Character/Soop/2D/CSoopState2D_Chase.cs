using UnityEngine;

public class CSoopState2D_Chase : CSoopState2D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _angryEmoticon = null;

    public override void InitState()
    {
        base.InitState();

        _angryEmoticon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(CUIManager.Instance.IsOnStageClearUI)
        {
            if (_angryEmoticon.gameObject.activeSelf)
                _angryEmoticon.gameObject.SetActive(false);
        }
        else
        {
            if (!_angryEmoticon.gameObject.activeSelf)
                _angryEmoticon.gameObject.SetActive(true);
        }

        Controller2D.MoveToPoint(CPlayerManager.Instance.RootObject2D.transform.position);

        if (_angryEmoticon.gameObject.activeSelf)
        {
            _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
            _angryEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        }

        if (Vector2.Distance(transform.position, CPlayerManager.Instance.RootObject2D.transform.position) <= Controller2D.Manager.Stat.PutDistance)
            Controller2D.ChangeState(ESoopState.PutInit);
        else if (!Controller2D.IsDetectionPlayer())
            Controller2D.ChangeState(ESoopState.Return);
    }

    public override void EndState()
    {
        base.EndState();

        _angryEmoticon.gameObject.SetActive(false);
    }
}
