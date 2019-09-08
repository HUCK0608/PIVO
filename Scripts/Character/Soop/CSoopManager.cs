using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopManager : MonoBehaviour
{
    private CSoopController2D _controller2D = null;
    /// <summary>숲숲이 컨트롤러 2D</summary>
    public CSoopController2D Controller2D { get { return _controller2D; } }

    private CSoopController3D _controller3D = null;
    /// <summary>숲숲이 컨트롤러 3D</summary>
    public CSoopController3D Controller3D { get { return _controller3D; } }

    private void Awake()
    {
    }

    private void Start()
    {
    }
}
