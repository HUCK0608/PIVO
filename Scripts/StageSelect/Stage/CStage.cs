using UnityEngine;
using UnityEngine.SceneManagement;

public class CStage : MonoBehaviour
{
    /// <summary>씬 이름</summary>
    [SerializeField]
    private string _gameSceneName;

    /// <summary>주위에 연결된 스테이지들</summary>
    [SerializeField]
    private CStageInfo[] _connectedStages = null;

    /// <summary>스테이지 데이터(저장 & 로드)</summary>
    [SerializeField]
    private CStageData _data;

    /// <summary>스테이지 시작</summary>
    public void StartStage()
    {
    }

    /// <summary>해당 방향에 스테이지가 있는지 검사(스테이지가 없을 경우 null을 반환)</summary>
    public CStage IsHaveStage(EStageDirection direction)
    {
        for(int i = 0; i < _connectedStages.Length; i++)
        {
            if (_connectedStages[i].Direction.Equals(direction))
                return _connectedStages[i].Stage;
        }

        return null;
    }
}
