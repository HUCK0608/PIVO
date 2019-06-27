using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTimelineTrigger3D : MonoBehaviour
{
    /// <summary>타임라인 트리거 스크립트</summary>
    CTimelineTrigger _timelineTirgger = null;

    private void Awake()
    {
        _timelineTirgger = GetComponentInParent<CTimelineTrigger>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 타임라인 로직 시작
        if (other.gameObject.layer.Equals(CLayer.Player))
        {
            _timelineTirgger.Collider3D.enabled = false;
            _timelineTirgger.StartTimelineLogic();
        }
    }
}
