using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineScene_Ending : MonoBehaviour
{
    public Design_EndingTimeLineColorEdit SequenceScript;

    public Color DefaultSkyBox, DefaultFogColor, DefaultMatColor;
    //DefaultSkyBoxColor = new Color(0.502f, 0.502f, 0.502f);
    //DefaultFogColor = new Color(0.510f, 0.125f, 0.169f);
    //DefaultMatColor = new Color(1f, 0f, 0f);

    void Awake()
    {
        int TimelineEnding = PlayerPrefs.GetInt("TimelineEnding", 0);
        InitializeColor();

        if (TimelineEnding.Equals(1))
            LoadSnowStageSelect();
        else
            StartCoroutine(PlayLevelSequence());
    }
    public void LoadSnowStageSelect()
    {
        PlayerPrefs.SetInt("TimelineEnding", 1);
        SceneManager.LoadScene("StageSelect_Snow");
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        InitializeColor();
        PlayerPrefs.SetInt("TimelineEnding", 1);
        SceneManager.LoadScene("SnowStage_Ending");
    }

    void InitializeColor()
    {
        RenderSettings.skybox.SetColor("_Tint", DefaultSkyBox);
        RenderSettings.fogColor = DefaultFogColor;

        foreach (var v in SequenceScript.TargetMatArray)
            v.SetColor("_EmissionColor", DefaultMatColor);
    }
}
