using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityStandardAssets.ImageEffects;

public class CTimelineTrigger : MonoBehaviour
{
    private Collider _collider3D = null;
    /// <summary>3D 콜라이더</summary>
    public Collider Collider3D { get { return _collider3D; } }

    /// <summary>Playable Director</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private PlayableDirector _playableDirector = null;

    /// <summary>타임라인 종료 시간</summary>
    [SerializeField]
    private float _timelineEndTime = 0f;


    /// <summary>스테이지 선택씬이 아닌 타임라인이 있는 씬으로 이동할 때 체크</summary>
    [SerializeField]
    private bool _bUseTimelineScene = false;
    /// <summary>_bUseTimelineScene이 true일때만 사용. 이동할 씬의 이름 작성</summary>
    [SerializeField]
    private string _TimelineSceneName = "";

    /// <summary>타임라인 시작 지점</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private Transform _startTimelinePoint = null;

    private void Awake()
    {
        _collider3D = GetComponentInChildren<Collider>();

        _playableDirector.gameObject.SetActive(false);
        if (_playableDirector.transform.position != transform.position)
        {
            Debug.LogError("도착지점과 타임라인의 위치를 동일하게 맞춰야 합니다.");
            _playableDirector.transform.position = transform.position;
        }
    }

    /// <summary>타임라인 로직 시작</summary>
    public void StartTimelineLogic()
    {
        // 타임라인 종료 체크 코루틴 시작
        StartCoroutine(TimelineLogic());
    }
    
    /// <summary>타임라인 종료 체크</summary>
    private IEnumerator TimelineLogic()
    {
        // 타임라인 시작 지점으로 자동 이동
        CPlayerManager.Instance.StartAutoMove(_startTimelinePoint.position);
        // 자동 이동이 끝날때까지 대기
        yield return new WaitUntil(() => CPlayerManager.Instance.IsCanOperation);
        // 플레이어 비활성화
        CPlayerManager.Instance.gameObject.SetActive(false);
        // InGameUI 비활성화
        CUIManager.Instance.SetActiveInGameUI(false);

        // 타겟 디스플레이 변경
        CCameraController.Instance.SetTargetDisplay(1);
        GlobalFog timelineGlobalFog = _playableDirector.GetComponentInChildren<GlobalFog>();
        timelineGlobalFog.height = CCameraController.Instance.GlobalFogHeight;
        timelineGlobalFog.heightDensity = CCameraController.Instance.GlobalFogHeightDensity;

        CSoopManager._isCanUseEmoticon = false;

        // 타임라인 활성화
        _playableDirector.gameObject.SetActive(true);
        // 타임라인 시작
        _playableDirector.Play();
        // 타임라인이 끝날때까지 대기
        yield return new WaitUntil(() => _playableDirector.time >= _timelineEndTime);

        //화면이 완전히 암전되는 것을 기다리기 위해서 사용
        yield return new WaitForSeconds(1.0f);
        // 클리어 UI 활성화
        CUIManager.Instance.SetActiveStageClear(true);
    }
}
