using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CBiscuitManager : MonoBehaviour
{
    private static CBiscuitManager _instance;
    public static CBiscuitManager Instance { get { return _instance; } }

    /// <summary>비스킷 모음</summary>
    private List<CBiscuit> _biscuits = new List<CBiscuit>();

    private int _currentBiscuitCount = 0;
    /// <summary>현재 비스킷 개수</summary>
    public int CurrentBiscuitCount { get { return _currentBiscuitCount; } set { _currentBiscuitCount = value; } }

    private int _saveBiscuitCount = 0;
    /// <summary>저장된 비스킷 개수</summary>
    public int SaveBiscuitCount { get { return _saveBiscuitCount; } set { _saveBiscuitCount = value; } }

    [SerializeField]
    private int[] _requirements = null;
    /// <summary>별을 얻는 조건 개수</summary>
    public int[] Requirements { get { return _requirements; } }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        LoadDatas();
    }

    private void OnDestroy()
    {
        SaveDatas();
    }

    /// <summary>
    /// 비스킷 등록
    /// </summary>
    /// <param name="number">번호</param>
    /// <param name="biscuit">비스킷 스크립트</param>
    public void RegisterBiscuit(CBiscuit biscuit)
    {
        _biscuits.Add(biscuit);
    }

    /// <summary>데이터 불러오기</summary>
    private void LoadDatas()
    {
        string[] datas = null;

        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] scenePaths = currentSceneName.Split('_');    // 0 : Season, 1 : Stage_x

        // 파일 이름 지정
        EXmlDocumentNames documentName = EXmlDocumentNames.None;
        if (scenePaths[0].Equals("GrassStage"))
            documentName = EXmlDocumentNames.GrassStageDatas;
        else if (scenePaths[0].Equals("SnowStage"))
            documentName = EXmlDocumentNames.SnowStageDatas;

        /* 이전에 먹었던 비스킷 개수 데이터 가져오기 */
        string nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
        string[] elementsName = new string[] { "HaveBiscuitCount" };

        // 데이터 읽기
        datas = CDataManager.ReadDatas(documentName, nodePath, elementsName);

        // 데이터 적용
        if (datas != null)
            _saveBiscuitCount = int.Parse(datas[0]);

        /* 비스킷 먹음 여부 데이터 가져오기 */
        // 노드 경로 설정
        nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName + "/BiscuitsDidEat";

        // 속성 배열 초기화
        int biscuitCount = _biscuits.Count;
        elementsName = new string[biscuitCount];

        // 속성 배열 설정
        for(int i = 0; i < biscuitCount; i++)
            elementsName[i] = "Biscuit_" + _biscuits[i].Number.ToString();

        // 데이터 읽기
        datas = CDataManager.ReadDatas(documentName, nodePath, elementsName);

        // 데이터가 없다면 저장 후 리턴
        if (datas == null)
        {
            SaveDatas();
            return;
        }

        // 데이터가 있을 경우 해당 비스킷의 데이터가 있는지 확인 후 적용
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i] != null)
                _biscuits[i].IsDidEat = datas[i].ToBoolean();
        }
    }

    /// <summary>데이터 저장하기</summary>
    private void SaveDatas()
    {
        int biscuitCount = _biscuits.Count;

        string currentSceneName = SceneManager.GetActiveScene().name;
        string[] scenePaths = currentSceneName.Split('_');    // 0 : Season, 1 : Stage_x

        // 파일 이름 지정
        EXmlDocumentNames documentName = EXmlDocumentNames.None;
        if (scenePaths[0].Equals("GrassStage"))
            documentName = EXmlDocumentNames.GrassStageDatas;
        else if (scenePaths[0].Equals("SnowStage"))
            documentName = EXmlDocumentNames.SnowStageDatas;

        string nodePath = string.Empty;
        string[] elementsName = null;
        string[] datas = null;

        if (_saveBiscuitCount.Equals(0) || _currentBiscuitCount > _saveBiscuitCount)
        {
            /* 이전에 먹었던 비스킷 개수 데이터 저장 */
            nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
            elementsName = new string[] { "HaveBiscuitCount" };
            datas = new string[] { _saveBiscuitCount.ToString() };

            // 데이터 쓰기
            CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
        }

        /* 별 개수 데이터 저장 */
        nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName;
        elementsName = new string[] { "Stars" };
        datas = new string[] { GetCurrentStar().ToString() };

        // 데이터 쓰기
        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);

        /* 해금조건 저장 */
        elementsName = new string[] { "RequirementStar1", "RequirementStar2", "RequirementStar3" };
        datas = new string[] { _requirements[0].ToString(), _requirements[1].ToString(), _requirements[2].ToString() };

        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);
        

        /* 비스킷 먹음 여부 데이터 저장 */
        // 노드 경로 설정
        nodePath = documentName.ToString("G") + "/StageDatas/" + currentSceneName + "/BiscuitsDidEat";

        // 배열 초기화
        elementsName = new string[biscuitCount];
        datas = new string[biscuitCount];

        // 속성 배열 설정
        for (int i = 0; i < biscuitCount; i++)
        {
            elementsName[i] = "Biscuit_" + _biscuits[i].Number.ToString();
            datas[i] = _biscuits[i].IsDidEat.ToString();
        }

        // 데이터 쓰기
        CDataManager.WritingDatas(documentName, nodePath, elementsName, datas);

        // 데이터 저장
        CDataManager.SaveCurrentXmlDocument();
    }

    /// <summary>현재 별의 개수 반환</summary>
    public int GetCurrentStar()
    {
        int currentStar = 0;

        for(int i = 0; i < 3; i++)
        {
            if (_currentBiscuitCount >= _requirements[i])
                currentStar++;
        }

        return currentStar;
    }
}
