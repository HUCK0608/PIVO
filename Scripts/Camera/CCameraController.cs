using System.Collections;
using UnityEngine;

public class CCameraController : MonoBehaviour
{
    private static CCameraController _instance;
    /// <summary>카메라 컨트롤러 싱글턴</summary>
    public static CCameraController Instance { get { return _instance; } }

    private static string _animParameter = "Is3D";

    private Animator _animator;

    private Transform _target;
    public Transform Target { set { _target = value; } }

    private bool _isOnMovingWork = false;
    public bool IsOnMovingWork { get { return _isOnMovingWork; } }

    private void Awake()
    {
        _instance = this;

        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _target = CPlayerManager.Instance.RootObject3D.transform;
    }

    private void LateUpdate()
    {
        if(!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
            transform.position = _target.transform.position;
    }

    public void Change2D()
    {
        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, false);
    }

    public void Change3D()
    {
        _isOnMovingWork = true;
        _animator.SetBool(_animParameter, true);
    }

    public void CompleteMovingWork()
    {
        _isOnMovingWork = false;
    }
}
