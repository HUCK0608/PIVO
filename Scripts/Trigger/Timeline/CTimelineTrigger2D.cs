using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTimelineTrigger2D : MonoBehaviour
{
    /// <summary>타임라인 트리거 스크립트</summary>
    CTimelineTrigger _timelineTirgger = null;

    private void Awake()
    {
        _timelineTirgger = GetComponentInParent<CTimelineTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 3D 상태로 변경 후 타임라인 로직 시작
        if (collision.gameObject.layer.Equals(CLayer.Player))
        {
            _timelineTirgger.Collider3D.enabled = false;
            CWorldManager.Instance.ChangeWorld();
            _timelineTirgger.StartTimelineLogic();
        }
    }
}
