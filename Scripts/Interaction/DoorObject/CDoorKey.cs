using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDoorKey : MonoBehaviour
{
    /// <summary>이펙트 활성화 컨트롤러</summary>
    [Header("Programmer can edit")]
    [SerializeField]
    private CEffectVisableController _effectVisableController = null;

    /// <summary>연결된 문</summary>
    [Header("Anyone can edit")]
    [SerializeField]
    private CDoor _door = null;

    /// <summary>애니메이터</summary>
    private Animator _animator = null;

    /// <summary>이미 키를 습득했는지 여부</summary>
    bool _isGet = false;

    private void Awake()
    {
        _door.RegisterKey();
        _animator = GetComponent<Animator>();
    }

    /// <summary>키 습득</summary>
    public void GetKey()
    {
        if (!_isGet)
            StartCoroutine(GetKeyLogic());
    }

    /// <summary>키 습득 로직</summary>
    private IEnumerator GetKeyLogic()
    {
        _animator.SetBool("IsHave", true);

        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
        {
            CPlayerManager.Instance.IsCanOperation = false;
            CPlayerManager.Instance.Controller3D.Stop();
        }
        
        while (true)
        {
            AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("Have") && animatorStateInfo.normalizedTime >= 1.5f)
                break;

            yield return null;
        }

        if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
        {
            CCameraController.Instance.IsLerpMove = true;
            CCameraController.Instance.Target = _door.transform;
        }

        _door.GetKey();
        CWorldManager.Instance.RemoveEffectVisableController(_effectVisableController);

        gameObject.SetActive(false);
    }
}
