using UnityEngine;

public abstract class CWorldObject : MonoBehaviour
{
    private GameObject _rootObject = null;
    private GameObject _rootObject3D = null;

    /// <summary>최상위 오브젝트</summary>
    public GameObject RootObject { get { return _rootObject; } }
    /// <summary>최상위 3D 오브젝트</summary>
    public GameObject RootObject3D { get { return _rootObject3D; } }

    private bool _isCanChange2D = false;
    /// <summary>2D 시점전환 가능 여부</summary>
    public bool IsCanChange2D { get { return _isCanChange2D; } set { _isCanChange2D = value; } }

    [SerializeField]
    private bool _isUse2DTexture = false;
    /// <summary>2D 텍스처 사용 여부</summary>
    public bool IsUse2DTexture { get { return _isUse2DTexture; } }

    private static Material _blockMateiral = null;
    /// <summary>블락 머테리얼</summary>
    public static Material BlockMaterial { get { return _blockMateiral; } }

    protected virtual void Awake()
    {
        _rootObject = transform.parent.gameObject;
        _rootObject3D = gameObject;

        _blockMateiral = Resources.Load("BlockMaterialDumy") as Material;

        _isCanChange2D = false;
    }

    protected virtual void Start()
    {
        if(CWorldManager.Instance != null)
            CWorldManager.Instance.AddWorldObject(this);
    }

    /// <summary>2D로 시점전환</summary>
    public abstract void Change2D();
    /// <summary>3D로 시점전환</summary>
    public abstract void Change3D();
    /// <summary>오브젝트를 블락 상태로 보이게 함</summary>
    public abstract void ShowOnBlock();
    /// <summary>블락 상태의 오브젝트를 원래 상태로 되돌림</summary>
    public abstract void ShowOffBlock();

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.ViewChangeRect))
            _isCanChange2D = true;
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer.Equals(CLayer.ViewChangeRect))
        {
            _isCanChange2D = false;
            ShowOffBlock();
        }
    }
}
