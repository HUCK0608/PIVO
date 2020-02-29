using UnityEngine;

public class CPlayerState3D_PushIdle : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CUIManager.Instance.SetActiveMove3DTutorialUI(true);
    }

    public override void EndState()
    {
        base.EndState();

        CUIManager.Instance.SetActiveMove3DTutorialUI(false);
    }
}
