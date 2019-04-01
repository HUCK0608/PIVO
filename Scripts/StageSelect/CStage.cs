using UnityEngine;
using UnityEngine.SceneManagement;

public class CStage : MonoBehaviour
{
    /// <summary>씬 이름</summary>
    [SerializeField]
    private string _gameSceneName;

    /// <summary>주위에 연결된 스테이지들</summary>
    [SerializeField]
    private CStageInfo[] _connectedStages;

    /// <summary>스테이지 시작</summary>
    public void StartStage()
    {
    }
}
