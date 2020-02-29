using UnityEngine;

public class CSoopState3D_Chase : CSoopState3D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _angryEmoticon = null;

    public override void InitState()
    {
        base.InitState();

        _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
        _angryEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        _angryEmoticon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (false == CSoopManager._isCanUseEmoticon)
        {
            if (_angryEmoticon.gameObject.activeSelf)
                _angryEmoticon.gameObject.SetActive(false);
        }
        else
        {
            if (!_angryEmoticon.gameObject.activeSelf)
                _angryEmoticon.gameObject.SetActive(true);
        }

        Controller3D.MoveToPoint(CPlayerManager.Instance.RootObject3D.transform.position);

        if (_angryEmoticon.gameObject.activeSelf)
        {
            _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
            _angryEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        }

        if (Vector3.Distance(transform.position, CPlayerManager.Instance.RootObject3D.transform.position) <= Controller3D.Manager.Stat.PutDistance)
            Controller3D.ChangeState(ESoopState.PutInit);
        else if (!Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Return);
    }

    public override void EndState()
    {
        base.EndState();

        _angryEmoticon.gameObject.SetActive(false);
    }
}
