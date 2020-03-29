using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_TempleColorInitalize : MonoBehaviour
{
    public List<Material> TargetMatArray = new List<Material>();

    private Color DefaultSkyBox, DefaultFogColor, DefaultMatColor;
    //DefaultSkyBoxColor = new Color(0.502f, 0.502f, 0.502f);
    //DefaultFogColor = new Color(0.510f, 0.125f, 0.169f);
    //DefaultMatColor = new Color(1f, 0f, 0f);

    void Awake()
    {
        DefaultSkyBox = new Color(0.502f, 0.502f, 0.502f);
        DefaultFogColor = new Color(0.510f, 0.125f, 0.169f);
        DefaultMatColor = new Color(1f, 0f, 0f);

        RenderSettings.skybox.SetColor("_Tint", DefaultSkyBox);
        RenderSettings.fogColor = DefaultFogColor;

        foreach (var v in TargetMatArray)
            v.SetColor("_EmissionColor", DefaultMatColor);
    }
}
