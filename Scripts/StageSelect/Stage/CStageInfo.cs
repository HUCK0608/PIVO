using System;
using UnityEngine;

public enum EStageDirection { Left, Right, Up, Down }

[Serializable]
public class CStageInfo
{
    [SerializeField]
    private CStage _stage = null;
    [SerializeField]
    private EStageDirection _direction = 0;

    public CStage Stage { get { return _stage; } }
    public EStageDirection Direction { get { return _direction; } }
}
