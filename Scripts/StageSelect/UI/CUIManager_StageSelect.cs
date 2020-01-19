using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CUIManager_StageSelect : MonoBehaviour
{
    public static CUIManager_StageSelect _instance;
    public static CUIManager_StageSelect Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

        StartFadeIn();
    }

    private CStage _currentStage = null;

    [SerializeField]
    private Text _totalStarText = null;

    [SerializeField]
    private GameObject[] _stars = null;

    [SerializeField]
    private Text[] _requirementsText = null;

    /// <summary>
    /// 스테이지 상태 UI 설정
    /// </summary>
    /// <param name="_currentStage">스테이지</param>
    public void SetStageStatUI(CStage currentStage)
    {
        _currentStage = currentStage;

        SetStar();
        SetTotalStarText();
        SetRequirementText();
    }

    private void SetTotalStarText()
    {
        _totalStarText.text = string.Format("x {0}", CStageManager.Instance.GetTotalStar().ToString());
    }

    private void SetStar()
    {
        for (int i = 0; i < 3; i++)
            _stars[i].SetActive(false);

        for (int i = 0; i < _currentStage.Stars; i++)
            _stars[i].SetActive(true);
    }

    private void SetRequirementText()
    {
        for (int i = 0; i < 3; i++)
            _requirementsText[i].text = _currentStage.Requirements[i].ToString();
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
