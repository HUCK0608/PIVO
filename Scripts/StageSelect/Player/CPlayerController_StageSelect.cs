using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

public class CPlayerController_StageSelect : MonoBehaviour
{
    private static CPlayerController_StageSelect _instance = null;
    public static CPlayerController_StageSelect Insatnce { get { return _instance; } }

    /// <summary>중력 체크 지점들</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private List<Transform> _gravityCheckPoints = null;

    /// <summary>기어오르기 체크 지점</summary>
    [SerializeField]
    private Transform _climbCheckPoint = null;

    /// <summary>카메라</summary>
    [SerializeField]
    private Transform _camera = null;

    /// <summary>글로벌 포그</summary>
    [SerializeField]
    private GlobalFog _globalFog = null;
    /// <summary>시작 글로벌 포그 높이</summary>
    private float _startGlobalFogHeight = 0f;

    /// <summary>기어오르기 카메라 위치</summary>
    [SerializeField]
    private Transform[] _climbCameraPoints = null;

    /// <summary>현재 스테이지</summary>
    private CStage _currentStage = null;

    /// <summary>스텟</summary>
    private CPlayerStat_StageSelect _stat = null;

    /// <summary>리지드바디</summary>
    private Rigidbody _rigidbody;
    /// <summary>애니메이터</summary>
    private Animator _animator;

    /// <summary>Idle상태에서 기본 오일러 회전값</summary>
    private Vector3 _idleEulerRotation = new Vector3(0, 180f, 0);

    /// <summary>노드 이름</summary>
    private string _nodeName = "SelectPlayerDatas";
    /// <summary>속성들의 이름</summary>
    private string[] _elementsName = new string[] { "LastSeason", "CurrentStage" };

    public bool IsCanOperation = true;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _stat = GetComponent<CPlayerStat_StageSelect>();
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();

        LoadPlayerDatas();

        transform.position = _currentStage.transform.position;
        _camera.position = _currentStage.transform.position;

        // UI 변경
        CUIManager_StageSelect.Instance.SetStageStatUI(_currentStage);

        _startGlobalFogHeight = _globalFog.height;

        StartCoroutine(IdleLogic());
    }

    private void OnDestroy()
    {
        SavePlayerData();
    }

    /// <summary>플레이어 데이터 저장하기</summary>
    private void SavePlayerData()
    {
        EXmlDocumentNames selectPlayerDatasName = EXmlDocumentNames.SelectPlayerDatas;

        // 데이터 쓰기
        string nodePath = selectPlayerDatasName.ToString("G") + "/" + _nodeName;
        string[] datas = new string[] { CStageManager.Instance.XmlDocumentName.ToString("G"), _currentStage.GameSceneName };
        CDataManager.WritingDatas(selectPlayerDatasName, nodePath, _elementsName, datas);

        // 파일 저장
        CDataManager.SaveCurrentXmlDocument();
    }

    /// <summary>플레이어 데이터 불러오기</summary>
    private void LoadPlayerDatas()
    {
        EXmlDocumentNames selectPlayerDatasName = EXmlDocumentNames.SelectPlayerDatas;

        string nodePath = selectPlayerDatasName.ToString("G") + "/" + _nodeName;

        // 데이터 불러오기
        string[] datas = CDataManager.ReadDatas(selectPlayerDatasName, nodePath, _elementsName);

        // 스테이지들
        List<CStage> stages = CStageManager.Instance.Stages;

        // 데이터가 없다면 첫 번째 스테이지를 현재 스테이지로 적용 리턴
        if (datas == null)
        {
            _currentStage = stages[0];
            return;
        }
        // 데이터가 존재할 경우 현재 스테이지를 데이터에서 가져옴
        else
        {
            bool isHaveStage = false;
            for (int i = 0; i < stages.Count; i++)
            {
                if (stages[i].GameSceneName.Equals(datas[1]))
                {
                    _currentStage = stages[i];
                    isHaveStage = true;
                    break;
                }
            }

            if(!isHaveStage)
            {
                if (datas[1].Equals("GrassToSnowCut"))
                    _currentStage = stages[0];
                else if (datas[1].Equals("StageSelect_Grass"))
                    _currentStage = stages[stages.Count - 1];
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
            if(CUIManager_StageSelect.Instance.IsFadeInOut || !IsCanOperation)
            {
                yield return null;
                continue;
            }

            if (ApplyGravity())
                _animator.SetBool("IsFalling", true);
            else
                _animator.SetBool("IsFalling", false);

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

        // UI 변경
        CUIManager_StageSelect.Instance.SetStageStatUI(_currentStage);

        StartCoroutine(MoveLogic());
    }

    /// <summary>이동 로직</summary>
    private IEnumerator MoveLogic()
    {
        // 목적지
        Vector3 destination = _currentStage.transform.position;

        // 회전
        Vector3 direction = destination - transform.position;
        direction.y = 0f;
        transform.rotation = Quaternion.LookRotation(direction.normalized);

        _animator.SetBool("IsMove", true);
        
        while(true)
        {
            destination.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, destination, _stat.MoveSpeed * Time.deltaTime);
            _camera.position = transform.position;
            _globalFog.height = _camera.position.y + _startGlobalFogHeight;

            if (ApplyGravity())
                _animator.SetBool("IsFalling", true);
            else
                _animator.SetBool("IsFalling", false);

            RaycastHit hit;
            if (Physics.Raycast(_climbCheckPoint.position, transform.forward, out hit, 0.5f))
                yield return StartCoroutine(ClimbLogic(hit));

            if (transform.position.Equals(destination))
                break;

            yield return null;
        }

        _animator.SetBool("IsMove", false);

        StartCoroutine(IdleLogic());
    }

    /// <summary>기어오르기 로직</summary>
    private IEnumerator ClimbLogic(RaycastHit hit)
    {
        // 애니메이션 랜덤 재생
        int randAni = Random.Range(0f, 100f) <= _stat.Climb1Percent ? 0 : 1;

        _animator.SetInteger("Climb", randAni);

        // 시작점과 최종위치 계산
        Vector3 origin = hit.point + hit.normal;
        origin.y = transform.position.y;

        Vector3 destination = Vector3.zero;
        if (randAni.Equals(0))
            destination = hit.point - hit.normal * 0.69f + Vector3.up * 1.025f;
        else
            destination = hit.point - hit.normal * 0.71f + Vector3.up;

        // 캐릭터를 시작위치로 이동
        transform.position = origin;
        transform.rotation = Quaternion.LookRotation(-hit.normal);

        while(true)
        {
            _camera.position = _climbCameraPoints[randAni].position;
            _globalFog.height = _camera.position.y + _startGlobalFogHeight;

            AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            if(currentAnimatorStateInfo.IsName("Climb_0") && currentAnimatorStateInfo.normalizedTime >= 1.05f)
                break;
            else if(currentAnimatorStateInfo.IsName("Climb_1") && currentAnimatorStateInfo.normalizedTime >= 1.02f)
                break;

            yield return null;
        }

        transform.position = destination;

        _animator.SetInteger("Climb", -1);
    }

    /// <summary>중력 적용</summary>
    private bool ApplyGravity()
    {
        bool isApplyGravity = true;

        for(int i = 0; i < 4; i++)
        {
            if(Physics.Raycast(_gravityCheckPoints[i].position, Vector3.down, 0.15f))
            {
                isApplyGravity = false;
                break;
            }
        }

        // 땅이 아닐경우 중력 적용
        if (isApplyGravity)
        {
            Vector3 newVelocity = Vector3.zero;
            newVelocity.y = _rigidbody.velocity.y + _stat.Gravity;
            _rigidbody.velocity = newVelocity;
        }
        else
            _rigidbody.velocity = Vector3.zero;

        return isApplyGravity;
    }
}
