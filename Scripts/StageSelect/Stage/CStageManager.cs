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
    /// <summary>데이터 파일 이름</summary>
    [SerializeField]
    private string _dataFileName = null;
    public string DataFileName { get { return _dataFileName; } }

    /// <summary>노드 경로</summary>
    private string _nodePath = "GrassStageDatas/StageDatas/";
    /// <summary>속성들의 이름</summary>
    private string[] _elementsName = new string[] { "MaxBiscuitCount", "HaveBiscuitCount", "IsClear", "IsUnlock" };

    private void Awake()
    {
        _instance = this;

        InitStages();
        LoadStageDatas();
    }

    private void Start()
    {
        ChangeStagesShader();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
            SaveStageDatas();
        else if (Input.GetKeyDown(KeyCode.F6))
            LoadStageDatas();
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

        // Xml 초기화
        CDataManager.InitXmlDocument(_dataFileName, FileMode.OpenOrCreate);

        // 데이터 쓰기
        for (int i = 0; i < stageAmount; i++)
        {
            string[] datas = new string[] { _stages[i].MaxBiscuitCount.ToString(), _stages[i].HaveBiscuitCount.ToString(), _stages[i].IsClear.ToString(), _stages[i].IsUnlock.ToString() };
            CDataManager.WritingData( _nodePath + _stages[i].GameSceneName, _elementsName, datas);
        }

        // 파일 저장
        CDataManager.SaveFile(_dataFileName);
    }

    /// <summary>스테이지 데이터들을 불러옴</summary>
    private void LoadStageDatas()
    {
        int stageAmount = _stages.Count;

        // Xml 초기화
        CDataManager.InitXmlDocument(_dataFileName);

        // 데이터 불러오기
        for(int i = 0; i < stageAmount; i++)
        {
            string[] datas = CDataManager.LoadData(_nodePath + _stages[i].GameSceneName, _elementsName);

            // 데이터가 존재하지 않는다면 첫 번째 스테이지의 잠금을 풀고 데이터를 저장한다.
            if (datas == null)
            {
                _stages[0].IsUnlock = true;
                SaveStageDatas();
                break;
            }

            _stages[i].MaxBiscuitCount = int.Parse(datas[0]);
            _stages[i].HaveBiscuitCount = int.Parse(datas[1]);
            _stages[i].IsClear = datas[2].ToBoolean();
            _stages[i].IsUnlock = datas[3].ToBoolean();
        }
    }
}
