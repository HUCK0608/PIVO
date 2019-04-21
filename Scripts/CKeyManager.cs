using UnityEngine;

public static class CKeyManager
{
    /* In Game Key */

    private static KeyCode _climbKey = KeyCode.X;
    /// <summary>기어오르기 키</summary>
    public static KeyCode ClimbKey { get { return _climbKey; } }

    private static KeyCode _viewChangeExecutionKey = KeyCode.Z;
    /// <summary>시점전환 실행 키</summary>
    public static KeyCode ViewChangeExecutionKey { get { return _viewChangeExecutionKey; } }

    private static KeyCode _viewChangeCancelKey = KeyCode.Escape;
    /// <summary>시점전환 취소 키</summary>
    public static KeyCode ViewChangeCancelKey { get { return _viewChangeCancelKey; } }
    private static KeyCode _anotherViewChangeCancelKey = KeyCode.X;
    /// <summary>또 다른 시점전환 취소 키</summary>
    public static KeyCode AnotherViewChangeCancelKey { get { return _anotherViewChangeCancelKey; } }

    private static KeyCode _viewRectScaleAdjustKey1 = KeyCode.LeftArrow;
    /// <summary>시점전환 크기 조절 1번 키</summary>
    public static KeyCode ViewRectScaleAdjustKey1 { get { return _viewRectScaleAdjustKey1; } }
    private static KeyCode _anotherViewRectScaleAdjustKey1 = KeyCode.UpArrow;
    /// <summary>또 다른 시점전환 크기 조절 1번 키</summary>
    public static KeyCode AnotherViewRectScaleAdjustKey1 { get { return _anotherViewRectScaleAdjustKey1; } }

    private static KeyCode _viewRectScaleAdjustKey2 = KeyCode.RightArrow;
    /// <summary>시점전환 크기 조절 2번 키</summary>
    public static KeyCode ViewRectScaleAdjustKey2 { get { return _viewRectScaleAdjustKey2; } }
    private static KeyCode _anotherViewRectScaleAdjustKey2 = KeyCode.DownArrow;
    /// <summary>또 다른시점전환 크기 조절 2번 키</summary>
    public static KeyCode AnotherViewRectScaleAdjustKey2 { get { return _anotherViewRectScaleAdjustKey2; } }

    /* StageSelect Key */

    private static KeyCode _startStageKey = KeyCode.Z;
    /// <summary>스테이지 시작 키</summary>
    public static KeyCode StartStageKey { get { return _startStageKey; } }
}
