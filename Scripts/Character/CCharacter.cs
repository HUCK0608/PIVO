using UnityEngine;

public abstract class CCharacter : MonoBehaviour
{
    private GameObject _rootObject2D;
    private GameObject _rootObject3D;

    /// <summary>최상위 2D 오브젝트</summary>
    public GameObject RootObject2D { get { return _rootObject2D; } }
    /// <summary>최상위 3D 오브젝트</summary>
    public GameObject RootObject3D { get { return _rootObject3D; } }

    protected virtual void Awake()
    {
        _rootObject2D = transform.Find("Root2D").gameObject;
        _rootObject3D = transform.Find("Root3D").gameObject;
    }

    public virtual void Change2D()
    {
        _rootObject2D.SetActive(false);
        _rootObject2D.transform.parent = transform;
        _rootObject2D.transform.eulerAngles = Vector3.zero;
        _rootObject3D.transform.parent = RootObject2D.transform;
        _rootObject2D.SetActive(true);
    }

    public virtual void Change3D()
    {
        _rootObject2D.SetActive(false);
        _rootObject3D.transform.parent = transform;
        _rootObject2D.transform.parent = RootObject3D.transform;
        _rootObject3D.SetActive(true);
    }
}
