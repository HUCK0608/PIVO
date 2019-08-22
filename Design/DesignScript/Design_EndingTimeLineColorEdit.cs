using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] //테스트 끝나면 삭제
public class Design_EndingTimeLineColorEdit : MonoBehaviour
{
    public bool SkyboxMaterialTrigger;
    public Color SkyboxBlueColor;
    public Color FogBlueColor;
    public float LerpSpeed;
    public float LerpVar;

    Color DefaultSkyboxColor;
    Color DefaultFogColor;
    
    void Start()
    {
        DefaultSkyboxColor = new Color(0.5f, 0.5f, 0.5f);
        DefaultFogColor = RenderSettings.fogColor;
        StartCoroutine("ChangeColorToBlue");
    }

    IEnumerator ChangeColorToBlue()
    {
        while (LerpVar < 1)
        {
            RenderSettings.skybox.SetColor("_Tint", Color.Lerp(DefaultSkyboxColor, SkyboxBlueColor, LerpVar));
            RenderSettings.fogColor = Color.Lerp(DefaultFogColor, FogBlueColor, LerpVar);
            LerpVar += LerpSpeed;
            yield return new WaitForSeconds(0.02f);
        }
        
    }

    void Update() 
    {
        
        if(SkyboxMaterialTrigger)//테스트 종료시 삭제할 코드
        {
            RenderSettings.skybox.SetColor("_Tint", DefaultSkyboxColor);
            RenderSettings.fogColor = DefaultFogColor;
            LerpVar = 0;
        }
        
    }
}
