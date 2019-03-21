using UnityEngine;

public class CPlayerState3D_ViewChangeInit : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        Controller3D.Move(Vector3.zero);

        StartCoroutine(Controller3D.ViewChangeRect.IncreaseScaleXYLogic());
    }

    private void Update()
    {
        if (!Controller3D.ViewChangeRect.IsOnIncreaseScaleXY)
            Controller3D.ChangeState(EPlayerState3D.ViewChangeIdle);
    }
}
