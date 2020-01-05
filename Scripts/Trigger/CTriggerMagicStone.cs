using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTriggerMagicStone : MonoBehaviour
{
    /// <summary>적용 숲숲이</summary>
    [SerializeField]
    private List<CSoopManager> _targetSoops = new List<CSoopManager>();

    /// <summary>활성화 적용 범위</summary>
    [SerializeField]
    private float _checkDistance = 0f;

    private bool _isActive = false;
    /// <summary>매직스톤 활성화 여부</summary>
    public bool IsActive { set { _isActive = value; } }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _checkDistance);
    }
#endif

    private void Update()
    {
        if(_isActive)
        {
            foreach(CSoopManager soopManager in _targetSoops)
            {
                if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View3D))
                {
                    if (Vector3.Distance(transform.position, soopManager.Controller3D.transform.position) <= _checkDistance)
                    {
                        soopManager.ActivateDead(true);
                        continue;
                    }
                }
                else if (CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.View2D))
                {
                    if (Vector3.Distance(transform.position, soopManager.Controller2D.transform.position) <= _checkDistance)
                    {
                        soopManager.ActivateDead(true);
                        continue;
                    }
                }

                if(!CWorldManager.Instance.CurrentWorldState.Equals(EWorldState.Changing))
                    soopManager.ActivateDead(false);
            }
        }
        else
        {
            foreach(CSoopManager soopManager in _targetSoops)
            {
                if (soopManager.Equals(null))
                    continue;

                soopManager.ActivateDead(false);
            }
        }
    }
}
