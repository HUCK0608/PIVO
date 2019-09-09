public class CSoopState3D_Idle : CSoopState3D
{
    private void Update()
    {
        if (Controller.IsDetectionPlayer())
            Controller.ChangeState(ESoopState.Move);
    }
}
