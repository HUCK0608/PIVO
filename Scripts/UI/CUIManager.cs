﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

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

    private void Start()
    {
        LoadOptionData();
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
            else if(_optionGroup.activeSelf)
            {
                CancelOption();
                return;
            }

            if (!_pauseGroup.activeSelf)
                SetActivePause(true);
            else
                SetActivePause(false);
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            if (_returnToTitleGroup.activeSelf)
            {
                SetActiveReturnToTitle(false);
                return;
            }
            else if (_restartGroup.activeSelf)
            {
                SetActiveRestart(false);
                return;
            }
            else if (_optionGroup.activeSelf)
            {
                CancelOption();
                return;
            }

            if(_pauseGroup.activeSelf)
                SetActivePause(false);
        }

        if (_stageClear.activeSelf)
            StageClearLogic();
        else if (_pauseGroup.activeSelf)
            PauseLogic();
        else if (_returnToTitleGroup.activeSelf)
            ReturnToTitleLogic();
        else if (_restartGroup.activeSelf)
            RestartLogic();
        else if (_optionGroup.activeSelf)
            OptionLogic();
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
    private Design_ClearCorgiCamera _ClearCorgiCamera = null;

    [SerializeField]
    private GameObject[] _grayStars = null;
    [SerializeField]
    private GameObject[] _yellowStars = null;

    [SerializeField]
    private GameObject[] _grayLine = null;
    [SerializeField]
    private GameObject[] _yellowLine = null;

    [SerializeField]
    private Text biscuitCount = null;

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
        _ClearCorgiCamera.SetFinishAnim();
    }

    private void SetStar()
    {
        biscuitCount.text = "x " + CBiscuitManager.Instance.GetCurrentBiscuitCount().ToString();

        for (int i = 0; i < CBiscuitManager.Instance.GetCurrentStar(); i++)
            SetActiveYellowStar(i, true);
    }

    private void SetActiveYellowStar(int index, bool active)
    {
        _yellowStars[index].SetActive(active);
        _grayStars[index].SetActive(!active);

        if (index > 0)
        {
            _grayLine[index-1].SetActive(!active);
            _yellowLine[index-1].SetActive(active);
        }
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
        SetActivePause(false, false);
        SetActiveOption(true);
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

    #region Option

    /// <summary>옵션 메뉴 그룹</summary>
    [SerializeField]
    private GameObject _optionGroup = null;

    /// <summary>선택 옵션 메뉴 백 이미지</summary>
    [SerializeField]
    private Transform _selectOptionMenuBG = null;
    /// <summary>현재 옵션 선택 메뉴</summary>
    private int _currentSelectMenu_OptionMenu = 0;

    private bool _isOptionInitialize = false;

    /// <summary>옵션 설정 저장</summary>
    private void SaveOptionData()
    {
        EXmlDocumentNames documentName = EXmlDocumentNames.Setting;
        string nodePath = documentName.ToString("G");
        string[] elementsName = new string[] { "WindowMode", "Resolution", "BGM", "SFX" };
        string[] datas = new string[] { ((int)_currentWindowMode).ToString(),
                                        _currentResolution.ToString(),
                                        _currentBGMNormalizedValue.ToString(),
                                        _currentSFXNormalizedValue.ToString() };

        CDataManager.WritingDatas(EXmlDocumentNames.Setting, nodePath, elementsName, datas, true);
        CDataManager.SaveCurrentXmlDocument(true);
    }

    /// <summary>옵션 설정 로드</summary>
    private void LoadOptionData()
    {
        EXmlDocumentNames documentName = EXmlDocumentNames.Setting;
        string nodePath = documentName.ToString("G");
        string[] elementsName = new string[] { "WindowMode", "Resolution", "BGM", "SFX" };

        string[] datas = CDataManager.ReadDatas(documentName, nodePath, elementsName);

        if (datas == null)
            return;

        _currentWindowMode = (EWindowMode)int.Parse(datas[0]);
        _selectWindowMode = _currentWindowMode;
        _currentResolution = int.Parse(datas[1]);
        _selectResolution = _currentResolution;
        _currentBGMNormalizedValue = float.Parse(datas[2]);
        _selectBGMNormalizedValue = _currentBGMNormalizedValue;
        _currentSFXNormalizedValue = float.Parse(datas[3]);
        _selectSFXNormalizedValue = _currentSFXNormalizedValue;

        UpdateOptionUI();

        ApplyOption();

        _isOptionInitialize = true;
    }

    /// <summary>옵션 메뉴 활성화</summary>
    private void SetActiveOption(bool active)
    {
        _optionGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else
            Invoke("PlayerCanOperationTrue", Time.deltaTime);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitOption();
    }

    private void InitOption()
    {
        // 선택 메뉴 초기화(맨 위)
        SetSelectOptionMenuBGPoint(0);

        LoadOptionData();
    }

    /// <summary>선택 옵션 메뉴 BG 위치 설정</summary>
    public void SetSelectOptionMenuBGPoint(int selectMenuValue)
    {
        if (_currentSelectMenu_OptionMenu.Equals(selectMenuValue))
            return;

        _currentSelectMenu_OptionMenu = selectMenuValue;

        if (3 >= _currentSelectMenu_OptionMenu)
        {
            _selectOptionMenuBG.gameObject.SetActive(true);
        }
        else
        {
            PlayPointerUpAudio();
            _selectOptionMenuBG.gameObject.SetActive(false);
            return;
        }

        Vector3 temp = Vector3.up * 70f;
        _selectOptionMenuBG.localPosition = -temp * _currentSelectMenu_OptionMenu + temp + Vector3.right * -65f;

        PlayPointerUpAudio();
    }

    /// <summary>옵션 업데이트</summary>
    private void UpdateOptionUI()
    {
        UpdateOptionUI_WindowMode();
        UpdateOptionUI_Resolution();
        UpdateOptionUI_BGM();
        UpdateOptionUI_SFX();
    }

    private void ChangeOption(float addValue)
    {
        switch (_currentSelectMenu_OptionMenu)
        {
            case 0:
                ChangeWindowMode((int)addValue);
                break;
            case 1:
                ChangeResolution((int)addValue);
                break;
            case 2:
                ChangeBGMVolume(addValue);
                break;
            case 3:
                ChangeSFXVolume(addValue);
                break;
        }
    }

    /// <summary>옵션 로직</summary>
    private void OptionLogic()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
        {
            if (_currentSelectMenu_OptionMenu == 4)
            {
                CancelOption();
            }
            else
            {
                ApplyOption();
            }
        }
        else if (_currentSelectMenu_OptionMenu == 4 || _currentSelectMenu_OptionMenu == 5)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _currentSelectMenu_OptionMenu = Mathf.Clamp(_currentSelectMenu_OptionMenu - 1, 4, 5);
                PlayPointerUpAudio();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _currentSelectMenu_OptionMenu = Mathf.Clamp(_currentSelectMenu_OptionMenu + 1, 4, 5);
                PlayPointerUpAudio();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                SetSelectOptionMenuBGPoint(3);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetSelectOptionMenuBGPoint(Mathf.Clamp(_currentSelectMenu_OptionMenu - 1, 0, 5));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetSelectOptionMenuBGPoint(Mathf.Clamp(_currentSelectMenu_OptionMenu + 1, 0, 5));
        }
        else if (_currentSelectMenu_OptionMenu == 0 || _currentSelectMenu_OptionMenu == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ChangeOption(-1f);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                ChangeOption(1f);
        }
        else if (_currentSelectMenu_OptionMenu == 2 || _currentSelectMenu_OptionMenu == 3)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                ChangeOption(-0.01f);
            else if (Input.GetKey(KeyCode.RightArrow))
                ChangeOption(0.01f);
        }
    }

    /// <summary>옵션 적용</summary>
    public void ApplyOption()
    {
        ApplyOption_WindowModeWithResolution();
        ApplyOption_Sound();
        PlayPointerEnterAudio();

        if (_isOptionInitialize)
        {
            SaveOptionData();
        }
    }

    /// <summary>윈도우 모드 및 해상도 변경</summary>
    private void ApplyOption_WindowModeWithResolution()
    {
        _currentWindowMode = _selectWindowMode;
        _currentResolution = _selectResolution;

        if (_currentWindowMode.Equals(EWindowMode.FullScreen))
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.ExclusiveFullScreen);
        else if (_currentWindowMode.Equals(EWindowMode.FullScreenWindow))
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.FullScreenWindow);
        else
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.Windowed);
    }

    /// <summary>사운드 적용</summary>
    private void ApplyOption_Sound()
    {
        _currentBGMNormalizedValue = _selectBGMNormalizedValue;
        _currentSFXNormalizedValue = _selectSFXNormalizedValue;

        string volume = "Volume";
        _BGMAudioMixer.SetFloat(volume, 80f * _currentBGMNormalizedValue - 80f);
        _SFXAudioMixer.SetFloat(volume, 80f * _currentSFXNormalizedValue - 80f);
    }

    /// <summary>옵션 적용 취소</summary>
    public void CancelOption()
    {
        CancelWindowMode();
        CancelResolution();
        CancelSound();

        UpdateOptionUI();

        PlayPointerEnterAudio();

        SetActiveOption(false);
    }

    /// <summary>윈도우 모드 설정 취소</summary>
    private void CancelWindowMode() { _selectWindowMode = _currentWindowMode; }

    /// <summary>해상도 설정 취소</summary>
    private void CancelResolution() { _selectResolution = _currentResolution; }

    /// <summary>사운드 설정 취소</summary>
    private void CancelSound()
    {
        _selectBGMNormalizedValue = _currentBGMNormalizedValue;
        _selectSFXNormalizedValue = _currentSFXNormalizedValue;
    }


    ///////////////////////////////////////// Window Mode
    /// <summary>윈도우 모드 Enum</summary>
    private enum EWindowMode { FullScreen = 0, FullScreenWindow = 1, Windowed };
    /// <summary>현재 윈도우 모드</summary>
    private EWindowMode _currentWindowMode = EWindowMode.FullScreen;
    /// <summary>선택 윈도우 모드</summary>
    private EWindowMode _selectWindowMode = EWindowMode.FullScreen;

    /// <summary>윈도우 모드 값 텍스트</summary>
    [SerializeField]
    private Text _windowModeValueText = null;

    /// <summary>
    /// 윈도우 모드 설정
    /// </summary>
    /// <param name="addValue">추가 값</param>
    public void ChangeWindowMode(int addValue)
    {
        _selectWindowMode += addValue;

        if (((int)_selectWindowMode).Equals(-1))
            _selectWindowMode = (EWindowMode)(System.Enum.GetValues(typeof(EWindowMode)).Length - 1);
        else if (((int)_selectWindowMode).Equals(System.Enum.GetValues(typeof(EWindowMode)).Length))
            _selectWindowMode = 0;

        UpdateOptionUI_WindowMode();

        PlayPointerUpAudio();
    }

    /// <summary>윈도우 모드 UI 업데이트</summary>
    private void UpdateOptionUI_WindowMode()
    {
        UpdateWindowModeText();
    }

    /// <summary>윈도우 모드 텍스트 업데이트</summary>
    private void UpdateWindowModeText() { _windowModeValueText.text = _selectWindowMode.ToString("G"); }


    ///////////////////////////////////////// Resoultion
    /// <summary>현재 해상도</summary>
    private int _currentResolution = 0;
    /// <summary>선택 해상도</summary>
    private int _selectResolution = 0;

    /// <summary>해상도</summary>
    [SerializeField]
    private List<int> _resolution1 = null, _resolution2 = null;

    /// <summary>해상도 값 텍스트</summary>
    [SerializeField]
    private Text _resolutionValueText = null;

    /// <summary>
    /// 해상도 설정
    /// </summary>
    /// <param name="addValue">추가 값</param>
    public void ChangeResolution(int addValue)
    {
        _selectResolution -= addValue;

        if (_selectResolution.Equals(-1))
            _selectResolution = _resolution1.Count - 1;
        else if (_selectResolution.Equals(_resolution1.Count))
            _selectResolution = 0;

        UpdateOptionUI_Resolution();

        PlayPointerUpAudio();
    }

    /// <summary>해상도 UI 업데이트</summary>
    private void UpdateOptionUI_Resolution()
    {
        UpdateResolutionText();
    }

    /// <summary>해상도 텍스트 업데이트</summary>
    private void UpdateResolutionText() { _resolutionValueText.text = _resolution1[_selectResolution].ToString() + " * " + _resolution2[_selectResolution].ToString(); }


    ///////////////////////////////////////// Sound
    /// <summary>BGM 오디오 믹서</summary>
    [SerializeField]
    private AudioMixer _BGMAudioMixer = null;
    /// <summary>BGM 스크롤</summary>
    [SerializeField]
    private CScroll _BGMScroll = null;

    /// <summary>SFX 오디오 믹서</summary>
    [SerializeField]
    private AudioMixer _SFXAudioMixer = null;
    /// <summary>SFX 스크롤</summary>
    [SerializeField]
    private CScroll _SFXScroll = null;

    /// <summary>현재 BGM 노말 값</summary>
    private float _currentBGMNormalizedValue = 1f;
    /// <summary>선택 BGM 노말 값</summary>
    private float _selectBGMNormalizedValue = 1f;

    /// <summary>현재 SFX 노말 값</summary>
    private float _currentSFXNormalizedValue = 1f;
    /// <summary>선택 SFX 노말 값</summary>
    private float _selectSFXNormalizedValue = 1f;

    /// <summary>BGM UI 업데이트</summary>
    private void UpdateOptionUI_BGM() { _BGMScroll.SetScroll(_currentBGMNormalizedValue); }
    /// <summary>SFX UI 업데이트</summary>
    private void UpdateOptionUI_SFX() { _SFXScroll.SetScroll(_currentSFXNormalizedValue); }

    /// <summary>BGM 볼륨 설정</summary>
    public void ChangeBGMVolume() { _selectBGMNormalizedValue = _BGMScroll.NormalizedValue; }
    /// <summary>SFX 볼륨 설정</summary>
    public void ChangeSFXVolume() { _selectSFXNormalizedValue = _SFXScroll.NormalizedValue; }
    public void ChangeBGMVolume(float addValue) { _selectBGMNormalizedValue = Mathf.Clamp(_selectBGMNormalizedValue + addValue, 0f, 1f); _BGMScroll.SetScroll(_selectBGMNormalizedValue); }
    public void ChangeSFXVolume(float addValue) { _selectSFXNormalizedValue = Mathf.Clamp(_selectSFXNormalizedValue + addValue, 0f, 1f); _SFXScroll.SetScroll(_selectSFXNormalizedValue); }

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
