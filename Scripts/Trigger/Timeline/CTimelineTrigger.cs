using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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

    /// <summary>타임라인 시작 지점</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private Transform _startTimelinePoint = null;

    private void Awake()
    {
        _collider3D = GetComponentInChildren<Collider>();

        _playableDirector.gameObject.SetActive(false);
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

        // 타겟 디스플레이 변경
        CCameraController.Instance.SetTargetDisplay(1);
        CUIManager.Instance.SetTargetDisplay(1);

        // 타임라인 활성화
        _playableDirector.gameObject.SetActive(true);
        // 타임라인 시작
        _playableDirector.Play();
        // 타임라인이 끝날때까지 대기
        yield return new WaitUntil(() => _playableDirector.time >= _timelineEndTime);
        // 스테이지 클리어 처리
        CWorldManager.Instance.StageClear();
    }
}
