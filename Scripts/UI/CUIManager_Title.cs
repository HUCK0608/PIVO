using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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

    /// <summary>현재 선택하고 있는 메뉴</summary>
    private int _currentSelect = 0;
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

        StartCoroutine(SelectMenuInputLogic());
    }

    /// <summary>메뉴 선택 로직</summary>
    private IEnumerator SelectMenuInputLogic()
    {
        ChangeSelectMenu(_currentSelect);

        while(!_isExcutionAnything)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int nextSelect = Mathf.Clamp(_currentSelect - 1, 0, _maxMenuCount - 1);

                if (!_isHaveData && nextSelect.Equals(1))
                    nextSelect = 0;

                ChangeSelectMenu(nextSelect);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int nextSelect = Mathf.Clamp(_currentSelect + 1, 0, _maxMenuCount - 1);

                if (!_isHaveData && nextSelect.Equals(1))
                    nextSelect = 2;

                ChangeSelectMenu(nextSelect);
            }
            else if (Input.GetKeyDown(KeyCode.Z))
                ExcutionSelectMenu(_currentSelect);

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

        _defaultMenu[_currentSelect].SetActive(true);
        _selectMenu[_currentSelect].SetActive(false);
        _currentSelect = selectMenu;
        _defaultMenu[_currentSelect].SetActive(false);
        _selectMenu[_currentSelect].SetActive(true);
    }

    /// <summary>선택 메뉴 실행</summary>
    public void ExcutionSelectMenu(int selectMenu)
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
}
