using UnityEngine;

public static class CLayer
{
    private static int _ignoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    public static int IgnoreRaycast { get { return _ignoreRaycast; } }

    private static int _viewChagneRect = LayerMask.NameToLayer("ViewChangeRect");
    public static int ViewChangeRect { get { return _viewChagneRect; } }

    private static int _player = LayerMask.NameToLayer("Player");
    public static int Player { get { return _player; } }

    private static int _backgroundObject = LayerMask.NameToLayer("BackgroundObject");
    public static int BackgroundObject { get { return _backgroundObject; } }

    private static int _pushTile = LayerMask.NameToLayer("PushTile");
    public static int PushTile { get { return _pushTile; } }

    private static int _offBlockOnPut = LayerMask.NameToLayer("OffBlockOnPut");
    public static int OffBlockOnPut { get { return _offBlockOnPut; } }

    /// <summary>1로 현재 값만큼 좌측 시프트 연산을 시행</summary>
    public static int LeftShiftToOne (this int value)
    {
        return 1 << value;
    }
}
