using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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
    private GameObject[] _starsLine = null;

    [SerializeField]
    private Text[] _requirementsText = null;

    [SerializeField]
    private Text _stageText = null;

    bool _isActiveStar = true;

    /// <summary>
    /// 스테이지 상태 UI 설정
    /// </summary>
    /// <param name="_currentStage">스테이지</param>
    public void SetStageStatUI(CStage currentStage)
    {
        _currentStage = currentStage;

        SetActivateStar(currentStage.IsUseStarUI);
        SetStar();
        SetTotalStarText();
        SetRequirementText();
        SetStageText();
    }

    private void SetTotalStarText()
    {
        _totalStarText.text = string.Format("x {0}", CStageManager.Instance.GetTotalStar().ToString());
    }

    private void SetActivateStar(bool active)
    {
        if (!active)
        {
            for (int i = 0; i < 3; i++)
            {
                _starsLine[i].SetActive(active);
                _stars[i].SetActive(active);
                if (i > 0)
                    _requirementsText[i].gameObject.SetActive(active);
            }
        }

        _isActiveStar = active;
    }

    private void SetStar()
    {
        if (!_isActiveStar)
            return;

        for (int i = 0; i < 3; i++)
        {
            _stars[i].SetActive(false);
            _starsLine[i].SetActive(true);
        }

        for (int i = 0; i < _currentStage.Stars; i++)
        {
            _stars[i].SetActive(true);
        }
    }

    private void SetRequirementText()
    {
        if (!_isActiveStar)
            return;

        for (int i = 0; i < 3; i++)
        {
            if (i > 0)
                _requirementsText[i].gameObject.SetActive(true);

            _requirementsText[i].text = _currentStage.Requirements[i].ToString();
        }
    }

    private void SetStageText()
    {
        _stageText.text = _currentStage.StageNameForUI;
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

    #region ReturnToTitle

    [SerializeField]
    private GameObject _returnToTitleGroup = null;
    [SerializeField]
    private GameObject _yes = null, _yesSelect = null;
    [SerializeField]
    private GameObject _no = null, _noSelect = null;

    private int _currentSelectMenu_ReturnToTitle = 0;

    private bool _isExcutionAnything = false;

    public void SetActivateReturnToTitle(bool active)
    {
        _returnToTitleGroup.SetActive(active);
        CCameraController_StageSelect.Instance.SetActivateBlur(active);

        if (active)
        {
            SetSelectMenu_ReturnToTitle(0);
            PlayerCanOperationOff();
        }
        else
            Invoke("PlayerCanOperationOn", Time.deltaTime);
    }

    public void SetSelectMenu_ReturnToTitle(int selectMenu)
    {
        _currentSelectMenu_ReturnToTitle = selectMenu;

        if (_currentSelectMenu_ReturnToTitle == 0)
        {
            SetActivateYes_ReturnToTitle(true);
            SetActivateNo_ReturnToTitle(false);
        }
        else
        {
            SetActivateYes_ReturnToTitle(false);
            SetActivateNo_ReturnToTitle(true);
        }
    }

    private void Update()
    {
        if (!CUIManager_StageSelect.Instance.IsFadeInOut)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_returnToTitleGroup.activeSelf)
                SetActivateReturnToTitle(true);
            else if ((Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape)) && _returnToTitleGroup.activeSelf)
                SetActivateReturnToTitle(false);

            if (_returnToTitleGroup.activeSelf)
                ReturnToTitleLogic();
        }
    }

    private void ReturnToTitleLogic()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SetSelectMenu_ReturnToTitle((_currentSelectMenu_ReturnToTitle + 1) % 2);
        else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
            ExcuteSelectMenu_ReturnToTitle(_currentSelectMenu_ReturnToTitle);
    }

    private void SetActivateYes_ReturnToTitle(bool active)
    {
        _yesSelect.SetActive(active);
        _yes.SetActive(!active);
    }

    private void SetActivateNo_ReturnToTitle(bool active)
    {
        _noSelect.SetActive(active);
        _no.SetActive(!active);
    }

    public void ExcuteSelectMenu_ReturnToTitle(int selectMenu)
    {
        if (_isExcutionAnything)
            return;

        if (selectMenu == 0)
            ExcuteYes_ReturnToTitle();
        else
            ExcuteNo_ReturnToTitle();
    }

    private void ExcuteYes_ReturnToTitle()
    {
        SceneManager.LoadScene("GrassStage_Stage1");
        _isExcutionAnything = true;
    }

    private void ExcuteNo_ReturnToTitle()
    {
        SetActivateReturnToTitle(false);
    }

    #endregion

    public void PlayerCanOperationOff()
    {
        CPlayerController_StageSelect.Insatnce.IsCanOperation = false;
    }

    public void PlayerCanOperationOn()
    {
        CPlayerController_StageSelect.Insatnce.IsCanOperation = true;
    }
}
