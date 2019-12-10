using UnityEngine;

public class CUse2DTexture : MonoBehaviour
{
    private void Awake() { GetComponentInChildren<CWorldObject>().IsUse2DTexture = true; }
}
