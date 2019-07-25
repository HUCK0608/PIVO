using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CUIManager : MonoBehaviour
{
    private static CUIManager _instance = null;
    public static CUIManager Instance { get { return _instance; } }

    private void Awake()
    {
        _instance = this;

        _interactionKeyText.text = CKeyManager.InteractionKey.ToString("G");
    }

    /// <summary>캔버스</summary>
    [SerializeField]
    private Canvas _canvas = null;

    /// <summary>목표 디스플레이 설정</summary>
    public void SetTargetDisplay(int targetValue)
    {
        _canvas.targetDisplay = targetValue;
    }

    /// <summary>체력 이미지 리스트</summary>
    [SerializeField]
    private List<GameObject> _hpImages = null;

    /// <summary>
    /// 체력 UI 설정
    /// </summary>
    /// <param name="currentHp">현재 체력</param>
    public void SetHpUI(int currentHp)
    {
        _hpImages[currentHp].SetActive(false);
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
        _biscuitCountText.text = "X " + currentBiscuitCount.ToString();
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
    private GameObject _deadGroup = null;
    /// <summary>재시작 및 스테이지 선택 이미지</summary>
    [SerializeField]
    private Image _retryImage = null, _stageSelectImage = null;
    /// <summary>기본 및 빛나는 재시작 스프라이트</summary>
    [SerializeField]
    private Sprite _defaultRetrySprite = null, _glowRetrySprite = null;
    /// <summary>기본 및 빛나는 스테이지 선택 스프라이트</summary>
    [SerializeField]
    private Sprite _defaultStageSelectSprite, _glowStageSelectSprite = null;

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

        _retryImage.sprite = _glowRetrySprite;
        _stageSelectImage.sprite = _defaultStageSelectSprite;
    }

    /// <summary>스테이지 선택 포인터 엔터 이벤트</summary>
    public void StageSelectPointEnter()
    {
        _isSelectRetry = false;

        _retryImage.sprite = _defaultRetrySprite;
        _stageSelectImage.sprite = _glowStageSelectSprite;
    }

    /// <summary>재시작 클릭 이벤트</summary>
    public void RetryPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>스테이지 선택 클릭 이벤트</summary>
    public void StageSelectPointUp()
    {
        if (_isEndDeadUI)
            return;

        _isEndDeadUI = true;

        string season = SceneManager.GetActiveScene().name.Split('_')[0].Equals("GrassStage") ? "Grass" : "Snow";
        SceneManager.LoadScene("StageSelect_" + season);
    }
}
