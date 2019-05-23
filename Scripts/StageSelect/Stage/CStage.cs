using UnityEngine;
using UnityEngine.SceneManagement;

public class CStage : MonoBehaviour
{
    [Header("Anyone can edit")]
    [SerializeField]
    private string _gameSceneName = null;
    /// <summary>씬 이름</summary>
    public string GameSceneName { get { return _gameSceneName; } }

    [SerializeField]
    private int _maxBiscuitCount = 0;
    /// <summary>최대 비스킷 개수</summary>
    public int MaxBiscuitCount { get { return _maxBiscuitCount; } set { _maxBiscuitCount = value; } }

    [SerializeField]
    private int _haveBiscuitCount = 0;
    /// <summary>먹은 비스킷 개수</summary>
    public int HaveBiscuitCount { get { return _haveBiscuitCount; } set { _haveBiscuitCount = value; } }

    /// <summary>완벽한 클리어 여부</summary>
    public bool IsPerfectClear { get { return _haveBiscuitCount.Equals(_maxBiscuitCount); } }

    [SerializeField]
    private bool _isClear = false;
    /// <summary>클리어 여부</summary>
    public bool IsClear { get { return _isClear; } set { _isClear = value; } }

    [SerializeField]
    private bool _isUnlock = false;
    /// <summary>스테이지 잠김 여부</summary>
    public bool IsUnlock { get { return _isUnlock; } set { _isUnlock = value; } }

    [Header("Programmer can edit")]
    /// <summary>주위에 연결된 스테이지들</summary>
    [SerializeField]
    private CStageInfo[] _connectedStages = null;

    /// <summary>메쉬 랜더러</summary>
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

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

    /// <summary>셰이더 변경</summary>
    public void ChangeShader()
    {
        // Perfect clear
        if (_haveBiscuitCount.Equals(_maxBiscuitCount))
            _meshRenderer.material.SetFloat("_IsPerfectClear", 1f);
        // Clear
        else if (_isClear)
            _meshRenderer.material.SetFloat("_IsClear", 1f);
        // Unlock
        else if (_isUnlock)
            _meshRenderer.material.SetFloat("_IsUnlock", 1f);
    }
}
