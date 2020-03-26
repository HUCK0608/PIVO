using UnityEngine;

public class CSoopState3D_PutInit : CSoopState3D
{
    [SerializeField]
    private Transform _putInitPoint = null;

    private float _increaseValue = 0.4f;
    private float _addTime = 0f;

    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.PutInit);

        _addTime = 0f;
    }

    private void Update()
    {
        _addTime += Time.deltaTime;
        Transform player3DTransform = CPlayerManager.Instance.Controller3D.transform;
        player3DTransform.position = Vector3.MoveTowards(player3DTransform.transform.position, _putInitPoint.position, Mathf.Clamp(_increaseValue * _addTime, 0f, 1f));

        if (CPlayerManager.Instance.Stat.IsPut)
            Controller3D.ChangeState(ESoopState.PutMove);
    }
}
