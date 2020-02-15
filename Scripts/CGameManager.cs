using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CGameManager : MonoBehaviour
{
    public static CGameManager _instance = null;
    public static CGameManager Instance { get { return _instance; } }

    /// <summary>마우스 숨기기 시간</summary>
    [SerializeField]
    private float _mouseHideOnTime = 3f;

    /// <summary>마우스가 현재 움직이지 않은 시간</summary>
    private float _mouseNotMoveTime = 0f;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        // Move
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            if (false == Cursor.visible)
                Cursor.visible = true;

            _mouseNotMoveTime = 0f;
        }
        // NotMove
        else
            _mouseNotMoveTime += Time.deltaTime;

        // Hide
        if (true == Cursor.visible && _mouseNotMoveTime >= _mouseHideOnTime)
            Cursor.visible = false;
    }
}
