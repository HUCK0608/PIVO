﻿using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class CUIManager_Title : MonoBehaviour
{
    /// <summary>애니메이터</summary>
    private Animator _animator = null;

    /// <summary>기본 메뉴 및 글로우 메뉴</summary>
    [SerializeField]
    private GameObject[] _defaultMenu = null, _selectMenu = null;

    /// <summary>인트로 Playable Director</summary>
    [SerializeField]
    private PlayableDirector _introPlayerDirector = null;

    [SerializeField]
    private Image _loadGameImage = null;

    /// <summary>현재 선택하고 있는 메인 메뉴</summary>
    private int _mainCurrentSelect = 0;
    /// <summary>최대 메뉴 개수</summary>
    private int _maxMenuCount = 0;

    /// <summary>무언가를 실행하고 있는지 여부</summary>
    private bool _isExcutionAnything = false;

    /// <summary>데이터가 존재하는지 여부</summary>
    private bool _isHaveData = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _maxMenuCount = _defaultMenu.Length;
        _audioSource = GetComponent<AudioSource>();


        if (!CDataManager.IsHaveData())
        {
            _isHaveData = false;
            _loadGameImage.color = Color.black;
            CDataManager.IsSaveData = false;
        }
    }

    private void Start()
    {
        // 0 : true, 1 : false
        int isOnTitle = PlayerPrefs.GetInt("IsOnTitle");

        // 타이틀을 보여주지 않을 경우 비활성화
        if (isOnTitle.Equals(1))
        {
            PlayerPrefs.SetInt("IsOnTitle", 0);
            gameObject.SetActive(false);
            _introPlayerDirector.gameObject.SetActive(false);
            return;
        }

        // 플레이어 조작 막기
        CPlayerManager.Instance.IsCanOperation = false;

        // 메인카메라와 메인 UI의 목표 디스플레이 변경(화면에서 안보이게)
        CCameraController.Instance.SetTargetDisplay(1);
        CUIManager.Instance.SetTargetDisplay(1);

        // 모든 오브젝트를 2D 상태로 변경
        CWorldManager.Instance.AllObjectsCanChange2D();
        CWorldManager.Instance.ChangeWorld();

        StartCoroutine(MainMenuInputLogic());
    }

    /// <summary>메뉴 선택 로직</summary>
    private IEnumerator MainMenuInputLogic()
    {
        ChangeSelectMenu(_mainCurrentSelect);

        while(!_isExcutionAnything)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int nextSelect = Mathf.Clamp(_mainCurrentSelect - 1, 0, _maxMenuCount - 1);

                if (!_isHaveData && nextSelect.Equals(1))
                    nextSelect = 0;

                ChangeSelectMenu(nextSelect);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int nextSelect = Mathf.Clamp(_mainCurrentSelect + 1, 0, _maxMenuCount - 1);

                if (!_isHaveData && nextSelect.Equals(1))
                    nextSelect = 2;

                ChangeSelectMenu(nextSelect);
            }
            else if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Return))
                ExcutionMainMenu(_mainCurrentSelect);

            yield return null;
        }
    }

    /// <summary>선택 메뉴 변경</summary>
    public void ChangeSelectMenu(int selectMenu)
    {
        // 무언가 실행중이라면 리턴
        if (_isExcutionAnything)
            return;

        // 데이터가 없는데 로드게임을 선택하면 리턴
        if (!_isHaveData && selectMenu.Equals(1))
            return;

        PlayPointerEnterAudio();

        _defaultMenu[_mainCurrentSelect].SetActive(true);
        _selectMenu[_mainCurrentSelect].SetActive(false);
        _mainCurrentSelect = selectMenu;
        _defaultMenu[_mainCurrentSelect].SetActive(false);
        _selectMenu[_mainCurrentSelect].SetActive(true);
    }

    /// <summary>메인 메뉴 실행</summary>
    public void ExcutionMainMenu(int selectMenu)
    {
        // 무언가 실행중이라면 리턴
        if (_isExcutionAnything)
            return;

        // 데이터가 없는데 로드게임을 선택하면 리턴
        if (!_isHaveData && selectMenu == 1)
            return;

        _isExcutionAnything = true;

        PlayPointerUpAudio();

        switch (selectMenu)
        {
            case 0:
                _animator.SetBool("IsFadeOut", true);
                break;
            case 1:
                SceneManager.LoadScene("StageSelect_Grass");
                break;
            case 2:
                StartCoroutine(OptionMenuInputLogic());
                break;
            case 3:
                Application.Quit();
                break;
        }
    }
    
    /// <summary>인트로 타임라인 시작</summary>
    public void StartIntroTimeline()
    {
        _introPlayerDirector.Play();

        StartCoroutine(IntroTimelineEndCheck());
    }

    /// <summary>인트로 타임라인 끝남 체크</summary>
    private IEnumerator IntroTimelineEndCheck()
    {
        yield return new WaitUntil(() => _introPlayerDirector.time >= 35f);
        
        CPlayerManager.Instance.Controller2D.ChangeState(EPlayerState2D.DownIdle);
        CCameraController.Instance.IsHoldingToTarget = true;
        CPlayerManager.Instance.Controller2D.IsUseGravity = false;
        Vector3 startPosition = CPlayerManager.Instance.Controller2D.transform.position;
        startPosition.x = -46.72f;
        startPosition.y = 1f;
        CPlayerManager.Instance.Controller2D.transform.position = startPosition;
        _introPlayerDirector.gameObject.SetActive(false);

        CCameraController.Instance.SetTargetDisplay(0);
        CUIManager.Instance.SetTargetDisplay(0);
        CPlayerManager.Instance.IsCanOperation = true;

        CWorldManager.Instance.PlayBGM();
        CDataManager.IsSaveData = true;

        gameObject.SetActive(false);
    }

    ///// <summary>모든 스테이지 잠금해제</summary>
    //private void AllStageUnlock()
    //{
    //    EXmlDocumentNames documentName = EXmlDocumentNames.GrassStageDatas;
    //    string[] elementsName = new string[] { "IsUnlock" };
    //    string[] datas = new string[] { "True" };
    //    string nodePath = null;
    //    // 데이터 쓰기
    //    for (int i = 1; i < 8; i++)
    //    {
    //        nodePath = "GrassStageDatas/StageDatas/GrassStage_Stage" + i.ToString();
    //        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
    //    }

    //    nodePath = "GrassStageDatas/StageDatas/StageSelect_Snow";
    //    CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);

    //    documentName = EXmlDocumentNames.SnowStageDatas;

    //    for(int i = 1; i < 3; i++)
    //    {
    //        nodePath = "SnowStageDatas/StageDatas/SnowStage_Stage" + i.ToString();
    //        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
    //    }

    //    // 데이터 저장
    //    CDataManager.SaveCurrentXmlDocument();

    //    // 스테이지 선택 씬 불러오기
    //    SceneManager.LoadScene("StageSelect_Grass");
    //}

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

    /// <summary>메인 메뉴 그룹</summary>
    [SerializeField]
    private GameObject _mainMenu = null;
    /// <summary>옵션 메뉴 그룹</summary>
    [SerializeField]
    private GameObject _optionMenu = null;

    /// <summary>메인 메뉴 활성화 여부</summary>
    private void SetActiveMainMenu(bool value) { _mainMenu.SetActive(value); }
    /// <summary>옵션 메뉴 활성화 여부</summary>
    private void SetActiveOptionMenu(bool value) { _optionMenu.SetActive(value); }

    /// <summary>선택 옵션 메뉴 백 이미지</summary>
    [SerializeField]
    private Transform _selectOptionMenuBG = null;
    /// <summary>현재 옵션 선택 메뉴</summary>
    private int _currentSelectOptionMenu = 1;

    /// <summary>옵션 메뉴 활성화</summary>
    private void OnEnableOntionMenu()
    {
        // 메인 메뉴 비활성화, 옵션 메뉴 활성화
        SetActiveMainMenu(false);
        SetActiveOptionMenu(true);

        _currentSelectOptionMenu = 0;

        // 선택 메뉴 초기화(맨 위)
        SetSelectOptionMenuBGPoint(_currentSelectOptionMenu);
    }

    /// <summary>옵션 메뉴 비활성화</summary>
    private void OnDisableOtionMenu()
    {
        _isExcutionAnything = false;

        SetActiveMainMenu(true);
        SetActiveOptionMenu(false);

        StartCoroutine(MainMenuInputLogic());
        StopCoroutine("OptionMenuInputLogic");
    }

    /// <summary>선택 옵션 메뉴 BG 위치 설정</summary>
    public void SetSelectOptionMenuBGPoint(int selectMenuValue)
    {
        if (_currentSelectOptionMenu.Equals(selectMenuValue))
            return;

        _currentSelectOptionMenu = selectMenuValue;

        Vector3 temp = Vector3.up * 70f;
        _selectOptionMenuBG.localPosition = -temp * _currentSelectOptionMenu + temp + Vector3.right * -65f;

        PlayPointerUpAudio();
    }

    /// <summary>옵션 메뉴 입력 로직</summary>
    private IEnumerator OptionMenuInputLogic()
    {
        OnEnableOntionMenu();

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
                break;

            yield return null;
        }

        OnDisableOtionMenu();
    }

    /// <summary>옵션 적용</summary>
    public void ApplyOption()
    {
        ApplyWindowModeWithResolution();
        ApplySound();

        PlayPointerEnterAudio();
    }

    /// <summary>윈도우 모드 및 해상도 변경</summary>
    private void ApplyWindowModeWithResolution()
    {
        // 윈도우 모드나 해상도가 변경되었는지 체크
        bool isChanging = _currentWindowMode != _selectWindowMode ||
                          _currentResolution != _selectResolution ? true : false;

        if (!isChanging)
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

    /// <summary>사운드 적용</summary>
    private void ApplySound()
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

        OnDisableOtionMenu();
    }

    /// <summary>윈도우 모드 설정 취소</summary>
    private void CancelWindowMode()
    {
        _selectWindowMode = _currentWindowMode;

        UpdateWindowModeUI();
    }

    /// <summary>해상도 설정 취소</summary>
    private void CancelResolution()
    {
        _selectResolution = _currentResolution;

        UpdateResolutionUI();
    }

    /// <summary>사운드 설정 취소</summary>
    private void CancelSound()
    {
        _selectBGMNormalizedValue = _currentBGMNormalizedValue;
        _selectSFXNormalizedValue = _currentSFXNormalizedValue;

        UpdateSoundUI(_BGMScroll, _currentBGMNormalizedValue);
        UpdateSoundUI(_SFXScroll, _currentSFXNormalizedValue);
    }


    ///////////////////////////////////////// Window Mode
    /// <summary>윈도우 모드 Enum</summary>
    private enum EWindowMode { FullScreen = 0, FullScreenWindow, Windowed };
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

        UpdateWindowModeUI();

        PlayPointerUpAudio();
    }

    /// <summary>윈도우 모드 UI 업데이트</summary>
    private void UpdateWindowModeUI()
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
        _selectResolution += addValue;

        if (_selectResolution.Equals(-1))
            _selectResolution = _resolution1.Count - 1;
        else if (_selectResolution.Equals(_resolution1.Count))
            _selectResolution = 0;

        UpdateResolutionUI();

        PlayPointerUpAudio();
    }

    /// <summary>해상도 UI 업데이트</summary>
    private void UpdateResolutionUI()
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

    /// <summary>사운드 UI 업데이트</summary>
    private void UpdateSoundUI(CScroll scroll, float normalizedValue) { scroll.SetScroll(normalizedValue); }

    /// <summary>BGM 볼륨 설정</summary>
    public void ChangeBGMVolume() { _selectBGMNormalizedValue = _BGMScroll.NormalizedValue; }
    /// <summary>SFX 볼륨 설정</summary>
    public void ChangeSFXVolume() { _selectSFXNormalizedValue = _SFXScroll.NormalizedValue; }
}
