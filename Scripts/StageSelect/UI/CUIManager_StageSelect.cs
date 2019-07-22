using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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

        StartFadeIn();
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

    /// <summary>검은 화면 이미지</summary>
    [SerializeField]
    private Image _blackBG = null;
    /// <summary>검은 화면 알파에 더해질 수치</summary>
    [SerializeField]
    private float _blackBGIncreaseValue = 0f;

    private bool _isFadeInOrOut = false;
    /// <summary>페이드 인 또는 아웃이 실행중인지 여부</summary>
    public bool IsFadeInOut { get { return _isFadeInOrOut; } }

    /// <summary>점점 밝아지며 화면이 보임</summary>
    public void StartFadeIn()
    {
        StartCoroutine(BlackBGIncreaseAlphaLogic(-_blackBGIncreaseValue, 1f, 0f));
    }

    /// <summary>점점 어두워지며 화면이 안보임</summary>
    public void StartFadeOut()
    {
        StartCoroutine(BlackBGIncreaseAlphaLogic(_blackBGIncreaseValue, 0f, 1f));
    }

    /// <summary>검은 화면 값 증가(목표값까지)</summary>
    private IEnumerator BlackBGIncreaseAlphaLogic(float value, float startAlpha, float finalAlpha)
    {
        _isFadeInOrOut = true;
        _blackBG.gameObject.SetActive(true);

        // 시작값 설정
        Color currentColor = Color.black;
        currentColor.a = startAlpha;
        _blackBG.color = currentColor;

        while(!currentColor.a.Equals(finalAlpha))
        {
            currentColor.a = Mathf.Clamp01(currentColor.a + value * Time.deltaTime);
            _blackBG.color = currentColor;

            yield return null;
        }

        _isFadeInOrOut = false;
    }
}
