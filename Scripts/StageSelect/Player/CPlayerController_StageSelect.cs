using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPlayerController_StageSelect : MonoBehaviour
{
    /// <summary>카메라</summary>
    [SerializeField]
    private Transform _camera = null;

    /// <summary>현재 스테이지</summary>
    private CStage _currentStage = null;

    /// <summary>스텟</summary>
    private CPlayerStat_StageSelect _stat = null;

    /// <summary>애니메이터</summary>
    private Animator _animator;

    /// <summary>Idle상태에서 기본 오일러 회전값</summary>
    private Vector3 _idleEulerRotation = new Vector3(0, 180f, 0);

    /// <summary>노드 경로</summary>
    private string _nodePath = "GrassStageDatas/PlayerDatas";
    /// <summary>속성들의 이름</summary>
    private string[] _elementsName = new string[] { "CurrentStage" };

    private void Start()
    {
        _stat = GetComponent<CPlayerStat_StageSelect>();
        _animator = GetComponentInChildren<Animator>();

        LoadPlayerDatas();

        transform.position = _currentStage.transform.position;
        _camera.position = _currentStage.transform.position;

        StartCoroutine(IdleLogic());
    }

    /// <summary>플레이어 데이터 저장하기</summary>
    private void SavePlayerData()
    {
        // 데이터 파일 이름
        string dataFileName = CStageManager.Instance.DataFileName;

        // Xml 초기화
        CDataManager.InitXmlDocument(dataFileName, FileMode.OpenOrCreate);

        // 데이터 쓰기
        string[] datas = new string[] { _currentStage.GameSceneName };
        CDataManager.WritingData(_nodePath, _elementsName, datas);

        // 파일 저장
        CDataManager.SaveFile(dataFileName);
    }

    /// <summary>플레이어 데이터 불러오기</summary>
    private void LoadPlayerDatas()
    {
        // 데이터 파일 이름
        string dataFileName = CStageManager.Instance.DataFileName;

        // Xml 초기화
        CDataManager.InitXmlDocument(dataFileName, FileMode.OpenOrCreate);

        // 데이터 불러오기
        string[] datas = CDataManager.LoadData(_nodePath, _elementsName);

        // 스테이지들
        List<CStage> stages = CStageManager.Instance.Stages;

        // 데이터가 없다면 첫 번째 스테이지를 현재 스테이지로 적용 후 저장
        if (datas == null)
        {
            _currentStage = stages[0];
            SavePlayerData();
        }
        // 데이터가 존재할 경우 현재 스테이지를 데이터에서 가져옴
        else
        {
            for (int i = 0; i < stages.Count; i++)
            {
                if (stages[i].GameSceneName.Equals(datas[0]))
                    _currentStage = stages[i];
            }
        }
    }

    /// <summary>대기 로직</summary>
    private IEnumerator IdleLogic()
    {
        transform.eulerAngles = _idleEulerRotation;

        CStage nextStage = null;

        while(true)
        {
            if (Input.GetKeyDown(CKeyManager.StartStageKey))
            {
                SavePlayerData();
                _currentStage.StartStage();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Right);
            else if (Input.GetKeyDown(KeyCode.UpArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                nextStage = _currentStage.IsHaveStage(EStageDirection.Down);

            if (nextStage != null)
            {
                if(nextStage.IsUnlock)
                    break;
            }

            yield return null;
        }

        _currentStage = nextStage;

        StartCoroutine(MoveLogic());
    }

    /// <summary>이동 로직</summary>
    private IEnumerator MoveLogic()
    {
        // 목적지
        Vector3 destination = _currentStage.transform.position;
        destination.y = transform.position.y;

        // 회전
        Vector3 direction = destination - transform.position;
        transform.rotation = Quaternion.LookRotation(direction);

        _animator.SetBool("IsMove", true);
        
        while(true)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, _stat.MoveSpeed * Time.deltaTime);
            _camera.position = transform.position;

            if (transform.position.Equals(destination))
                break;

            yield return null;
        }

        _animator.SetBool("IsMove", false);

        StartCoroutine(IdleLogic());
    }
}
