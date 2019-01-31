using UnityEngine;

public static class CKeyManager
{
    private static KeyCode _viewChangeExecutionKey = KeyCode.LeftShift;
    /// <summary>시점전환 실행 키</summary>
    public static KeyCode ViewChangeExecutionKey { get { return _viewChangeExecutionKey; } }

    private static KeyCode _viewChangeCancelKey = KeyCode.Escape;
    /// <summary>시점전환 취소 키</summary>
    public static KeyCode ViewChangeCancelKey { get { return _viewChangeCancelKey; } }

    private static KeyCode _viewRectScaleAdjustKey1 = KeyCode.LeftArrow;
    /// <summary>시점전환 크기 조절 1번 키</summary>
    public static KeyCode ViewRectScaleAdjustKey1 { get { return _viewRectScaleAdjustKey1; } }

    private static KeyCode _viewRectScaleAdjustKey2 = KeyCode.RightArrow;
    /// <summary>시점전환 크기 조절 2번 키</summary>
    public static KeyCode ViewRectScaleAdjustKey2 { get { return _viewRectScaleAdjustKey2; } }
}
