using UnityEngine;
using UnityEngine.UI;

public class CUIManager_StageSelect : MonoBehaviour
{
    public static CUIManager_StageSelect _instance;
    public static CUIManager_StageSelect Instance { get { return _instance; } }

    /// <summary>스테이지 상태 이미지</summary>
    [SerializeField]
    private Image _stageStatImage = null;

    [SerializeField]
    private Sprite _perfectClearSprite = null;
    [SerializeField]
    private Sprite _clearSprite = null;
    [SerializeField]
    private Sprite _unlockSprite = null;

    /// <summary>스테이지 상태 텍스트</summary>
    [SerializeField]
    private Text _stageStatText = null;

    [SerializeField]
    private Color _perfectClearTextColor = Color.white;
    [SerializeField]
    private Color _clearTextColor = Color.white;
    [SerializeField]
    private Color _unlockTextColor = Color.white;

    private void Awake()
    {
        _instance = this;
    }

    /// <summary>
    /// 스테이지 상태 UI 설정
    /// </summary>
    /// <param name="_currentStage">스테이지</param>
    public void SetStageStatUI(CStage _currentStage)
    {
        if(_currentStage.IsPerfectClear)
        {
            _stageStatImage.sprite = _perfectClearSprite;
            _stageStatText.text = "Perfect Clear";
            _stageStatText.color = _perfectClearTextColor;
        }
        else if(_currentStage.IsClear)
        {
            _stageStatImage.sprite = _clearSprite;
            _stageStatText.text = _currentStage.HaveBiscuitCount.ToString() + " / " + _currentStage.MaxBiscuitCount.ToString();
            _stageStatText.color = _clearTextColor;
        }
        else
        {
            _stageStatImage.sprite = _unlockSprite;
            _stageStatText.text = "Unlock";
            _stageStatText.color = _unlockTextColor;
        }
    }
}
