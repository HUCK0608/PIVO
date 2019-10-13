using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoopState { Idle = 0, Surprise, Chase, Return, PutInit, PutMove, PutEnd }

public class CSoopManager : MonoBehaviour
{
    private CSoopStat _stat = null;
    /// <summary>숲숲이 스텟</summary>
    public CSoopStat Stat { get { return _stat; } }

    private CSoopController2D _controller2D = null;
    /// <summary>숲숲이 2D 컨트롤러</summary>
    public CSoopController2D Controller2D { get { return _controller2D; } }
    private CSoopController3D _controller3D = null;
    /// <summary>숲숲이 3D 컨트롤러</summary>
    public CSoopController3D Controller3D { get { return _controller3D; } }

    private void Awake()
    {
        _stat = GetComponent<CSoopStat>();

        _controller2D = GetComponentInChildren<CSoopController2D>();
        _controller3D = GetComponentInChildren<CSoopController3D>();
    }
}
