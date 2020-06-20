using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class CStageManager : MonoBehaviour
{
    private static CStageManager _instance;
    public static CStageManager Instance { get { return _instance; } }

    private List<CStage> _stages;
    /// <summary>스테이지들</summary>
    public List<CStage> Stages { get { return _stages; } }

    [Header("Programmer can edit")]
    [SerializeField]
    private EXmlDocumentNames _xmlDocumentName = EXmlDocumentNames.None;
    /// <summary>xml 문서 이름</summary>
    public EXmlDocumentNames XmlDocumentName { get { return _xmlDocumentName; } }

    /// <summary>속성들의 이름</summary>
    private string[] _elementsName = new string[] { "MaxBiscuitCount", "HaveBiscuitCount", "IsClear", "IsUnlock", "Stars", "RequirementStar1", "RequirementStar2", "RequirementStar3" };

    private int _currentSeasonTotalStar = 0;

    private int _totalStar = 0;
    public int TotalStar { get { return _totalStar; } }

    private void Awake()
    {
        _instance = this;

        InitStages();
        LoadStageDatas();
        InitUnlock();
        SaveStageDatas();

        CSteamAchievementManager.Instance.UpdateAchievment(CSteamAchievementManager.eSteamAchievementType.ALL_STAGE_PERFECTCLEAR);
    }

    private void Start()
    {
        ChangeStagesShader();
    }

    private void OnDestroy()
    {
        SaveStageDatas();
    }

    /// <summary>스테이지 초기화</summary>
    private void InitStages()
    {
        _stages = new List<CStage>();

        for (int i = 0; i < transform.childCount; i++)
            _stages.Add(transform.GetChild(i).GetComponent<CStage>());
    }

    /// <summary>스테이지 쉐이더 변경</summary>
    private void ChangeStagesShader()
    {
        for (int i = 0; i < _stages.Count; i++)
            _stages[i].ChangeShader();
    }

    /// <summary>스테이지 데이터들을 저장함</summary>
    private void SaveStageDatas()
    {
        int stageAmount = _stages.Count;

        string firstNodePath = _xmlDocumentName.ToString("G") + "/StageDatas/";

        string[] datas = null;
        // 데이터 쓰기
        for (int i = 0; i < stageAmount; i++)
        {
            datas = new string[] { _stages[i].MaxBiscuitCount.ToString(),
                                            _stages[i].HaveBiscuitCount.ToString(),
                                            _stages[i].IsClear.ToString(),
                                            _stages[i].IsUnlock.ToString(),
                                            _stages[i].Stars.ToString(),
                                            _stages[i].Requirements[0].ToString(),
                                            _stages[i].Requirements[1].ToString(),
                                            _stages[i].Requirements[2].ToString() };
            CDataManager.WritingDatas(_xmlDocumentName, firstNodePath + _stages[i].GameSceneName, _elementsName, datas);
        }

        // 파일 저장
        CDataManager.SaveCurrentXmlDocument();

        EXmlDocumentNames commonDataName = EXmlDocumentNames.CommonDatas;
        firstNodePath = commonDataName.ToString("G") + "/StageDatas";
        string[] elementsName = new string[] { _xmlDocumentName.ToString("G") + "TotalStar" };
        datas = new string[] { _currentSeasonTotalStar.ToString() };

        CDataManager.WritingDatas(commonDataName, firstNodePath, elementsName, datas);
        CDataManager.SaveCurrentXmlDocument();
    }

    /// <summary>스테이지 데이터들을 불러옴</summary>
    private void LoadStageDatas()
    {
        // 첫 번째 스테이지에 대한 잠금이 걸려 있을 경우 잠금을 해제
        if(!_stages[0].IsUnlock)
            _stages[0].IsUnlock = true;

        int stageAmount = _stages.Count;

        string firstNodePath = _xmlDocumentName.ToString("G") + "/StageDatas/";
        string[] datas = null;

        // 데이터 불러오기
        for (int i = 0; i < stageAmount; i++)
        {
            datas = CDataManager.ReadDatas(_xmlDocumentName, firstNodePath + _stages[i].GameSceneName, _elementsName);

            // 반환받은 데이터가 있을 경우에만 데이터를 가져옴
            if (datas != null)
            {
                if (datas[0] != null)
                    _stages[i].MaxBiscuitCount = int.Parse(datas[0]);

                if (datas[1] != null)
                    _stages[i].HaveBiscuitCount = int.Parse(datas[1]);
                else
                    _stages[i].HaveBiscuitCount = 0;

                if (datas[2] != null)
                    _stages[i].IsClear = datas[2].ToBoolean();
                else
                    _stages[i].IsClear = false;

                if (datas[3] != null)
                    _stages[i].IsUnlock = datas[3].ToBoolean();
                else
                    _stages[i].IsUnlock = false;

                if (datas[4] != null)
                {
                    _stages[i].Stars = int.Parse(datas[4]);
                    _currentSeasonTotalStar += _stages[i].Stars;
                }
                else
                    _stages[i].Stars = 0;

                if (datas[5] != null)
                    _stages[i].Requirements[0] = int.Parse(datas[5]);
                if (datas[6] != null)
                    _stages[i].Requirements[1] = int.Parse(datas[6]);
                if (datas[7] != null)
                    _stages[i].Requirements[2] = int.Parse(datas[7]);
            }
        }

        _totalStar = _currentSeasonTotalStar;

        EXmlDocumentNames commonDataName = EXmlDocumentNames.CommonDatas;
        firstNodePath = commonDataName.ToString("G") + "/StageDatas";
        string[] elementsName = null;

        if(_xmlDocumentName.Equals(EXmlDocumentNames.GrassStageDatas))
            elementsName = new string[] { EXmlDocumentNames.SnowStageDatas.ToString("G") + "TotalStar" };
        else if(_xmlDocumentName.Equals(EXmlDocumentNames.SnowStageDatas))
            elementsName = new string[] { EXmlDocumentNames.GrassStageDatas.ToString("G") + "TotalStar" };

        datas = CDataManager.ReadDatas(commonDataName, firstNodePath, elementsName);

        if (datas != null)
        {
            if (datas[0] != null)
                _totalStar += int.Parse(datas[0]);
        }
    }

    /// <summary>잠금 초기화</summary>
    private void InitUnlock()
    {
        for(int i = 0; i < _stages.Count - 1; i++)
        {
            if (_stages[i].IsClear)
                _stages[i + 1].IsUnlock = true;
        }
    }
}
