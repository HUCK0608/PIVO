using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class CGrassToSnowCut : MonoBehaviour
{
    private void Awake()
    {
        // 0 : True, 1 : False
        int ShowGrassToSnowCut = PlayerPrefs.GetInt("ShowGrassToSnowCut", 0);

        if (ShowGrassToSnowCut.Equals(1))
            LoadSnowStageSelect();
        else
            StartCoroutine(PlayLevelSequence());

    }

    public void LoadSnowStageSelect()
    {
        PlayerPrefs.SetInt("ShowGrassToSnowCut", 1);
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
