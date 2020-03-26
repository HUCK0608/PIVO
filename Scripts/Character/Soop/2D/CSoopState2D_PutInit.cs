using UnityEngine;

public class CSoopState2D_PutInit : CSoopState2D
{
    [SerializeField]
    private Transform _putInitPoint = null;

    private float _increaseValue = 1.25f;
    private float _addTime = 0f;

    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.PutInit);

        _addTime = 0f;
    }

    private void Update()
    {
        _addTime += Time.deltaTime;
        Transform player2DTransform = CPlayerManager.Instance.Controller2D.transform;
        Vector3 destination = _putInitPoint.position;
        destination.z = player2DTransform.position.z;
        player2DTransform.position = Vector3.MoveTowards(player2DTransform.transform.position, destination, Mathf.Clamp(_increaseValue * _addTime, 0f, 1f));

        if (CPlayerManager.Instance.Stat.IsPut)
            Controller2D.ChangeState(ESoopState.PutMove);
    }
}
