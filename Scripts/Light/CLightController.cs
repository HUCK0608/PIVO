using UnityEngine;

public class CLightController : MonoBehaviour
{
    private static CLightController _instance;
    /// <summary>라이트 컨트롤러 싱글턴</summary>
    public static CLightController Instance { get { return _instance; } }

    private Light _light;

    private void Awake()
    {
        _instance = this;

        _light = GetComponent<Light>();
    }

    public void SetShadows(bool value)
    {
        _light.shadows = value ? LightShadows.Soft : LightShadows.None;
    }
}
