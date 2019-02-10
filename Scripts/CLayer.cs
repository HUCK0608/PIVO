using UnityEngine;

public static class CLayer
{
    private static int _viewChagneRect = LayerMask.NameToLayer("ViewChangeRect");
    public static int ViewChangeRect { get { return _viewChagneRect; } }

    private static int _player = LayerMask.NameToLayer("Player");
    public static int Player { get { return _player; } }

    /// <summary>1로 현재 값만큼 좌측 시프트 연산을 시행</summary>
    public static int LeftShiftToOne (this int value)
    {
        return 1 << value;
    }
}
