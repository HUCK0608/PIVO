using UnityEngine;

public class CPlayerState2D_Dead : CPlayerState2D
{
    public override void InitState()
    {
        base.InitState();

        CUIManager.Instance.ActiveDeadUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            CUIManager.Instance.DeadUIChangeSelectMenu();
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            CUIManager.Instance.DeadUIExcutionSelectMenu();
    }
}
