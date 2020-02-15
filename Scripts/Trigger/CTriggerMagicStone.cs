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
    [SerializeField]
    private float MonoToColorSpeed = 0f;
    [SerializeField]
    private float DisableFlowSpeed = 0.5f;
    [SerializeField]
    private float EnableFlowSpeed = 1;

    private bool _isActive = false;
    /// <summary>매직스톤 활성화 여부</summary>
    public bool IsActive { set { _isActive = value; } }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _checkDistance);
    }
#endif

    public void ActiveMagicStone()
    {
        IsActive = true;
        StartCoroutine(MonoToColorful());
    }

    void SetMagicStoneMat(float Value)
    {
        Transform parentObject = transform.Find("Root3D").Find("Models");
        parentObject.Find("MagicStonePattern").GetComponent<MeshRenderer>().material.SetFloat("_Monotone", Value);
        parentObject.Find("MagicStoneCube_1").GetComponent<MeshRenderer>().material.SetFloat("_Monotone", Value);
        parentObject.Find("MagicStoneCube_2").GetComponent<MeshRenderer>().material.SetFloat("_Monotone", Value);
        parentObject.Find("MagicStoneCube_3").GetComponent<MeshRenderer>().material.SetFloat("_Monotone", Value);
    }

    //void SetMagicStoneFlowSpeed(float Value)
    //{
    //    Transform parentObject = transform.Find("Root3D").Find("Models");
    //    parentObject.Find("MagicStonePattern").GetComponent<MeshRenderer>().material.SetFloat("_FlowSpeed", Value);
    //    parentObject.Find("MagicStoneCube_1").GetComponent<MeshRenderer>().material.SetFloat("_FlowSpeed", Value);
    //    parentObject.Find("MagicStoneCube_2").GetComponent<MeshRenderer>().material.SetFloat("_FlowSpeed", Value);
    //    parentObject.Find("MagicStoneCube_3").GetComponent<MeshRenderer>().material.SetFloat("_FlowSpeed", Value);
    //}

    IEnumerator MonoToColorful()
    {
        float TargetValue = 0;

        while (TargetValue < 1)
        {
            TargetValue += MonoToColorSpeed;
            SetMagicStoneMat(TargetValue);
            //SetMagicStoneFlowSpeed(Mathf.Lerp(DisableFlowSpeed, EnableFlowSpeed, TargetValue));

            yield return new WaitForFixedUpdate();
        }

        SetMagicStoneMat(1);
        //SetMagicStoneFlowSpeed(EnableFlowSpeed);
    }

    private void Start()
    {
        //SetMagicStoneFlowSpeed(DisableFlowSpeed);
    }

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
