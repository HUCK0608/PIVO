using UnityEngine;

public abstract class CWorldObject : MonoBehaviour
{
    private GameObject _rootObject;
    private GameObject _rootObject3D;
    private GameObject _rootObject2D;

    /// <summary>최상위 오브젝트</summary>
    public GameObject RootObject { get { return _rootObject; } }
    /// <summary>최상위 2D 오브젝트</summary>
    public GameObject RootObject2D { get { return _rootObject2D; } }
    /// <summary>최상위 3D 오브젝트</summary>
    public GameObject RootObject3D { get { return _rootObject3D; } }

    private bool _isCanChange2D;

    /// <summary>2D 시점전환 가능 여부</summary>
    public bool IsCanChange2D { get { return _isCanChange2D; } set { _isCanChange2D = value; } }

    protected virtual void Awake()
    {
        _rootObject = transform.parent.gameObject;
        _rootObject3D = gameObject;
        _rootObject2D = _rootObject.transform.Find("Root2D").gameObject;

        _isCanChange2D = false;
    }

    protected virtual void Start()
    {
        CWorldManager.Instance.AddWorldObject(this);
    }

    /// <summary>2D로 시점전환</summary>
    public abstract void Change2D();
    /// <summary>3D로 시점전환</summary>
    public abstract void Change3D();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.ViewChangeRect))
            _isCanChange2D = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.ViewChangeRect))
            _isCanChange2D = false;
    }
}
