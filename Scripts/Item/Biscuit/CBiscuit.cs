using System.Collections;
using UnityEngine;

public class CBiscuit : MonoBehaviour
{
    [SerializeField]
    private float _upDownRange = 0f;

    [SerializeField]
    private float _moveSpeed = 0f;

    [SerializeField]
    private GameObject _biscuitEatEffect = null;

    private int _number = -1;
    /// <summary>구분 숫자</summary>
    public int Number { get { return _number; } }
    public bool _isDidEat = false;
    /// <summary>이전에 먹은 비스킷인지 여부</summary>
    public bool IsDidEat { get { return _isDidEat; } set { _isDidEat = value; } }

    private void Awake()
    {
        if (CBiscuitManager.Instance == null)
            gameObject.AddComponent<CBiscuitManager>();

        RegisterBiscuit();
    }

    private void Start()
    {
        StartCoroutine(MoveUpDown());
    }

    /// <summary>비스킷 등록</summary>
    private void RegisterBiscuit()
    {
        _number = int.Parse(gameObject.name.Split('_')[2]);

        CBiscuitManager.Instance.RegisterBiscuit(this);
    }

    /// <summary>비스킷을 먹어서 없앰</summary>
    public void DestroyBiscuit()
    {
        // 이펙트 보여주기
        _biscuitEatEffect.transform.parent = null;
        _biscuitEatEffect.transform.position = CPlayerManager.Instance.RootObject3D.transform.position;
        _biscuitEatEffect.SetActive(true);

        _isDidEat = true;
        CBiscuitManager.Instance.HaveBiscuitCount++;
        CUIManager.Instance.SetBiscuitUI(CBiscuitManager.Instance.HaveBiscuitCount);
        gameObject.SetActive(false);
    }

    /// <summary>위 아래 이동</summary>
    private IEnumerator MoveUpDown()
    {
        Vector3 startPosition = transform.position;
        float currentMoveY = 0f;
        bool isMoveUp = true;

        while(true)
        {
            if(!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
            {
                if (isMoveUp)
                {
                    currentMoveY = Mathf.Clamp(currentMoveY + _moveSpeed * Time.deltaTime, -_upDownRange, _upDownRange);

                    if (currentMoveY.Equals(_upDownRange))
                        isMoveUp = false;
                }
                else
                {
                    currentMoveY = Mathf.Clamp(currentMoveY - _moveSpeed * Time.deltaTime, -_upDownRange, _upDownRange);

                    if (currentMoveY.Equals(-_upDownRange))
                        isMoveUp = true;
                }
            }

            transform.position = startPosition + Vector3.up * currentMoveY;

            yield return null;
        }
    }
}
