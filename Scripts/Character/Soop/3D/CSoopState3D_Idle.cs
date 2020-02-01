using UnityEngine;

public class CSoopState3D_Idle : CSoopState3D
{
    [SerializeField]
    private Transform _emoticonPoint = null;
    [SerializeField]
    private Transform _sleepEmoticon = null;

    public override void InitState()
    {
        base.InitState();

        Controller3D.LookDirection(Controller3D.Manager.Stat.IsSoopDirectionRight ? Vector3.right : Vector3.left);

        _emoticonPoint.position = transform.position + new Vector3(1.4f, 2.5f);
        _sleepEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);
        _sleepEmoticon.gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 startPoint = Controller3D.Manager.transform.position;

        _sleepEmoticon.position = Camera.main.WorldToScreenPoint(_emoticonPoint.position);

        if (Controller3D.IsDetectionPlayer())
            Controller3D.ChangeState(ESoopState.Surprise);
        if (!transform.position.Equals(startPoint))
            Controller3D.ChangeState(ESoopState.Return);
    }

    public override void EndState()
    {
        base.EndState();

        _sleepEmoticon.gameObject.SetActive(false);
    }
}
