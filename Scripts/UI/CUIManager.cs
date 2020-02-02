using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public enum EPaintingType { Snow = 0, Temple };

public class CUIManager : MonoBehaviour
{
    private static CUIManager _instance = null;
    public static CUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

        _interactionKeyText.text = CKeyManager.InteractionKey.ToString("G");

        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(_returnToTitleGroup.activeSelf)
            {
                SetActiveReturnToTitle(false);
                return;
            }
            else if(_restartGroup.activeSelf)
            {
                SetActiveRestart(false);
                return;
            }

            if (!_pauseGroup.activeSelf)
                SetActivePause(true);
            else
                SetActivePause(false);
        }

        if (_stageClear.activeSelf)
            StageClearLogic();

        if (_pauseGroup.activeSelf)
            PauseLogic();

        if (_returnToTitleGroup.activeSelf)
            ReturnToTitleLogic();

        if (_restartGroup.activeSelf)
            RestartLogic();
    }

    /// <summary>캔버스</summary>
    [SerializeField]
    private Canvas _canvas = null;

    /// <summary>목표 디스플레이 설정</summary>
    public void SetTargetDisplay(int targetValue)
    {
        _canvas.targetDisplay = targetValue;
    }

    #region InGameUI

    [SerializeField]
    private GameObject _inGameUI = null;

    public void SetActiveInGameUI(bool active)
    {
        _inGameUI.SetActive(active);
    }

    /// <summary>체력 애니메이션 리스트</summary>
    [SerializeField]
    private List<Animation> _hpAnimations = null;

    /// <summary>
    /// 체력 UI 설정
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    public void SetHpUI(int currentHp)
    {
        _hpAnimations[currentHp].Play();
    }

    /// <summary>비스킷 개수 텍스트</summary>
    [SerializeField]
    private Text _biscuitCountText = null;

    /// <summary>
    /// 비스킷 UI 설정
    /// </summary>
    /// <param name="currentBiscuitCount">현재 비스킷 개수</param>
    public void SetBiscuitUI(int currentBiscuitCount)
    {
        _biscuitCountText.text = string.Format("X {0}", currentBiscuitCount.ToString());
    }

    /// <summary>상호작용 그룹</summary>
    [SerializeField]
    private GameObject _interactionGroup = null;
    /// <summary>사용 수</summary>
    private int _useCount = 0;

    /// <summary>상호작용 키 텍스트</summary>
    [SerializeField]
    private Text _interactionKeyText = null;

    /// <summary>상호작용 UI 활성화 설정</summary>
    public void SetActiveInteractionUI(bool value)
    {
        _useCount = value ? _useCount + 1 : _useCount - 1;
        if (_useCount < 0)
            _useCount = 0;

        if (_useCount.Equals(0))
            _interactionGroup.SetActive(false);
        else if (!_interactionGroup.activeSelf)
            _interactionGroup.SetActive(true);
    }

    [SerializeField]
    private GameObject _bombExplosionGroup = null;

    public void SetActiveBombExplosionUI(bool value)
    {
        _bombExplosionGroup.SetActive(value);
    }

    [SerializeField]
    private GameObject _holdingUIGroup = null;

    public void SetHoldingUI(bool value) { _holdingUIGroup.SetActive(value); }

    #endregion

    #region DeadUI

    [SerializeField]
    private GameObject _deadGroup = null;
    /// <summary>재시작 및 스테이지 선택 이미지</summary>
    [SerializeField]
    private Image _retryImage = null, _stageSelectImage = null;
    /// <summary>기본 및 빛나는 재시작 스프라이트</summary>
    [SerializeField]
    private Sprite _defaultRetrySprite = null, _glowRetrySprite = null;
    /// <summary>기본 및 빛나는 스테이지 선택 스프라이트</summary>
    [SerializeField]
    private Sprite _defaultStageSelectSprite = null, _glowStageSelectSprite = null;

    private bool _isSelectRetry = true;
    private bool _isEndDeadUI = false;

    public void ActiveDeadUI()
    {
        _deadGroup.SetActive(true);
    }

    /// <summary>죽음 UI 선택 메뉴 변경</summary>
    public void DeadUIChangeSelectMenu()
    {
        if (!_retryImage.raycastTarget)
            return;

        if (_isSelectRetry)
            StageSelectPointEnter();
        else
            RetryPointEnter();
    }

    /// <summary>죽음 UI 선택 메뉴 실행</summary>
    public void DeadUIExcutionSelectMenu()
    {
        if (!_retryImage.raycastTarget)
            return;

        if (_isSelectRetry)
            RetryPointUp();
        else
            StageSelectPointUp();
    }

    /// <summary>재시작 포인터 엔터 이벤트</summary>
    public void RetryPointEnter()
    {
        _isSelectRetry = true;

        PlayPointerEnterAudio();

        _retryImage.sprite = _glowRetrySprite;
        _stageSelectImage.sprite = _defaultStageSelectSprite;
    }

    /// <summary>스테이지 선택 포인터 엔터 이벤트</summary>
    public void StageSelectPointEnter()
    {
        _isSelectRetry = false;

        PlayPointerEnterAudio();

        _retryImage.sprite = _defaultRetrySprite;
        _stageSelectImage.sprite = _glowStageSelectSprite;
    }

    /// <summary>재시작 클릭 이벤트</summary>
    public void RetryPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        PlayPointerUpAudio();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>스테이지 선택 클릭 이벤트</summary>
    public void StageSelectPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        PlayPointerUpAudio();

        string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
        SceneManager.LoadScene("StageSelect_" + season);
    }

    #endregion

    #region Sound
    private AudioSource _audioSource = null;

    [SerializeField]
    private AudioClip[] _pointerEnterAudioClips = null, _pointerUpAudioClips = null;

    public void PlayPointerEnterAudio()
    {
        int randomValue = Random.Range(1, 101);

        if (randomValue <= 20)
            _audioSource.clip = _pointerEnterAudioClips[0];
        else if (randomValue <= 40)
            _audioSource.clip = _pointerEnterAudioClips[1];
        else if (randomValue <= 60)
            _audioSource.clip = _pointerEnterAudioClips[2];
        else if (randomValue <= 80)
            _audioSource.clip = _pointerEnterAudioClips[3];
        else
            _audioSource.clip = _pointerEnterAudioClips[4];

        _audioSource.Play();
    }

    public void PlayPointerUpAudio()
    {
        int randomValue = Random.Range(1, 100);

        if (randomValue <= 33)
            _audioSource.clip = _pointerUpAudioClips[0];
        else if (randomValue <= 66)
            _audioSource.clip = _pointerUpAudioClips[1];
        else
            _audioSource.clip = _pointerUpAudioClips[2];

        _audioSource.Play();
    }
    #endregion

    #region WallPainting

    [Header("WallPainting")]
    [SerializeField]
    private GameObject _goWallPaintingGroup = null;

    [Space(10f)]
    // Snow
    [SerializeField]
    private GameObject _goSnowPaintingGroup = null;
    [SerializeField]
    private List<GameObject> _goArrSnowPainting = null;

    [Space(10f)]
    // Temple
    [SerializeField]
    private GameObject _goTemplePaintingGroup = null;
    [SerializeField]
    private List<GameObject> _goArrTemplePainting = null;

    private bool _isOnWallPainting = false;
    /// <summary>벽화가 켜져있는지 여부</summary>
    public bool IsOnWallPainting { get { return _isOnWallPainting; } }

    /// <summary>
    /// 페인팅 활성화 설정
    /// </summary>
    /// <param name="paintingType">페인팅 타입(겨울, 신전)</param>
    /// <param name="index">페인팅 인덱스</param>
    /// <param name="active">활성화 여부</param>
    public void SetActivePainting(bool active, EPaintingType paintingType = EPaintingType.Snow, int index = 0)
    {
        CPlayerManager.Instance.IsCanOperation = !active;

        _isOnWallPainting = active;
        index -= 1;

        if (!active)
        {
            SetActivePaintingGroup(active);
        }
        else
        {
            ActivatePainting(paintingType, index);
            ActivatePaintingType(paintingType);
            SetActivePaintingGroup(true);
            CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Idle);
        }
    }

    private void SetActivePaintingGroup(bool active)
    {
        _goWallPaintingGroup.SetActive(active);
    }

    private void ActivatePainting(EPaintingType paintingType, int index)
    {
        if(paintingType.Equals(EPaintingType.Snow))
        {
            for (int i = 0; i < _goArrSnowPainting.Count; i++)
                SetActiveSnowPainting(i, false);

            SetActiveSnowPainting(index, true);
        }
        else if(paintingType.Equals(EPaintingType.Temple))
        {
            for (int i = 0; i < _goArrTemplePainting.Count; i++)
                SetActiveTemplePainting(i, false);

            SetActiveTemplePainting(index, true);
        }
    }

    private void SetActiveSnowPainting(int index, bool active)
    {
        _goArrSnowPainting[index].SetActive(active);
    }

    private void SetActiveTemplePainting(int index, bool active)
    {
        _goArrTemplePainting[index].SetActive(active);
    }

    private void ActivatePaintingType(EPaintingType paintingType)
    {
        SetActiveSnowPaintingGroup(false);
        SetActiveTemplePaintingGroup(false);

        if (paintingType.Equals(EPaintingType.Snow))
            SetActiveSnowPaintingGroup(true);
        else if (paintingType.Equals(EPaintingType.Temple))
            SetActiveTemplePaintingGroup(true);
    }

    private void SetActiveSnowPaintingGroup(bool active)
    {
        _goSnowPaintingGroup.SetActive(active);
    }

    private void SetActiveTemplePaintingGroup(bool active)
    {
        _goTemplePaintingGroup.SetActive(active);
    }

    public void UIWallPaintingButtonClick()
    {
        SetActivePainting(false);
    }

    #endregion

    #region StageClear

    [SerializeField]
    private GameObject _stageClear = null;

    [SerializeField]
    private GameObject[] _grayStars = null;
    [SerializeField]
    private GameObject[] _yellowStars = null;

    [SerializeField]
    private Text[] _stageClearRequirementTexts = null;

    private void StageClearLogic()
    {
        if (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) || Input.GetKeyDown(KeyCode.Return))
            UIStageClearOkButtonClick();
    }

    public void SetActiveStageClear(bool active)
    {
        if (active)
        {
            SetStar();
            SetStageClearRequirementText();
        }

        SetTargetDisplay(0);

        _stageClear.SetActive(active);

        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    private void SetStar()
    {
        for(int i = 0; i < CBiscuitManager.Instance.GetCurrentStar(); i++)
            SetActiveYellowStar(i, true);
    }

    private void SetActiveYellowStar(int index, bool active)
    {
        _yellowStars[index].SetActive(active);
        _grayStars[index].SetActive(!active);
    }

    private void SetStageClearRequirementText()
    {
        int[] requirement = CBiscuitManager.Instance.Requirements;

        for(int i = 0; i < 3; i ++)
            _stageClearRequirementTexts[i].text = string.Format("{0}", requirement[i].ToString());
    }

    public void UIStageClearOkButtonClick()
    {
        if (CWorldManager.Instance.IsUseTimeLineScene)
            CWorldManager.Instance.StageClearWaitTimeLineScene(CWorldManager.Instance.TimeLineSceneName);
        else
            CWorldManager.Instance.StageClear();
    }

    #endregion

    #region Pause

    [SerializeField]
    private GameObject _pauseGroup = null;

    [SerializeField]
    private GameObject _resume = null, _resumeSelect = null;
    [SerializeField]
    private GameObject _restart = null, _restartSelect = null;
    [SerializeField]
    private GameObject _options = null, _optionsSelect = null;
    [SerializeField]
    private GameObject _exit = null, _exitSelect = null;

    private int _pauseSelectMenu = 0;

    private bool _isInitPause = false;

    public void SetActivePause(bool active, bool isSetPlayerOperation = true)
    {
        _isInitPause = false;
        _pauseGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else if(isSetPlayerOperation)
            Invoke("PlayerCanOperationTrue", Time.deltaTime);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitPause();
    }

    private void InitPause()
    {
        SetSelectMenu_Pause(0);

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Idle);
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Idle);
    }

    private void PauseLogic()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
            SetSelectMenu_Pause(Mathf.Clamp(_pauseSelectMenu - 1, 0, 3));
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            SetSelectMenu_Pause(Mathf.Clamp(_pauseSelectMenu + 1, 0, 3));

        if (_isInitPause && (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) || Input.GetKeyDown(KeyCode.Return)))
            ExcuteMenu_Pause(_pauseSelectMenu);
        else if (!_isInitPause)
            _isInitPause = true;
    }

    public void SetSelectMenu_Pause(int menuIndex)
    {
        _pauseSelectMenu = menuIndex;

        SetSelectResume_Pause(false);
        SetSelectRestart_Pause(false);
        SetSelectOptions_Pause(false);
        SetSelectExit_Pause(false);

        switch (_pauseSelectMenu)
        {
            case 0:
                SetSelectResume_Pause(true);
                break;
            case 1:
                SetSelectRestart_Pause(true);
                break;
            case 2:
                SetSelectOptions_Pause(true);
                break;
            case 3:
                SetSelectExit_Pause(true);
                break;
        }

        PlayPointerEnterAudio();
    }

    private void SetSelectResume_Pause(bool active)
    {
        _resume.SetActive(!active);
        _resumeSelect.SetActive(active);
    }

    private void SetSelectRestart_Pause(bool active)
    {
        _restart.SetActive(!active);
        _restartSelect.SetActive(active);
    }

    private void SetSelectOptions_Pause(bool active)
    {
        _options.SetActive(!active);
        _optionsSelect.SetActive(active);
    }

    private void SetSelectExit_Pause(bool active)
    {
        _exit.SetActive(!active);
        _exitSelect.SetActive(active);
    }

    public void ExcuteMenu_Pause(int menuIndex)
    {
        switch (menuIndex)
        {
            case 0:
                ExcuteResume_Pause();
                break;
            case 1:
                ExcuteRestart_Pause();
                break;
            case 2:
                ExcuteOption_Pause();
                break;
            case 3:
                ExcuteExit_Pause();
                break;
        }

        PlayPointerEnterAudio();
    }

    private void ExcuteResume_Pause()
    {
        SetActivePause(false);
    }

    private void ExcuteRestart_Pause()
    {
        SetActivePause(false, false);
        SetActiveRestart(true);
    }

    private void ExcuteOption_Pause()
    {

    }

    private void ExcuteExit_Pause()
    {
        SetActivePause(false , false);
        SetActiveReturnToTitle(true);
    }

    #endregion

    #region ReturnToTitle

    [SerializeField]
    private GameObject _returnToTitleGroup = null;

    [SerializeField]
    private GameObject _yes_ReturnToTitle = null, _yesSelect_ReturnToTitle = null;
    [SerializeField]
    private GameObject _no_ReturnToTitle = null, _noSelect_ReturnToTitle = null;

    private int _returnToTitleSelectMenu = 0;

    private bool _isInitReturnToTitle = false;

    public void SetActiveReturnToTitle(bool active)
    {
        _isInitReturnToTitle = false;
        _returnToTitleGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else
            Invoke("PlayerCanOperationTrue", Time.deltaTime);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitReturnToTitle();
    }

    private void InitReturnToTitle()
    {
        SetSelectMenu_ReturnToTitle(0);

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Idle);
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Idle);
    }

    private void ReturnToTitleLogic()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SetSelectMenu_ReturnToTitle((_returnToTitleSelectMenu + 1) % 2);

        if (_isInitReturnToTitle && (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) || Input.GetKeyDown(KeyCode.Return)))
            ExcuteMenu_ReturnToTitle(_returnToTitleSelectMenu);
        else if (!_isInitReturnToTitle)
            _isInitReturnToTitle = true;
    }

    public void SetSelectMenu_ReturnToTitle(int menuIndex)
    {
        _returnToTitleSelectMenu = menuIndex;

        if (_returnToTitleSelectMenu == 0)
        {
            SetSelectYes_ReturnToTitle(true);
            SetSelectNo_ReturnToTitle(false);
        }
        else
        {
            SetSelectYes_ReturnToTitle(false);
            SetSelectNo_ReturnToTitle(true);
        }

        PlayPointerEnterAudio();
    }

    private void SetSelectYes_ReturnToTitle(bool active)
    {
        _yes_ReturnToTitle.SetActive(!active);
        _yesSelect_ReturnToTitle.SetActive(active);
    }

    private void SetSelectNo_ReturnToTitle(bool active)
    {
        _no_ReturnToTitle.SetActive(!active);
        _noSelect_ReturnToTitle.SetActive(active);
    }

    public void ExcuteMenu_ReturnToTitle(int menuIndex)
    {
        if (menuIndex == 0)
            ExcuteYes_ReturnToTitle();
        else
            ExcuteNo_ReturnToTitle();

        PlayPointerEnterAudio();
    }

    private void ExcuteYes_ReturnToTitle()
    {
        string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
        SceneManager.LoadScene("StageSelect_" + season);
    }

    private void ExcuteNo_ReturnToTitle()
    {
        SetActiveReturnToTitle(false);
    }

    #endregion

    #region Restart

    [SerializeField]
    private GameObject _restartGroup = null;

    [SerializeField]
    private GameObject _yes_Restart = null, _yesSelect_Restart = null;
    [SerializeField]
    private GameObject _no_Restart = null, _noSelect_Restart = null;

    private int _RestartSelectMenu = 0;

    private bool _isInitRestart = false;

    public void SetActiveRestart(bool active)
    {
        _isInitRestart = false;
        _restartGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else
            Invoke("PlayerCanOperationTrue", Time.deltaTime);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitRestart();
    }

    private void InitRestart()
    {
        SetSelectMenu_Restart(0);

        CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Idle);
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.Idle);
    }

    private void RestartLogic()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SetSelectMenu_Restart((_RestartSelectMenu + 1) % 2);

        if (_isInitRestart && (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) || Input.GetKeyDown(KeyCode.Return)))
            ExcuteMenu_Restart(_RestartSelectMenu);
        else if (!_isInitRestart)
            _isInitRestart = true;
    }

    public void SetSelectMenu_Restart(int menuIndex)
    {
        _RestartSelectMenu = menuIndex;

        if (_RestartSelectMenu == 0)
        {
            SetSelectYes_Restart(true);
            SetSelectNo_Restart(false);
        }
        else
        {
            SetSelectYes_Restart(false);
            SetSelectNo_Restart(true);
        }

        PlayPointerEnterAudio();
    }

    private void SetSelectYes_Restart(bool active)
    {
        _yes_Restart.SetActive(!active);
        _yesSelect_Restart.SetActive(active);
    }

    private void SetSelectNo_Restart(bool active)
    {
        _no_Restart.SetActive(!active);
        _noSelect_Restart.SetActive(active);
    }

    public void ExcuteMenu_Restart(int menuIndex)
    {
        if (menuIndex == 0)
            ExcuteYes_Restart();
        else
            ExcuteNo_Restart();

        PlayPointerEnterAudio();
    }

    private void ExcuteYes_Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void ExcuteNo_Restart()
    {
        SetActiveRestart(false);
    }

    #endregion

    private void PlayerCanOperationFalse()
    {
        CPlayerManager.Instance.IsCanOperation = false;
    }

    private void PlayerCanOperationTrue()
    {
        CPlayerManager.Instance.IsCanOperation = true;
    }
}
