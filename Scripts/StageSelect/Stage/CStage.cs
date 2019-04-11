using UnityEngine;
using UnityEngine.SceneManagement;

public class CStage : MonoBehaviour
{
    [SerializeField]
    private string _gameSceneName = null;
    /// <summary>씬 이름</summary>
    public string GameSceneName { get { return _gameSceneName; } }

    [SerializeField]
    private int _maxBiscuitCount = 0;
    /// <summary>최대 비스킷 개수</summary>
    public int MaxBiscuitCount { get { return _maxBiscuitCount; } }

    [Space(20f)]
    /// <summary>주위에 연결된 스테이지들</summary>
    [SerializeField]
    private CStageInfo[] _connectedStages = null;

    

    /// <summary>스테이지 시작</summary>
    public void StartStage()
    {
        SceneManager.LoadSceneAsync(_gameSceneName);
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
