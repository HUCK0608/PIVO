public class CSoopState2D_PutInit : CSoopState2D
{
    public override void InitState()
    {
        base.InitState();

        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.PutInit);
    }

    private void Update()
    {
        if (CPlayerManager.Instance.Stat.IsPut)
            Controller2D.ChangeState(ESoopState.PutMove);
    }
}
