using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CGrassToSnowCut : MonoBehaviour
{
    private void Awake()
    {
        // 0 : True, 1 : False
        int ShowGrassToSnowCut = PlayerPrefs.GetInt("ShowGrassToSnowCut");

        if (ShowGrassToSnowCut.Equals(1))
            LoadSnowStageSelect();
        else
            StartCoroutine(PlayLevelSequence());

    }

    public void LoadSnowStageSelect()
    {
        SceneManager.LoadScene("StageSelect_Snow");

        PlayerPrefs.SetInt("ShowGrassToSnowCut", 1);
    }

    IEnumerator PlayLevelSequence()
    {
        PlayableDirector LevelSequence = GetComponent<PlayableDirector>();
        float TargetDuration = Mathf.Floor((float)LevelSequence.duration);

        yield return new WaitUntil(() => LevelSequence.time >= TargetDuration);

        LoadSnowStageSelect();

    }
}
