using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class TimelineScene_Ending2 : MonoBehaviour
{

    void Awake()
    {
        int TimelineEnding2 = PlayerPrefs.GetInt("TimelineEnding2");

        if (TimelineEnding2.Equals(1))
            LoadSnowStageSelect();
        else
            StartCoroutine(PlayLevelSequence());
    }

    private void Start()
    {
        CPlayerManager.Instance.IsCanOperation = false;

        CWorldManager.Instance.AllObjectsCanChange2D();
        CWorldManager.Instance.ChangeWorld();

        CCameraController.Instance.gameObject.SetActive(false);
    }

    public void LoadSnowStageSelect()
    {
        SceneManager.LoadScene("StageSelect_Snow");

        PlayerPrefs.SetInt("TimelineEnding2", 1);
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        SceneManager.LoadScene("Ending_Credit");
    }
}
