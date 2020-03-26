using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineScene_4To5Cut : MonoBehaviour
{
    void Awake()
    {
        int Show4To5Cut = PlayerPrefs.GetInt("Show4To5Cut" , 0);

        if (Show4To5Cut.Equals(1))
            LoadSnowStageSelect();
        else if(Show4To5Cut.Equals(0))
            StartCoroutine(PlayLevelSequence());
    }
    public void LoadSnowStageSelect()
    {
        PlayerPrefs.SetInt("Show4To5Cut", 1);
        SceneManager.LoadScene("StageSelect_Snow");
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        LoadSnowStageSelect();
    }
}
