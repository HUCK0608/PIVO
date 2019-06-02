using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CBiscuitManager : MonoBehaviour
{
    private static CBiscuitManager _instance;
    public static CBiscuitManager Instance { get { return _instance; } }

    /// <summary>비스킷 모음</summary>
    private List<CBiscuit> _biscuits;

    public int _haveBiscuitCount = 0;
    /// <summary>먹은 비스킷 개수</summary>
    public int HaveBiscuitCount { get { return _haveBiscuitCount; } set { _haveBiscuitCount = value; } }

    private void Awake()
    {
        _instance = this;

        _biscuits = new List<CBiscuit>();
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

        string[] scenePaths = SceneManager.GetActiveScene().name.Split('_');    // 0 : Season, 1 : Stage_x

        // 봄 스테이지일 경우
        if(scenePaths[0].Equals("GrassStage"))
        {
            /* 이전에 먹었던 비스킷 개수 데이터 가져오기 */
            string nodePath = EXmlDocumentNames.GrassStageDatas.ToString("G") + "/StageDatas/" + scenePaths[1];
            string[] elementsName = new string[] { "HaveBiscuitCount" };

            // 데이터 읽기
            datas = CDataManager.ReadDatas(EXmlDocumentNames.GrassStageDatas, nodePath, elementsName);

            // 데이터 적용
            if (datas != null)
                _haveBiscuitCount = int.Parse(datas[0]);

            /* 비스킷 먹음 여부 데이터 가져오기 */
            // 노드 경로 설정
            nodePath = EXmlDocumentNames.GrassStageDatas.ToString("G") + "/StageDatas/" + scenePaths[1] + "/BiscuitsDidEat";

            // 속성 배열 초기화
            int biscuitCount = _biscuits.Count;
            elementsName = new string[biscuitCount];

            // 속성 배열 설정
            for(int i = 0; i < biscuitCount; i++)
                elementsName[i] = "Biscuit_" + _biscuits[i].Number.ToString();

            // 데이터 읽기
            datas = CDataManager.ReadDatas(EXmlDocumentNames.GrassStageDatas, nodePath, elementsName);
        }
        else if(scenePaths[0].Equals("SnowStage"))
        {

        }

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

        string[] scenePaths = SceneManager.GetActiveScene().name.Split('_');    // 0 : Season, 1 : Stage_x

        // 봄 스테이지일 경우
        if (scenePaths[0].Equals("GrassStage"))
        {
            /* 이전에 먹었던 비스킷 개수 데이터 가져오기 */
            string nodePath = EXmlDocumentNames.GrassStageDatas.ToString("G") + "/StageDatas/" + scenePaths[1];
            string[] elementsName = new string[] { "HaveBiscuitCount" };
            string[] datas = new string[] { _haveBiscuitCount.ToString() };

            // 데이터 쓰기
            CDataManager.WritingDatas(EXmlDocumentNames.GrassStageDatas, nodePath, elementsName, datas);

            /* 비스킷 먹음 여부 데이터 가져오기 */
            // 노드 경로 설정
            nodePath = EXmlDocumentNames.GrassStageDatas.ToString("G") + "/StageDatas/" + scenePaths[1] + "/BiscuitsDidEat";

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
            CDataManager.WritingDatas(EXmlDocumentNames.GrassStageDatas, nodePath, elementsName, datas);
        }
        else if (scenePaths[0].Equals("SnowStage"))
        {

        }

        // 데이터 저장
        CDataManager.SaveCurrentXmlDocument();
    }
}
