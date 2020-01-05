using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Design_EndingTimeLineColorEdit : MonoBehaviour
{
    public List<Material> TargetMatArray;

    public Color SkyboxBlueColor;
    public Color FogBlueColor;
    public Color EmissionColor;
    
    public float LerpSpeed;
    public float LerpVar;

    Color DefaultSkyboxColor;
    Color DefaultFogColor;
    Color DefaultTileColor;

    void Start()
    {
        DefaultSkyboxColor = RenderSettings.skybox.GetColor("_Tint");
        DefaultFogColor = RenderSettings.fogColor;
        DefaultTileColor = TargetMatArray[0].GetColor("_EmissionColor");
        
        StartCoroutine(ChangeColorToBlue());
    }

    IEnumerator ChangeColorToBlue()
    {
        while (LerpVar < 1)
        {
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(DefaultSkyboxColor, SkyboxBlueColor, LerpVar));
            RenderSettings.fogColor = Color.Lerp(DefaultFogColor, FogBlueColor, LerpVar);

            foreach (var v in TargetMatArray)
                v.SetColor("_EmissionColor", Color.Lerp(DefaultTileColor, EmissionColor, LerpVar));

            LerpVar += LerpSpeed;
            yield return new WaitForFixedUpdate();
        }
        
    }
}
