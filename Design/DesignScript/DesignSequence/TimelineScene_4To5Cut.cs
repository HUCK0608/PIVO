using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineScene_4To5Cut : MonoBehaviour
{
    void Awake()
    {
        int Show4To5Cut = PlayerPrefs.GetInt("Show4To5Cut");

        if (Show4To5Cut.Equals(1))
            LoadSnowStageSelect();
        else
            StartCoroutine(PlayLevelSequence());
    }
    public void LoadSnowStageSelect()
    {
        SceneManager.LoadScene("StageSelect_Snow");

        PlayerPrefs.SetInt("Show4To5Cut", 1);
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        LoadSnowStageSelect();
    }
}
