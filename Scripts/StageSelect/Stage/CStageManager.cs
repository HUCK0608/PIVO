using UnityEngine;
using System.Collections.Generic;

public class CStageManager : MonoBehaviour
{
    private Dictionary<string, CStageData> _stageDatas;
    public CStageData StageData(string sceneName) { return _stageDatas[sceneName]; }

    private List<CStage> _stages;

    [SerializeField]
    private string _dataFileName = null;

    private void Awake()
    {
        DataManager.SaveData("Data", "AAAA/GGGG", "DataName123", "12341234");
        string data = DataManager.LoadData("aaaa", "AAAA/C", "DataName");
        //InitStages();
    }

    /// <summary>스테이지 초기화</summary>
    private void InitStages()
    {
        _stages = new List<CStage>();

        for (int i = 0; i < transform.childCount; i++)
            _stages.Add(transform.GetChild(i).GetComponent<CStage>());
    }

    /// <summary>스테이지 데이터를 불러옴</summary>
    private void LoadStageDatas()
    {
        _stageDatas = new Dictionary<string, CStageData>();

        int childCount = transform.childCount;

        for(int i = 0; i < childCount; i++)
        {
            CStageData data = DataManager.LoadData<CStageData>(_stages[i].GameSceneName);

            // 게임을 처음 시작한 경우
            if(i == 0 && data == null)
            {
                for(int j = 0; j < childCount; j++)
                {
                    CStageData newData = new CStageData();
                    newData.MaxBiscuitCount = _stages[i].MaxBiscuitCount;
                }

                break;
            }
        }
    }
}
