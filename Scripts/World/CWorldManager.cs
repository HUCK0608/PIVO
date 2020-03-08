using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EWorldState { View2D, View3D, Changing }
public class CWorldManager : MonoBehaviour
{
    private static CWorldManager _instance;
    /// <summary>월드 매니저 싱글턴</summary>
    public static CWorldManager Instance { get { return _instance; } }

    private EWorldState _currentWorldState = EWorldState.View3D;
    /// <summary>현재 월드 상태</summary>
    public EWorldState CurrentWorldState { get { return _currentWorldState; } set { _currentWorldState = value; } }

    /// <summary>월드의 오브젝트 리스트</summary>
    private List<CWorldObject> _worldObjects = null;
    /// <summary>월드 오브젝트 개수</summary>
    private int _worldObjectCount = 0;
    /// <summary>제일 최근에 변경되었던 오브젝트 모음</summary>
    private List<CWorldObject> _lastChangeObjects = null;
    /// <summary>제일 최근에 변경되었던 오브젝트 개수</summary>
    private int _lastChangeObjectCount = 0;

    /// <summary>이펙트 활성화 컨트롤러 리스트</summary>
    private List<CEffectVisableController> _effectVisableControllers = null;
    /// <summary>이펙트 활성화 컨트롤러 개수</summary>
    private int _effectVisableControllerCount = 0;

    /// <summary>카메라 무빙워크가 끝날때까지 대기</summary>
    private WaitUntil _endCameraMovingWorkWaitUntil = null;
    /// <summary>플레이어가 2D로 변하기까지 기다리는 시간</summary>
    private WaitForSeconds _changePlayer2DWaitForSeconds = null;

    /// <summary>오디오 소스</summary>
    private AudioSource _audioSource = null;
    /// <summary>반복 BGM 오디오 클립</summary>
    [SerializeField]
    private AudioClip _bgmLoopAudioClip = null;
    /// <summary>BGM을 사용할지 여부</summary>
    [SerializeField]
    private bool _isUseBGM = true;

    [SerializeField]
    private bool _isUseTimelineScene = false;
    /// <summary>스테이지 선택씬이 아닌 타임라인이 있는 씬으로 이동할 때 체크</summary>
    public bool IsUseTimeLineScene { get { return _isUseTimelineScene; } }
    [SerializeField]
    private string _TimelineSceneName = string.Empty;
    /// <summary>_bUseTimelineScene이 true일때만 사용. 이동할 씬의 이름 작성</summary>
    public string TimeLineSceneName { get { return _TimelineSceneName; } }

    private void Awake()
    {
        _instance = this;

        _worldObjects = new List<CWorldObject>();
        _lastChangeObjects = new List<CWorldObject>();

        _effectVisableControllers = new List<CEffectVisableController>();

        _endCameraMovingWorkWaitUntil = new WaitUntil(() => !CCameraController.Instance.IsOnMovingWork);
        _changePlayer2DWaitForSeconds = new WaitForSeconds(0.3f);

        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(_isUseBGM)
            PlayBGM();
    }

    /// <summary>월드 오브젝트 등록</summary>
    public void AddWorldObject(CWorldObject worldObject)
    {
        _worldObjects.Add(worldObject);
        _worldObjectCount++;
    }

    //@Design 폭탄오브젝트가 생성됐다 사라졌다를 반복해야해서 추가했음.
    /// <summary>월드 오브젝트 제거</summary>
    public void RemoveWorldObject(CWorldObject worldObject)
    {
        _worldObjects.Remove(worldObject);
        _worldObjectCount--;
    }

    /// <summary>이펙트 활성화 컨트롤러 등록</summary>
    public void AddEffectVisableController(CEffectVisableController effectVisableController)
    {
        _effectVisableControllers.Add(effectVisableController);
        _effectVisableControllerCount++;
    }

    /// <summary>이펙트 활성화 컨트롤러 제거</summary>
    public void RemoveEffectVisableController(CEffectVisableController effectVisableController)
    {
        _effectVisableControllers.Remove(effectVisableController);
        _effectVisableControllerCount--;
    }

    /// <summary>포함되었던 오브젝트 리셋</summary>
    public void ResetIncludedWorldObjects()
    {
        for (int i = 0; i < _worldObjectCount; i++)
            _worldObjects[i].IsCanChange2D = false;
    }

    /// <summary>월드 변경</summary>
    public void ChangeWorld(bool isChangeLastObject = false)
    {
        if (isChangeLastObject)
            SetLastObjects();

        StartCoroutine(ChangeWorldLogic());
    }

    /// <summary>최근 오브젝트 설정</summary>
    private void SetLastObjects()
    {
        for (int i = 0; i < _lastChangeObjectCount; i++)
            _lastChangeObjects[i].IsCanChange2D = true;
    }

    /// <summary>월드 변경 로직</summary>
    private IEnumerator ChangeWorldLogic()
    {
        if(_currentWorldState.Equals(EWorldState.View2D))
        {
            _currentWorldState = EWorldState.Changing;

            CCameraController.Instance.Change3D();

            for (int i = 0; i < _worldObjectCount; i++)
                _worldObjects[i].Change3D();
            for (int i = 0; i < _effectVisableControllerCount; i++)
                _effectVisableControllers[i].Change3D();

            CLightController.Instance.SetShadows(true);
            CPlayerManager.Instance.Change3D();

            yield return _endCameraMovingWorkWaitUntil;

            _currentWorldState = EWorldState.View3D;
        }
        else
        {
            _currentWorldState = EWorldState.Changing;

            CCameraController.Instance.Change2D();

            _lastChangeObjects.Clear();
            _lastChangeObjectCount = 0;

            for (int i = 0; i < _worldObjectCount; i++)
            {
                _worldObjects[i].Change2D();

                if (_worldObjects[i].IsCanChange2D)
                {
                    _lastChangeObjects.Add(_worldObjects[i]);
                    _lastChangeObjectCount++;
                }
            }

            for (int i = 0; i < _effectVisableControllerCount; i++)
                _effectVisableControllers[i].Change2D();

            CLightController.Instance.SetShadows(false);

            yield return _changePlayer2DWaitForSeconds;
            CPlayerManager.Instance.Change2D();

            yield return _endCameraMovingWorkWaitUntil;

            _currentWorldState = EWorldState.View2D;
        }
    }

    /// <summary>모든 오브젝트를 2D 상태로 변경 가능하게 설정</summary>
    public void AllObjectsCanChange2D()
    {
        for(int i = 0; i < _worldObjectCount; i++)
            _worldObjects[i].IsCanChange2D = true;
    }

    /// <summary>스테이지 클리어</summary>
    public void StageClear()
    {
        StartCoroutine(Test());
        //string currentSceneName = SceneManager.GetActiveScene().name;
        //string[] scenePaths = currentSceneName.Split('_');     // 0 : Season, 1 : Stage_x
        //string stageSelectScenePath = null;

        //// 파일 이름 지정
        //EXmlDocumentNames documentName = EXmlDocumentNames.None;
        //if (scenePaths[0].Equals("GrassStage"))
        //{
        //    documentName = EXmlDocumentNames.GrassStageDatas;
        //    stageSelectScenePath = "StageSelect_Grass";
        //}
        //else if (scenePaths[0].Equals("SnowStage"))
        //{
        //    documentName = EXmlDocumentNames.SnowStageDatas;
        //    stageSelectScenePath = "StageSelect_Snow";
        //}

        //// 저장에 필요한 변수 설정
        //string nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
        //string[] elementsName = new string[] { "IsClear", "IsUnlock" };
        //string[] datas = new string[] { "True", "True" };

        //// 데이터 쓰기
        //CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
        //// 데이터 저장
        //CDataManager.SaveCurrentXmlDocument();

        //// 스테이지 선택씬 로드
        //SceneManager.LoadScene(stageSelectScenePath);
    }

    private IEnumerator Test()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] scenePaths = currentSceneName.Split('_');     // 0 : Season, 1 : Stage_x
        string stageSelectScenePath = null;

        // 파일 이름 지정
        EXmlDocumentNames documentName = EXmlDocumentNames.None;
        if (scenePaths[0].Equals("GrassStage"))
        {
            documentName = EXmlDocumentNames.GrassStageDatas;
            stageSelectScenePath = "StageSelect_Grass";
        }
        else if (scenePaths[0].Equals("SnowStage"))
        {
            documentName = EXmlDocumentNames.SnowStageDatas;
            stageSelectScenePath = "StageSelect_Snow";
        }

        // 저장에 필요한 변수 설정
        string nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
        string[] elementsName = new string[] { "IsClear", "IsUnlock" };
        string[] datas = new string[] { "True", "True" };

        // 데이터 쓰기
        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
        // 데이터 저장
        CDataManager.SaveCurrentXmlDocument();

        yield return new WaitForSeconds(3f);

        // 스테이지 선택씬 로드
        SceneManager.LoadScene(stageSelectScenePath);
    }


    /// <summary>스테이지 클리어 후 타임라인이 들어있는 씬을 플레이하기 위해서 추가</summary>
    public void StageClearWaitTimeLineScene(string TimeLineSceneName)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] scenePaths = currentSceneName.Split('_');     // 0 : Season, 1 : Stage_x

        // 파일 이름 지정
        EXmlDocumentNames documentName = EXmlDocumentNames.None;
        if (scenePaths[0].Equals("GrassStage"))
        {
            documentName = EXmlDocumentNames.GrassStageDatas;
        }
        else if (scenePaths[0].Equals("SnowStage"))
        {
            documentName = EXmlDocumentNames.SnowStageDatas;
        }

        // 저장에 필요한 변수 설정
        string nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
        string[] elementsName = new string[] { "IsClear" };
        string[] datas = new string[] { "True" };

        // 데이터 쓰기
        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
        // 데이터 저장
        CDataManager.SaveCurrentXmlDocument();

        // 타임라인을 실행할 씬 틀기
        SceneManager.LoadScene(TimeLineSceneName);
    }
    
    /// <summary>BGM 재생</summary>
    public void PlayBGM()
    {
        StartCoroutine(FadeUpPlayBGM());
        StartCoroutine(BGMLogic());
    }

    private IEnumerator FadeUpPlayBGM()
    {
        var FadeSpeed = 1f;

        _audioSource.Play();
        _audioSource.volume = 0f;

        while (_audioSource.volume < 1)
        {
            yield return new WaitForFixedUpdate();
            _audioSource.volume += Time.deltaTime * FadeSpeed;
        }

        _audioSource.volume = 1f;
    }

    private IEnumerator BGMLogic()
    {
        // Intro BGM 이 끝날때까지 대기
        yield return new WaitUntil(() => !_audioSource.isPlaying);

        // 반복 오디오 재생
        _audioSource.clip = _bgmLoopAudioClip;
        _audioSource.loop = true;
        _audioSource.PlayDelayed(0.4f);
    }
}
