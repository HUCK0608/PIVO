using UnityEngine;

public class CSoopState2D_Idle : CSoopState2D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _sleepEmoticon = null;

    public override void InitState()
    {
        base.InitState();

        Vector3 newScale = Vector3.one;
        newScale.x = Controller2D.Manager.Stat.IsSoopDirectionRight ? -1 : 1;
        transform.localScale = newScale;
        
        _sleepEmoticon.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!Controller2D.CanOperation() || false == CSoopManager._isCanUseEmoticon)
        {
            if(_sleepEmoticon.gameObject.activeSelf)
                _sleepEmoticon.gameObject.SetActive(false);

            return;
        }
        else
        {
            if(!_sleepEmoticon.gameObject.activeSelf)
                _sleepEmoticon.gameObject.SetActive(true);
        }

        if (_sleepEmoticon.gameObject.activeSelf)
        {
            _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
            _sleepEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        }

        Vector3 startPoint = Controller2D.Manager.transform.position;

        if (Controller2D.IsDetectionPlayer())
            Controller2D.ChangeState(ESoopState.Surprise);
        else if (!transform.position.Equals(startPoint))
            Controller2D.ChangeState(ESoopState.Return);
    }

    public override void EndState()
    {
        base.EndState();

        _sleepEmoticon.gameObject.SetActive(false);
    }
}
