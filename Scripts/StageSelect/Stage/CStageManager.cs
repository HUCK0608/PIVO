using UnityEngine;
using System.Collections.Generic;

public class CStageManager : MonoBehaviour
{
    /// <summary>속성들의 이름</summary>
    private static string[] _elementsName = new string[] { "MaxBiscuitCount", "HaveBiscuitCount", "IsUnlock" };

    /// <summary>스테이지들</summary>
    private List<CStage> _stages;

    /// <summary>데이터 파일 이름</summary>
    [SerializeField]
    private string _dataFileName = null;

    private void Awake()
    {
        InitStages();
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

    /// <summary>스테이지 데이터들을 저장함</summary>
    private void SaveStageDatas()
    {
        int stageAmount = _stages.Count;

        // Xml 초기화
        CDataManager.InitXmlDocument(_dataFileName, System.IO.FileMode.OpenOrCreate);

        // 데이터 쓰기
        for (int i = 0; i < stageAmount; i++)
        {
            string[] datas = new string[] { _stages[i].MaxBiscuitCount.ToString(), _stages[i].HaveBiscuitCount.ToString(), _stages[i].IsUnlock.ToString() };
            CDataManager.WritingData( "StageDatas/" + _stages[i].GameSceneName, _elementsName, datas);
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
            string[] datas = CDataManager.LoadData(_dataFileName, "StageDatas/" + _stages[i].GameSceneName, _elementsName);
            _stages[i].MaxBiscuitCount = int.Parse(datas[0]);
            _stages[i].HaveBiscuitCount = int.Parse(datas[1]);
            _stages[i].IsUnlock = datas[2].ToBoolean();
        }
    }
}
