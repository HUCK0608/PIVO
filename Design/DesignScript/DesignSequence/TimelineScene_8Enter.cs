using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineScene_8Enter : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(PlayLevelSequence());
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        SceneManager.LoadScene("SnowStage_Stage8");
    }
}
