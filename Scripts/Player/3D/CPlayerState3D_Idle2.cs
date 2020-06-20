using UnityEngine;

public static class CIdleSpecialCountManager
{
    public static readonly string strIdleSpecialCountNodePath = "CommonDatas/PlayerDatas";
    public static readonly string[] strIdleSpecialCountElementName = new string[] { "IdleSpecialCount" };

    public static int iCount = 0;
    private static bool bInitialize = false;

    private static void Initialize()
    {
        if (true == bInitialize)
        {
            return;
        }

        string[] strDatas = CDataManager.ReadDatas(EXmlDocumentNames.CommonDatas, strIdleSpecialCountNodePath, strIdleSpecialCountElementName);

        if (null != strDatas && 0 < strDatas.Length)
        {
            iCount = int.Parse(strDatas[0]);
        }

        bInitialize = true;
    }

    public static void CountUp()
    {
        Initialize();

        iCount++;

        string[] strDatas = new string[] { iCount.ToString() };

        CDataManager.WritingDatas(EXmlDocumentNames.CommonDatas, strIdleSpecialCountNodePath, strIdleSpecialCountElementName, strDatas);
        CDataManager.SaveCurrentXmlDocument();

        CSteamAchievementManager.Instance.UpdateAchievment(CSteamAchievementManager.eSteamAchievementType.CORGI_IDLE_SPECIAL);
    }
}

public class CPlayerState3D_Idle2 : CPlayerState3D
{
    public override void InitState()
    {
        base.InitState();

        CIdleSpecialCountManager.CountUp();
    }

    private void Update()
    {
        float vertical = Input.GetAxis(CString.Vertical);
        float horizontal = Input.GetAxis(CString.Horizontal);

        Controller3D.Move(vertical, horizontal);

        if (Controller3D.RigidBody.velocity.y < -Mathf.Epsilon)
            Controller3D.ChangeState(EPlayerState3D.Falling);
        else if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey))
            Controller3D.ChangeState(EPlayerState3D.ViewChangeInit);
        else if (Input.GetKeyDown(CKeyManager.ClimbKey) && Controller3D.IsCanClimb())
            Controller3D.ChangeState(EPlayerState3D.Climb);
        else if (Controller3D.RigidBody.velocity.x != 0 || Controller3D.RigidBody.velocity.z != 0)
            Controller3D.ChangeState(EPlayerState3D.Move);
    }
}
