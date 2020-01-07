using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EndingSceneEditorMode : MonoBehaviour
{
    void Update()
    {
        if (!Application.isPlaying)
        {
            TimelineScene_Ending SequenceScript = GetComponent<TimelineScene_Ending>();

            if (SequenceScript.DefaultSkyBox != RenderSettings.skybox.GetColor("_Tint"))
                RenderSettings.skybox.SetColor("_Tint", SequenceScript.DefaultSkyBox);

            if (SequenceScript.DefaultFogColor != RenderSettings.fogColor)
                RenderSettings.fogColor = SequenceScript.DefaultFogColor;

            if (SequenceScript.DefaultMatColor != SequenceScript.SequenceScript.TargetMatArray[0].GetColor("_EmissionColor"))
            {
                foreach (var v in SequenceScript.SequenceScript.TargetMatArray)
                    v.SetColor("_EmissionColor", SequenceScript.DefaultMatColor);
            }
        }
    }
}
