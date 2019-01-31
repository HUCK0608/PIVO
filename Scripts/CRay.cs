using UnityEngine;

public static class CRay
{
    private static Ray _ray;
    private static Plane _plane;

    /// <summary>Plane 방식의 스크린 포인트 레이</summary>
    public static Vector3 ScreenPointToRay_Plane(Vector3 inNoraml, Vector3 inPoint)
    {
        Vector3 hitPoint = Vector3.zero;

        _plane.SetNormalAndPosition(inNoraml, inPoint);

        float rayDistance = 0f;

        _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (_plane.Raycast(_ray, out rayDistance))
            hitPoint = _ray.GetPoint(rayDistance);

        return hitPoint;
    }

    /// <summary>레이를 쏴 물체가 있는지 확인</summary>
    public static bool IsInObject(Vector3 origin, Vector3 direction, float distance)
    {
        bool result = false;

        _ray.origin = origin;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, distance))
            result = true;

        return result;
    }

    /// <summary>레이를 쏴 물체가 있는지 확인</summary>
    public static bool IsInObject(Vector3 origin, Vector3 direction, out RaycastHit hit, float distance)
    {
        bool result = false;

        _ray.origin = origin;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, out hit, distance))
            result = true;

        return result;
    }

    /// <summary>레이를 쏴 물체가 있는지 확인</summary>
    public static bool IsInObject(Vector3 origin, Vector3 direction, float distance, int layerMask)
    {
        bool result = false;

        _ray.origin = origin;
        _ray.direction = direction;

        if (Physics.Raycast(_ray, distance, layerMask))
            result = true;

        return result;
    }

    /// <summary>2D레이를 쏴 물체가 있는지 확인</summary>
    public static bool IsInObject2D(Vector2 origin, Vector2 direction, float distance, int layerMask)
    {
        bool result = false;

        if (Physics2D.Raycast(origin, direction, distance, layerMask))
            result = true;

        return result;
    }
}
