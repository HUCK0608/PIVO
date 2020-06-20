using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class CSteamAchievementManager : MonoBehaviour
{
    public enum eSteamAchievementType
    {
        ALL_STAGE_PERFECTCLEAR,
        CORGI_IDLE_SPECIAL,
        CORGI_DAMAGE_SOOP,
        CORGI_HOLDING,
        CORGI_HOLDING_CHANGE,
        STAGE_START_SPRING_1,
        STAGE_START_WINTER_1,
        STAGE_START_WINTER_8,
        STAGE_CLEAR_WINTER_8,
    }

    private static CSteamAchievementManager m_instance = null;
    public static CSteamAchievementManager Instance { get { return m_instance; } }

    private Dictionary<eSteamAchievementType, bool> m_dicTable = new Dictionary<eSteamAchievementType, bool>();

    private bool m_bInitialize = false;

    private void Awake()
    {
        m_instance = this;
    }

    public void Initialize()
    {
        m_bInitialize = GetAllAchievement();

        if(false == m_bInitialize)
        {
            StopAllCoroutines();
            StartCoroutine(CoInitialize());

            return;
        }

        SynchronizationOldUser();
    }

    private IEnumerator CoInitialize()
    {
        yield return new WaitUntil(() => (true == m_bInitialize));

        SynchronizationOldUser();
    }

    private void AddData(eSteamAchievementType _eSteamAchievementType, bool _bAchievementState)
    {
        m_dicTable[_eSteamAchievementType] = _bAchievementState;
    }

    private bool GetAllAchievement()
    {
        // 스팀 Init 실패
        if(false == SteamManager.Initialized || false == SteamUser.BLoggedOn())
        {
            return false;
        }

        m_dicTable.Clear();

        eSteamAchievementType[] arrSteamAchievementType = (eSteamAchievementType[])System.Enum.GetValues(typeof(eSteamAchievementType));

        foreach(eSteamAchievementType eSteamAchievementType in arrSteamAchievementType)
        {
            bool bAchievementState;

            SteamUserStats.GetAchievement(eSteamAchievementType.ToString(), out bAchievementState);

            AddData(eSteamAchievementType, bAchievementState);
        }

        return true;
    }

    public bool SetAchievment(eSteamAchievementType _eSteamAchievementType)
    {
        // 스팀 Init 실패
        if (false == SteamManager.Initialized || false == SteamUser.BLoggedOn())
        {
            return false;
        }

        // 로드 안됨 -> 로드 재시도
        if (false == m_bInitialize)
        {
            Initialize();
        }

        // 로드 안됨 -> 업적 설정 불가
        if (false == m_bInitialize)
        {
            return false;
        }

        // 이미 활성화 된 업적 -> false라고 해야하나?
        if(true == m_dicTable.ContainsKey(_eSteamAchievementType) && true == m_dicTable[_eSteamAchievementType])
        {
            return false;
        }

        bool bComplete = SteamUserStats.SetAchievement(_eSteamAchievementType.ToString());
        bComplete |= SteamUserStats.StoreStats();

        AddData(_eSteamAchievementType, bComplete);

        return bComplete;
    }

    // 기존 유저 동기화
    private void SynchronizationOldUser()
    {
        string[] strSpringStage1ClearData = CDataManager.ReadDatas(EXmlDocumentNames.GrassStageDatas, "GrassStageDatas/StageDatas/GrassStage_Stage1", new string[] { "IsClear" });

        if(null != strSpringStage1ClearData && null != strSpringStage1ClearData[0] && true == strSpringStage1ClearData[0].ToBoolean())
        {
            SetAchievment(eSteamAchievementType.STAGE_START_SPRING_1);
        }

        string[] strWinterStage1ClearData = CDataManager.ReadDatas(EXmlDocumentNames.SnowStageDatas, "SnowStageDatas/StageDatas/SnowStage_Stage1", new string[] { "IsClear" });

        if(null != strWinterStage1ClearData && null != strWinterStage1ClearData[0] && true == strWinterStage1ClearData[0].ToBoolean())
        {
            SetAchievment(eSteamAchievementType.STAGE_START_WINTER_1);
        }


        if(true == UpdateAchievment(eSteamAchievementType.STAGE_CLEAR_WINTER_8))
        {
            UpdateAchievment(eSteamAchievementType.STAGE_START_WINTER_8);
        }

        UpdateAchievment(eSteamAchievementType.ALL_STAGE_PERFECTCLEAR);
    }

    public bool UpdateAchievment(eSteamAchievementType _eSteamAchievementType)
    {
        bool bCanUpdate = false;

        switch(_eSteamAchievementType)
        {
            case eSteamAchievementType.ALL_STAGE_PERFECTCLEAR:
                bCanUpdate = CanUpdate_ALL_STAGE_PERFECTCLEAR();
                break;

            case eSteamAchievementType.CORGI_IDLE_SPECIAL:
                bCanUpdate = CanUpdate_CORGI_IDLE_SPECIAL();
                break;

            case eSteamAchievementType.STAGE_CLEAR_WINTER_8:
                bCanUpdate = CanUpdate_STAGE_CLEAR_WINTER_8();
                break;

            // 아래는 무조건 True
            case eSteamAchievementType.CORGI_HOLDING:
            case eSteamAchievementType.CORGI_HOLDING_CHANGE:
            case eSteamAchievementType.CORGI_DAMAGE_SOOP:
            case eSteamAchievementType.STAGE_START_SPRING_1:
            case eSteamAchievementType.STAGE_START_WINTER_1:
            case eSteamAchievementType.STAGE_START_WINTER_8:
                bCanUpdate = true;
                break;
        }

        bool bUpdate = false;

        if (true == bCanUpdate)
        {
            bUpdate = SetAchievment(_eSteamAchievementType);
        }

        return bUpdate;
    }

    private bool CanUpdate_CORGI_IDLE_SPECIAL()
    {
        bool bCanUpdate = false;

        if(10 <= CIdleSpecialCountManager.iCount)
        {
            bCanUpdate = true;
        }

        return bCanUpdate;
    }

    private bool CanUpdate_ALL_STAGE_PERFECTCLEAR()
    {
        bool bCanUpdate = false;

        string[] strTotalStarData = CDataManager.ReadDatas(EXmlDocumentNames.CommonDatas, "CommonDatas/StageDatas", new string[] { "GrassStageDatasTotalStar", "SnowStageDatasTotalStar" });

        if (null != strTotalStarData)
        {
            int iTotalStarCount = 0;

            if (null != strTotalStarData[0])
            {
                iTotalStarCount += int.Parse(strTotalStarData[0]);
            }
            if (null != strTotalStarData[1])
            {
                iTotalStarCount += int.Parse(strTotalStarData[1]);
            }

            if (45 <= iTotalStarCount)
            {
                bCanUpdate = true;
            }
        }

        return bCanUpdate;
    }

    private bool CanUpdate_STAGE_CLEAR_WINTER_8()
    {
        bool bCanUpdate = false;

        string[] strWinterStage8ClearData = CDataManager.ReadDatas(EXmlDocumentNames.SnowStageDatas, "SnowStageDatas/StageDatas/SnowStage_Stage8", new string[] { "IsClear" });

        if (null != strWinterStage8ClearData && null != strWinterStage8ClearData[0] && true == strWinterStage8ClearData[0].ToBoolean())
        {
            SetAchievment(eSteamAchievementType.STAGE_START_WINTER_8);
            SetAchievment(eSteamAchievementType.STAGE_CLEAR_WINTER_8);
        }

        return bCanUpdate;
    }


//    // For Cheat
//#if UNITY_EDITOR
//    private void Update()
//    { 
//        if(Input.GetKeyDown(KeyCode.A))
//        {
//            ClearAllAchievement();
//        }
//    }

//    private void ClearAllAchievement()
//    {
//        m_dicTable.Clear();

//        eSteamAchievementType[] arrSteamAchievementType = (eSteamAchievementType[])System.Enum.GetValues(typeof(eSteamAchievementType));

//        foreach (eSteamAchievementType eSteamAchievementType in arrSteamAchievementType)
//        {
//            SteamUserStats.ClearAchievement(eSteamAchievementType.ToString());
//            SteamUserStats.StoreStats();
//        }
//    }
//#endif
}
