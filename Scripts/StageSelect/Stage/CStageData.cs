using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CStageData
{
    private int _maxBiscuitCount = 0;
    /// <summary>최대 비스킷 개수</summary>
    public int MaxBiscuitCount { set { _maxBiscuitCount = value; } }

    private int _haveBiscuitCount = 0;
    private bool _isUnlock = false;

    private List<string> _haveBiscuitPath = null;
}
