using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CStageData
{
    [SerializeField]
    private int _maxBiscuitCount = 0;
    private int _haveBiscuitCount = 0;
    private bool _isUnlock = false;

    private List<string> _haveBiscuitPath = null;
}
