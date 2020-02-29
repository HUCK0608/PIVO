using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoopState { Idle = 0, Surprise, Chase, Return, PutInit, PutMove, PutEnd, Dead }

public class CSoopManager : MonoBehaviour
{
    private CSoopStat _stat = null;
    /// <summary>숲숲이 스텟</summary>
    public CSoopStat Stat { get { return _stat; } }

    [SerializeField]
    private CSoopController2D _controller2D = null;
    /// <summary>숲숲이 2D 컨트롤러</summary>
    public CSoopController2D Controller2D { get { return _controller2D; } }
    [SerializeField]
    private CSoopController3D _controller3D = null;
    /// <summary>숲숲이 3D 컨트롤러</summary>
    public CSoopController3D Controller3D { get { return _controller3D; } }

    public static bool _isCanUseEmoticon = true;

    private void Awake()
    {
        _isCanUseEmoticon = true;
        _stat = GetComponent<CSoopStat>();
    }
    
    /// <summary>숲숲이의 죽음 상태 설정</summary>
    public void ActivateDead(bool active)
    {
        Controller2D.ActivateDead(active);
        Controller3D.ActivateDead(active);
    }
}
