﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Audio;

public enum EPaintingType { Snow = 0, Temple };
public enum eTutorialType { Move2D, Move3D, Climb, ViewChange }

public class CUIManager : MonoBehaviour
{
    private static CUIManager _instance = null;
    public static CUIManager Instance { get { return _instance; } }

    private bool _isCanOperation = true;
    public bool IsCanOperation { set { _isCanOperation = value; } }

    private void Awake()
    {
        _instance = this;

        _interactionKeyText.text = CKeyManager.InteractionKey.ToString("G");
    }

    private void Start()
    {
        LoadOptionData();
        _isOptionInitialize = true;

        if (!CUIManager_Title._isUseTitle)
        {
            PlayerCanOperationFalse();
            StartFadeIn(PlayerCanOperationTrue);
        }
    }

    private void Update()
    {
        if(_isFadeInOrOut)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_returnToTitleGroup.activeSelf)
            {
                SetActiveReturnToTitle(false);
                _mouseClickPlayer.Play();
                return;
            }
            else if (_restartGroup.activeSelf)
            {
                SetActiveRestart(false);
                _mouseClickPlayer.Play();
                return;
            }
            else if (_optionGroup.activeSelf)
            {
                CancelOption();
                _mouseClickPlayer.Play();
                return;
            }

            if (!_pauseGroup.activeSelf)
                SetActivePause(true);
            else
            {
                SetActivePause(false);
                _mouseClickPlayer.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            if (_returnToTitleGroup.activeSelf)
            {
                SetActiveReturnToTitle(false);
                _mouseClickPlayer.Play();
                return;
            }
            else if (_restartGroup.activeSelf)
            {
                SetActiveRestart(false);
                _mouseClickPlayer.Play();
                return;
            }
            else if (_optionGroup.activeSelf)
            {
                CancelOption();
                _mouseClickPlayer.Play();
                return;
            }

            if (_pauseGroup.activeSelf)
            {
                SetActivePause(false);
                _mouseClickPlayer.Play();
            }
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
    private bool _isOnInGameUI = true;

    public void SetActiveInGameUI(bool active)
    {
        _inGameUI.SetActive(active);
        _isOnInGameUI = active;
    }

    [SerializeField]
    private GameObject _hpGroup = null;
    /// <summary>체력 애니메이션 리스트</summary>
    [SerializeField]
    private List<Animation> _hpAnimations = null;

    private bool _isOnHpUI = true;

    private void SetActiveHpUI(bool active)
    {
        _isOnHpUI = active;
        _hpGroup.SetActive(active);
    }

    /// <summary>
    /// 체력 UI 설정
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    public void SetHpUI(int currentHp)
    {
        _hpAnimations[currentHp].Play();
    }

    [SerializeField]
    private GameObject _biscuitGroup = null;
    /// <summary>비스킷 개수 텍스트</summary>
    [SerializeField]
    private Text _biscuitCountText = null;

    bool _isOnBiscuitUI = true;

    private void SetActiveBiscuitUI(bool active)
    {
        _isOnBiscuitUI = active;
        _biscuitGroup.SetActive(active);
    }

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
    private bool _isOnInteractionUI = false;

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
            _isOnInteractionUI = false;
        else if (!_interactionGroup.activeSelf)
            _isOnInteractionUI = true;

        // 조건
        if (true == _isOnInteractionUI)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnHoldingUI) return;
            if (true == _isOnOptionUI) return;
            if (true == _isOnPauseUI) return;
            if (true == _isOnRestartUI) return;
            if (true == _isOnReturnToTitleUI) return;
            if (true == _isOnStageClearUI) return;
            if (true == _isOnWallPaintingUI) return;
        }

        _interactionGroup.SetActive(_isOnInteractionUI);
    }

    private void SetForceActiveInteractionUI(bool value)
    {
        if (true == value && true == _isOnInteractionUI)
            _interactionGroup.SetActive(value);
        else if (false == value)
            _interactionGroup.SetActive(value);
    }

    [SerializeField]
    private GameObject _bombExplosionGroup = null;
    private bool _isOnBombExplosionUI = false;

    public void SetActiveBombExplosionUI(bool value)
    {
        _isOnBombExplosionUI = value;

        if (true == _isOnBombExplosionUI)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnHoldingUI) return;
            if (true == _isOnOptionUI) return;
            if (true == _isOnPauseUI) return;
            if (true == _isOnRestartUI) return;
            if (true == _isOnReturnToTitleUI) return;
            if (true == _isOnStageClearUI) return;
            if (true == _isOnWallPaintingUI) return;
            if (true == _isOnMove3DTutorialUI) return;
        }

        _bombExplosionGroup.SetActive(value);
    }

    private void SetForceActiveBombExplosionUI(bool value)
    {
        if (true == value && true == _isOnBombExplosionUI)
            _bombExplosionGroup.SetActive(value);
        else if (false == value)
            _bombExplosionGroup.SetActive(value);
    }

    [SerializeField]
    private GameObject _holdingGroup = null;
    private bool _isOnHoldingUI = false;

    public void SetActiveHoldingUI(bool value)
    {
        _isOnHoldingUI = value;

        if (true == _isOnHoldingUI)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnOptionUI) return;
            if (true == _isOnPauseUI) return;
            if (true == _isOnRestartUI) return;
            if (true == _isOnReturnToTitleUI) return;
            if (true == _isOnStageClearUI) return;
            if (true == _isOnWallPaintingUI) return;

            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveMove2DTutorialUI(false);
            SetForceActiveMove3DTutorialUI(false);
            SetForceActiveClimbTutorialUI(false);
            SetForceActiveViewChangeTutorialUI(false);
        }
        else
        {
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveMove2DTutorialUI(true);
            SetForceActiveMove3DTutorialUI(true);
            SetForceActiveClimbTutorialUI(true);
            SetForceActiveViewChangeTutorialUI(true);
        }

        _holdingGroup.SetActive(value);
    }

    private void SetForceActiveHoldingUI(bool value)
    {
        if (true == value && true == _isOnHoldingUI)
            _holdingGroup.SetActive(value);
        else if (false == value)
            _holdingGroup.SetActive(value);
    }

    #endregion

    #region Tutorial

    [SerializeField]
    private GameObject _move2DTutorialGroup = null;
    [SerializeField]
    private GameObject _move3DTutorialGroup = null;
    [SerializeField]
    private GameObject _climbTutorialGroup = null;
    [SerializeField]
    private GameObject _viewChangeTutorialGroup = null;

    private bool _isOnMove2DTutorialUI = false;
    private bool _isOnMove3DTutorialUI = false;
    private bool _isOnClimbTutorialUI = false;
    private bool _isOnViewChangeTutorialUI = false;

    public void SetActiveMove2DTutorialUI(bool value)
    {
        _isOnMove2DTutorialUI = value;
        _move2DTutorialGroup.SetActive(value);
    }

    private void SetForceActiveMove2DTutorialUI(bool value)
    {
        if (true == value && true == _isOnMove2DTutorialUI)
            _move2DTutorialGroup.SetActive(true);
        else if (false == value)
            _move2DTutorialGroup.SetActive(false);
    }

    public void SetActiveMove3DTutorialUI(bool value)
    {
        _isOnMove3DTutorialUI = value;

        if(true == _isOnMove3DTutorialUI)
        {
            if (_isOnWallPaintingUI) return;
            if (_isOnPauseUI) return;
            if (_isOnRestartUI) return;
            if (_isOnOptionUI) return;
            if (_isOnReturnToTitleUI) return;
            if (_isOnDeadUI) return;

            SetForceActiveBombExplosionUI(false);
        }
        else
        {
            SetForceActiveBombExplosionUI(true);
        }

        _isOnMove3DTutorialUI = value;
        _move3DTutorialGroup.SetActive(value);
    }

    private void SetForceActiveMove3DTutorialUI(bool value)
    {
        if (true == value && true == _isOnMove3DTutorialUI)
            _move3DTutorialGroup.SetActive(true);
        else if (false == value)
            _move3DTutorialGroup.SetActive(false);
    }

    public void SetActiveClimbTutorialUI(bool value)
    {
        _isOnClimbTutorialUI = value;
        _climbTutorialGroup.SetActive(value);
    }

    private void SetForceActiveClimbTutorialUI(bool value)
    {
        if (true == value && true == _isOnClimbTutorialUI)
            _climbTutorialGroup.SetActive(true);
        else if (false == value)
            _climbTutorialGroup.SetActive(false);
    }

    public void SetActiveViewChangeTutorialUI(bool value)
    {
        _isOnViewChangeTutorialUI = value;
        _viewChangeTutorialGroup.SetActive(value);
    }

    private void SetForceActiveViewChangeTutorialUI(bool value)
    {
        if (true == value && true == _isOnViewChangeTutorialUI)
            _viewChangeTutorialGroup.SetActive(value);
        else if (false == value)
            _viewChangeTutorialGroup.SetActive(value);
    }

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
    private bool _isOnDeadUI = false;

    public void ActiveDeadUI()
    {
        if (true == _isOnStageClearUI) return;

        _isOnDeadUI = true;

        SetForceActiveInteractionUI(false);
        SetForceActiveBombExplosionUI(false);
        SetForceActiveHoldingUI(false);
        SetForceActiveMove2DTutorialUI(false);
        SetForceActiveMove3DTutorialUI(false);
        SetForceActiveClimbTutorialUI(false);
        SetForceActiveViewChangeTutorialUI(false);
        SetActiveWallPaintingUI(false);
        SetActivePause(false, false);
        SetActiveReturnToTitle(false);
        SetActiveRestart(false);
        CancelOption();

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

        _mouseEnterPlayer.Play();

        _retryImage.sprite = _glowRetrySprite;
        _stageSelectImage.sprite = _defaultStageSelectSprite;
    }

    /// <summary>스테이지 선택 포인터 엔터 이벤트</summary>
    public void StageSelectPointEnter()
    {
        _isSelectRetry = false;

        _mouseEnterPlayer.Play();

        _retryImage.sprite = _defaultRetrySprite;
        _stageSelectImage.sprite = _glowStageSelectSprite;
    }

    /// <summary>재시작 클릭 이벤트</summary>
    public void RetryPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        _mouseClickPlayer.Play();

        System.Action callback = delegate ()
        {
            SoundManager.Instance.StopAll();
            SetActiveLoading(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        };

        CUIManager_Title._isUseTitle = false;
        StartFadeOut(callback);
    }

    /// <summary>스테이지 선택 클릭 이벤트</summary>
    public void StageSelectPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        _mouseClickPlayer.Play();

        System.Action callback = delegate ()
        {
            SoundManager.Instance.StopAll();
            SetActiveLoading(true);
            string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
            SceneManager.LoadScene("StageSelect_" + season);
        };

        StartFadeOut(callback);
    }

    #endregion

    #region Sound
    [SerializeField]
    private SoundRandomPlayer_SFX _mouseEnterPlayer = null;
    [SerializeField]
    private SoundRandomPlayer_SFX _mouseClickPlayer = null;
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

    private bool _isOnWallPaintingUI = false;
    /// <summary>벽화가 켜져있는지 여부</summary>
    public bool IsOnWallPaintingUI { get { return _isOnWallPaintingUI; } }

    /// <summary>
    /// 페인팅 활성화 설정
    /// </summary>
    /// <param name="paintingType">페인팅 타입(겨울, 신전)</param>
    /// <param name="index">페인팅 인덱스</param>
    /// <param name="active">활성화 여부</param>
    public void SetActivePainting(bool active, EPaintingType paintingType = EPaintingType.Snow, int index = 0)
    {
        if (true == active)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnStageClearUI) return;
            if (true == _isOnPauseUI) return;
            if (true == _isOnReturnToTitleUI) return;
            if (true == _isOnRestartUI) return;
            if (true == _isOnOptionUI) return;

            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
        }
        else
        {
            SetActiveHpUI(true);
            SetActiveBiscuitUI(true);
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveHoldingUI(true);
        }

        CPlayerManager.Instance.IsCanOperation = !active;

        index -= 1;

        if (!active)
        {
            SetActiveWallPaintingUI(active);
        }
        else
        {
            ActivatePainting(paintingType, index);
            ActivatePaintingType(paintingType);
            SetActiveWallPaintingUI(true);
            CPlayerManager.Instance.Controller3D.ChangeState(EPlayerState3D.Idle);
        }
    }

    private void SetActiveWallPaintingUI(bool active)
    {
        _isOnWallPaintingUI = active;
        CCameraController.Instance.SetActivateBlur(active);
        _goWallPaintingGroup.SetActive(active);
    }

    private void ActivatePainting(EPaintingType paintingType, int index)
    {
        if (paintingType.Equals(EPaintingType.Snow))
        {
            for (int i = 0; i < _goArrSnowPainting.Count; i++)
                SetActiveSnowPainting(i, false);

            SetActiveSnowPainting(index, true);
        }
        else if (paintingType.Equals(EPaintingType.Temple))
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
        _mouseClickPlayer.Play();
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

    [SerializeField]
    private Design_ClearCorgiCamera _clearCorgiCam = null;

    [SerializeField]
    private Text _curBiscuitWhenFinished = null;

    [SerializeField]
    private GameObject[] _grayLines = null;
    [SerializeField]
    private GameObject[] _yellowLines = null;

    private bool _isOnStageClearUI = false;

    private bool _isExcuteStageClear = false;

    private void StageClearLogic()
    {
        if (!_isExcuteStageClear && (Input.GetKeyDown(CKeyManager.ViewChangeExecutionKey) || Input.GetKeyDown(KeyCode.Return)))
            UIStageClearOkButtonClick();
    }

    public void SetActiveStageClear(bool active)
    {
        if (true == active)
        {
            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
            SetActiveWallPaintingUI(false);
            SetActivePause(false, false);
            SetActiveReturnToTitle(false);
            SetActiveRestart(false);
            CancelOption(false);
        }

        _isOnStageClearUI = active;

        if (active)
        {
            SetStar();
            SetStarLine();
            SetStageClearRequirementText();
            SetCurBiscuitCountText();
        }

        SetTargetDisplay(0);

        _stageClear.SetActive(active);

        gameObject.SetActive(false);
        gameObject.SetActive(true);

        _clearCorgiCam.SetFinishAnim();
    }



    private void SetStar()
    {
        int currentStar = CBiscuitManager.Instance.GetCurrentStar();
        for (int i = 0; i < currentStar; i++)
            SetActiveYellowStar(i, true);

        if (1 == currentStar)
            SoundManager.Instance.PlaySFX(ESFXType.StageClear_0);
        else if(2 == currentStar)
            SoundManager.Instance.PlaySFX(ESFXType.StageClear_1);
        else if(3 == currentStar)
            SoundManager.Instance.PlaySFX(ESFXType.StageClear_2);
    }

    private void SetActiveYellowStar(int index, bool active)
    {
        _yellowStars[index].SetActive(active);
        _grayStars[index].SetActive(!active);
    }

    private void SetStarLine()
    {
        foreach (var v in _yellowLines)
            v.SetActive(false);

        foreach (var v in _grayLines)
            v.SetActive(true);

        for (int i = 0; i < CBiscuitManager.Instance.GetCurrentStar(); i++)
        {
            if (i > 0)
            {
                _yellowLines[i-1].SetActive(true);
                _grayLines[i-1].SetActive(false);
            }
        }
    }

    private void SetCurBiscuitCountText()
    {
        _curBiscuitWhenFinished.text = "x " + CBiscuitManager.Instance.CurrentBiscuitCount;
    }

    private void SetStageClearRequirementText()
    {
        int[] requirement = CBiscuitManager.Instance.Requirements;

        for (int i = 0; i < 3; i++)
            _stageClearRequirementTexts[i].text = string.Format("{0}", requirement[i].ToString());
    }

    public void UIStageClearOkButtonClick()
    {
        _mouseClickPlayer.Play();
        _isExcuteStageClear = true;

        if (CWorldManager.Instance.IsUseTimeLineScene)
        {
            SoundManager.Instance.StopAll();
            SetActiveLoading(true);
            CWorldManager.Instance.StageClearWaitTimeLineScene(CWorldManager.Instance.TimeLineSceneName);
        }
        else
        {
            SoundManager.Instance.StopAll();
            SetActiveLoading(true);
            CWorldManager.Instance.StageClear();
        }
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
    private bool _isOnPauseUI = false;

    public void SetActivePause(bool active, bool isSetPlayerOperation = true)
    {
        if (true == active)
        {
            if (false == _isCanOperation) return;
            if (true == _isOnDeadUI) return;
            if (true == _isOnStageClearUI) return;

            Time.timeScale = 0;
            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
            SetForceActiveMove2DTutorialUI(false);
            SetForceActiveMove3DTutorialUI(false);
            SetForceActiveClimbTutorialUI(false);
            SetForceActiveViewChangeTutorialUI(false);
            SetActiveWallPaintingUI(false);
        }
        else
        {
            Time.timeScale = 1;
            SetActiveHpUI(true);
            SetActiveBiscuitUI(true);
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveHoldingUI(true);
            SetForceActiveMove2DTutorialUI(true);
            SetForceActiveMove3DTutorialUI(true);
            SetForceActiveClimbTutorialUI(true);
            SetForceActiveViewChangeTutorialUI(true);
        }

        _isInitPause = false;
        _isOnPauseUI = active;
        _pauseGroup.SetActive(active);

        if (active && isSetPlayerOperation)
            PlayerCanOperationFalse();
        else if (!active && isSetPlayerOperation)
            Invoke("PlayerCanOperationTrue", 0.05f);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitPause();
    }

    private void InitPause()
    {
        SetSelectMenu_Pause(0);
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

        _mouseEnterPlayer.Play();
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

        _mouseClickPlayer.Play();
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
        SetActivePause(false, false);
        SetActiveReturnToTitle(true);
    }

    #endregion

    #region StageSelect

    [SerializeField]
    private GameObject _returnToTitleGroup = null;

    [SerializeField]
    private GameObject _yes_ReturnToTitle = null, _yesSelect_ReturnToTitle = null;
    [SerializeField]
    private GameObject _no_ReturnToTitle = null, _noSelect_ReturnToTitle = null;

    private int _returnToTitleSelectMenu = 0;

    private bool _isInitReturnToTitle = false;
    private bool _isOnReturnToTitleUI = false;

    public void SetActiveReturnToTitle(bool active)
    {
        if (true == active)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnStageClearUI) return;

            Time.timeScale = 0;
            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
            SetForceActiveMove2DTutorialUI(false);
            SetForceActiveMove3DTutorialUI(false);
            SetForceActiveClimbTutorialUI(false);
            SetForceActiveViewChangeTutorialUI(false);
            SetActiveWallPaintingUI(false);
        }
        else
        {
            Time.timeScale = 1;
            SetActiveHpUI(true);
            SetActiveBiscuitUI(true);
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveHoldingUI(true);
            SetForceActiveMove2DTutorialUI(true);
            SetForceActiveMove3DTutorialUI(true);
            SetForceActiveClimbTutorialUI(true);
            SetForceActiveViewChangeTutorialUI(true);
        }

        _isInitReturnToTitle = false;
        _isOnReturnToTitleUI = active;
        _returnToTitleGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else
            Invoke("PlayerCanOperationTrue", 0.05f);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitReturnToTitle();
    }

    private void InitReturnToTitle()
    {
        SetSelectMenu_ReturnToTitle(0);
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

        _mouseEnterPlayer.Play();
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

        _mouseClickPlayer.Play();
    }

    private void ExcuteYes_ReturnToTitle()
    {
        System.Action callback = delegate ()
        {
            SoundManager.Instance.StopAll();
            SetActiveLoading(true);
            string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
            SceneManager.LoadScene("StageSelect_" + season);
        };

        Time.timeScale = 1f;
        StartFadeOut(callback);
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
    private bool _isOnRestartUI = false;
    private bool _isExcuteEnd_Restart = false;

    public void SetActiveRestart(bool active)
    {
        if (true == active)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnStageClearUI) return;

            Time.timeScale = 0;
            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
            SetForceActiveMove2DTutorialUI(false);
            SetForceActiveMove3DTutorialUI(false);
            SetForceActiveClimbTutorialUI(false);
            SetForceActiveViewChangeTutorialUI(false);
            SetActiveWallPaintingUI(false);
        }
        else
        {
            Time.timeScale = 1;
            SetActiveHpUI(true);
            SetActiveBiscuitUI(true);
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveHoldingUI(true);
            SetForceActiveMove2DTutorialUI(true);
            SetForceActiveMove3DTutorialUI(true);
            SetForceActiveClimbTutorialUI(true);
            SetForceActiveViewChangeTutorialUI(true);
        }

        _isInitRestart = false;
        _isOnRestartUI = active;
        _restartGroup.SetActive(active);

        if (active)
            PlayerCanOperationFalse();
        else
            Invoke("PlayerCanOperationTrue", 0.05f);

        CCameraController.Instance.SetActivateBlur(active);

        if (active)
            InitRestart();
    }

    private void InitRestart()
    {
        SetSelectMenu_Restart(0);
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

        _mouseEnterPlayer.Play();
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
        if (_isExcuteEnd_Restart)
            return;
            
        _isExcuteEnd_Restart = true;

        if (menuIndex == 0)
        {
            Time.timeScale = 1f;
            ExcuteYes_Restart();
        }
        else
            ExcuteNo_Restart();

        _mouseClickPlayer.Play();
    }

    private void ExcuteYes_Restart()
    {
        System.Action callback = delegate ()
        {
            SoundManager.Instance.StopAll();
            CUIManager_Title._isUseTitle = false;
            SetActiveLoading(true);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        };

        StartFadeOut(callback);
    }

    private void ExcuteNo_Restart()
    {
        _isExcuteEnd_Restart = false;
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

    [SerializeField]
    private GameObject _applyInactive_OptionMenu = null, _applyActive_OptionMenu = null, _applyActiveSelect_OptionMenu = null;
    [SerializeField]
    private GameObject _cancel_OptionMenu = null, _cancelSelect_OptionMenu = null;

    private bool _isOptionInitialize = false;
    private bool _isChangeOption = false;
    private bool _isOnOptionUI = false;

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
        if (!CDataManager.IsHaveGameData(EXmlDocumentNames.Setting))
        {
            UpdateOptionUI();
            return;
        }

        EXmlDocumentNames documentName = EXmlDocumentNames.Setting;
        string nodePath = documentName.ToString("G");
        string[] elementsName = new string[] { "WindowMode", "Resolution", "BGM", "SFX" };

        string[] datas = CDataManager.ReadDatas(documentName, nodePath, elementsName);

        _currentWindowMode = (EWindowMode)int.Parse(datas[0]);
        _selectWindowMode = _currentWindowMode;
        _currentResolution = int.Parse(datas[1]);
        _selectResolution = _currentResolution;
        _currentBGMNormalizedValue = float.Parse(datas[2]);
        _selectBGMNormalizedValue = _currentBGMNormalizedValue;
        _currentSFXNormalizedValue = float.Parse(datas[3]);
        _selectSFXNormalizedValue = _currentSFXNormalizedValue;

        UpdateOptionUI();

        ApplyOption_Sound();
    }

    /// <summary>옵션 메뉴 활성화</summary>
    private void SetActiveOption(bool active)
    {
        if (true == active)
        {
            if (true == _isOnDeadUI) return;
            if (true == _isOnStageClearUI) return;

            Time.timeScale = 0;
            SetActiveHpUI(false);
            SetActiveBiscuitUI(false);
            SetForceActiveInteractionUI(false);
            SetForceActiveBombExplosionUI(false);
            SetForceActiveHoldingUI(false);
            SetForceActiveMove2DTutorialUI(false);
            SetForceActiveMove3DTutorialUI(false);
            SetForceActiveClimbTutorialUI(false);
            SetForceActiveViewChangeTutorialUI(false);
            SetActiveWallPaintingUI(false);
        }
        else
        {
            Time.timeScale = 1;
            SetActiveHpUI(true);
            SetActiveBiscuitUI(true);
            SetForceActiveInteractionUI(true);
            SetForceActiveBombExplosionUI(true);
            SetForceActiveHoldingUI(true);
            SetForceActiveMove2DTutorialUI(true);
            SetForceActiveMove3DTutorialUI(true);
            SetForceActiveClimbTutorialUI(true);
            SetForceActiveViewChangeTutorialUI(true);
        }

        _isOnOptionUI = false;
        _optionGroup.SetActive(active);

        if (active)
        {
            PlayerCanOperationFalse();
            InitButtonOptionMenu();
        }
        else
            Invoke("PlayerCanOperationTrue", 0.05f);

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

        // 바꾼게 없을 경우 Apply로 오면 Cancel로 넘김
        if(false == _isChangeOption && 5 == selectMenuValue)
        {
            _currentSelectMenu_OptionMenu = 4;
        }

        if (3 >= _currentSelectMenu_OptionMenu)
        {
            InitButtonOptionMenu();
            _selectOptionMenuBG.gameObject.SetActive(true);
            Vector3 temp = Vector3.up * 70f;
            _selectOptionMenuBG.localPosition = -temp * _currentSelectMenu_OptionMenu + temp + Vector3.right * -65f;
        }
        else
        {
            _selectOptionMenuBG.gameObject.SetActive(false);
            SetSelectButtonOptionMenu(_currentSelectMenu_OptionMenu);
        }

        _mouseEnterPlayer.Play();
    }

    public void InitButtonOptionMenu()
    {
        _applyInactive_OptionMenu.SetActive(false);
        _applyActive_OptionMenu.SetActive(false);
        _applyActiveSelect_OptionMenu.SetActive(false);
        _cancel_OptionMenu.SetActive(false);
        _cancelSelect_OptionMenu.SetActive(false);

        if(_isChangeOption)
            _applyActive_OptionMenu.SetActive(true);
        else
            _applyInactive_OptionMenu.SetActive(true);

        _cancel_OptionMenu.SetActive(true);
    }

    public void SetSelectButtonOptionMenu(int selectMenuValue)
    {
        if(4 == selectMenuValue)
        {
            if (_isChangeOption)
            {
                _applyActive_OptionMenu.SetActive(true);
                _applyActiveSelect_OptionMenu.SetActive(false);
            }
            _cancel_OptionMenu.SetActive(false);
            _cancelSelect_OptionMenu.SetActive(true);
        }
        else if(5 == selectMenuValue)
        {
            _applyActive_OptionMenu.SetActive(false);
            _applyActiveSelect_OptionMenu.SetActive(true);
            _cancel_OptionMenu.SetActive(true);
            _cancelSelect_OptionMenu.SetActive(false);
        }

        _mouseEnterPlayer.Play();
    }

    private void SetActiveApplyButtonOptionMenu(bool value)
    {
        if (_isChangeOption == value)
            return;

        _isChangeOption = value;

        _applyInactive_OptionMenu.SetActive(false);
        _applyActive_OptionMenu.SetActive(false);
        _applyActiveSelect_OptionMenu.SetActive(false);

        _applyActive_OptionMenu.SetActive(_isChangeOption);
        _applyInactive_OptionMenu.SetActive(!_isChangeOption);
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
            else if(_currentSelectMenu_OptionMenu == 5)
            {
                ApplyOption();
                SetSelectOptionMenuBGPoint(4);
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
                SetSelectOptionMenuBGPoint(Mathf.Clamp(_currentSelectMenu_OptionMenu - 1, true == _isChangeOption ? 4 : 5, 5));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                SetSelectOptionMenuBGPoint(Mathf.Clamp(_currentSelectMenu_OptionMenu + 1, true == _isChangeOption ? 4 : 5, 5));
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

        _mouseClickPlayer.Play();

        if (_isOptionInitialize)
        {
            SaveOptionData();
        }

        SetActiveApplyButtonOptionMenu(false);
    }

    /// <summary>윈도우 모드 및 해상도 변경</summary>
    private void ApplyOption_WindowModeWithResolution()
    {
        if (!IsChangeWindowModeOrResolution())
            return;

        _currentWindowMode = _selectWindowMode;
        _currentResolution = _selectResolution;

        if (_currentWindowMode.Equals(EWindowMode.FullScreen))
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.ExclusiveFullScreen);
        else if (_currentWindowMode.Equals(EWindowMode.FullScreenWindow))
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.FullScreenWindow);
        else
            Screen.SetResolution(_resolution1[_currentResolution], _resolution2[_currentResolution], FullScreenMode.Windowed);
    }

    private bool IsChangeWindowModeOrResolution()
    {
        return (_currentWindowMode != _selectWindowMode) || (_currentResolution != _selectResolution);
    }

    /// <summary>사운드 적용</summary>
    private void ApplyOption_Sound()
    {
        _currentBGMNormalizedValue = _selectBGMNormalizedValue;
        _currentSFXNormalizedValue = _selectSFXNormalizedValue;

        string volume = "Volume";
        _BGMAudioMixer.SetFloat(volume, (_currentBGMNormalizedValue - 1) * 40f);
        _SFXAudioMixer.SetFloat(volume, (_currentSFXNormalizedValue - 1) * 40f);
    }

    /// <summary>옵션 적용 취소</summary>
    public void CancelOption(bool _bUseSound = true)
    {
        CancelWindowMode();
        CancelResolution();
        CancelSound();

        UpdateOptionUI();

        if(_bUseSound)
            _mouseClickPlayer.Play();

        SetActiveApplyButtonOptionMenu(false);
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
    private enum EWindowMode { FullScreenWindow = 0, FullScreen = 1, Windowed };
    /// <summary>현재 윈도우 모드</summary>
    private EWindowMode _currentWindowMode = EWindowMode.FullScreenWindow;
    /// <summary>선택 윈도우 모드</summary>
    private EWindowMode _selectWindowMode = EWindowMode.FullScreenWindow;

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

        _mouseEnterPlayer.Play();
        SetActiveApplyButtonOptionMenu(true);
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

        _mouseEnterPlayer.Play();
        SetActiveApplyButtonOptionMenu(true);
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
    public void ChangeBGMVolume() { _selectBGMNormalizedValue = _BGMScroll.NormalizedValue; SetActiveApplyButtonOptionMenu(true); }
    /// <summary>SFX 볼륨 설정</summary>
    public void ChangeSFXVolume() { _selectSFXNormalizedValue = _SFXScroll.NormalizedValue; SetActiveApplyButtonOptionMenu(true); }
    public void ChangeBGMVolume(float addValue) { _selectBGMNormalizedValue = Mathf.Clamp(_selectBGMNormalizedValue + addValue, 0f, 1f); _BGMScroll.SetScroll(_selectBGMNormalizedValue); SetActiveApplyButtonOptionMenu(true); }
    public void ChangeSFXVolume(float addValue) { _selectSFXNormalizedValue = Mathf.Clamp(_selectSFXNormalizedValue + addValue, 0f, 1f); _SFXScroll.SetScroll(_selectSFXNormalizedValue); SetActiveApplyButtonOptionMenu(true); }

    #endregion

    #region Fade

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
    public void StartFadeIn(System.Action callback = null)
    {
        if(!_isFadeInOrOut)
            StartCoroutine(BlackBGIncreaseAlphaLogic(-_blackBGIncreaseValue, 1f, 0f, callback));
    }

    /// <summary>점점 어두워지며 화면이 안보임</summary>
    public void StartFadeOut(System.Action callback = null)
    {
        if(!_isFadeInOrOut)
            StartCoroutine(BlackBGIncreaseAlphaLogic(_blackBGIncreaseValue, 0f, 1f, callback));
    }

    /// <summary>검은 화면 값 증가(목표값까지)</summary>
    private IEnumerator BlackBGIncreaseAlphaLogic(float value, float startAlpha, float finalAlpha, System.Action callback = null)
    {
        _isFadeInOrOut = true;
        _blackBG.gameObject.SetActive(true);

        // 시작값 설정
        Color currentColor = Color.black;
        currentColor.a = startAlpha;
        _blackBG.color = currentColor;

        while (!currentColor.a.Equals(finalAlpha))
        {
            currentColor.a = Mathf.Clamp01(currentColor.a + value * Time.deltaTime);
            _blackBG.color = currentColor;

            yield return null;
        }

        if (null != callback)
            callback();

        _isFadeInOrOut = false;

        if(finalAlpha.Equals(0f))
            _blackBG.gameObject.SetActive(false);
    }

    #endregion

    #region Loading

    [SerializeField]
    private GameObject _loaindg = null;

    private bool _isOnLoadingUI = false;

    public void SetActiveLoading(bool _bActive)
    {
        _isOnLoadingUI = _bActive;
        _loaindg.SetActive(_bActive);
    }

    #endregion

    #region Util

    private void PlayerCanOperationFalse()
    {
        CPlayerManager.Instance.IsCanOperation = false;
    }

    private void PlayerCanOperationTrue()
    {
        CPlayerManager.Instance.IsCanOperation = true;
    }

    #endregion
}