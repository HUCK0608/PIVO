using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CCameraController_StageSelect : MonoBehaviour
{
    private static CCameraController_StageSelect _instance = null;
    public static CCameraController_StageSelect Instance { get { return _instance; } }

    [SerializeField]
    private BlurOptimized _blurOptimized = null;

    private void Awake()
    {
        _instance = this;
    }

    public void SetActivateBlur(bool active)
    {
        _blurOptimized.enabled = active;
    }
}
