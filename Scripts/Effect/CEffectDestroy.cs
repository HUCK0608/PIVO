using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEffectDestroy : MonoBehaviour
{
    // 활성화 시간
    [SerializeField]
    private float _activeTime = 0f;

    private float _addTime = 0f;

    private void Update()
    {
        _addTime += Time.deltaTime;

        if (_addTime >= _activeTime)
            gameObject.SetActive(false);
    }
}
