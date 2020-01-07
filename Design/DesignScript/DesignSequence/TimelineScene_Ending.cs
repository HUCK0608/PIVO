using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineScene_Ending : MonoBehaviour
{
    public Design_EndingTimeLineColorEdit SequenceScript;

    public Color DefaultSkyBox, DefaultFogColor, DefaultMatColor;
    //DefaultSkyBoxColor = new Color(0.502f, 0.502f, 0.502f);
    //DefaultFogColor = new Color(0.510f, 0.125f, 0.169f);
    //DefaultMatColor = new Color(1f, 0f, 0f);

    private PlayableDirector EndingSequence;

    void Start()
    {
        EndingSequence = GetComponent<PlayableDirector>();

        InitializeColor();
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        float TargetDuration = Mathf.Floor((float)EndingSequence.duration);

        yield return new WaitUntil(() => EndingSequence.time >= TargetDuration);

        InitializeColor();
    }

    void InitializeColor()
    {
        RenderSettings.skybox.SetColor("_Tint", DefaultSkyBox);
        RenderSettings.fogColor = DefaultFogColor;

        foreach (var v in SequenceScript.TargetMatArray)
            v.SetColor("_EmissionColor", DefaultMatColor);
    }
}
