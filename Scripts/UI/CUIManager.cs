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
            if (!_pauseGroup.activeSelf)
                SetActivePause(true);
            else
                SetActivePause(false);
        }

        if (_stageClear.activeSelf)
            StageClearLogic();

        if (_pauseGroup.activeSelf)
            PauseLogic();
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
    private GameObject _exit = null, _exitSelect = null;

    private int _pauseSelectMenu = 0;

    public void SetActivePause(bool active)
    {
        _pauseGroup.SetActive(active);

        CPlayerManager.Instance.IsCanOperation = !active;

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
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            SetSelectMenu_Pause((_pauseSelectMenu + 1) % 2);

        if (Input.GetKeyDown(CKeyManager.InteractionKey))
            ExcuteMenu_Pause(_pauseSelectMenu);
    }

    public void SetSelectMenu_Pause(int menuIndex)
    {
        _pauseSelectMenu = menuIndex;

        if(_pauseSelectMenu == 0)
        {
            SetSelectResume_Pause(true);
            SetSelectExit_Pause(false);
        }
        else
        {
            SetSelectResume_Pause(false);
            SetSelectExit_Pause(true);
        }

        PlayPointerEnterAudio();
    }

    private void SetSelectResume_Pause(bool active)
    {
        _resume.SetActive(!active);
        _resumeSelect.SetActive(active);
    }

    private void SetSelectExit_Pause(bool active)
    {
        _exit.SetActive(!active);
        _exitSelect.SetActive(active);
    }

    public void ExcuteMenu_Pause(int menuIndex)
    {
        if (menuIndex == 0)
            ExcuteResume_Pause();
        else
            ExcuteExit_Pause();

        PlayPointerEnterAudio();
    }

    private void ExcuteResume_Pause()
    {
        SetActivePause(false);
    }

    private void ExcuteExit_Pause()
    {
        string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
        SceneManager.LoadScene("StageSelect_" + season);
    }

    #endregion
}
