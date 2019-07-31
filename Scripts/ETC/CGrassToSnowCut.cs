using UnityEngine;
using UnityEngine.SceneManagement;

public class CGrassToSnowCut : MonoBehaviour
{
    private void Awake()
    {
        // 0 : True, 1 : False
        int ShowGrassToSnowCut = PlayerPrefs.GetInt("ShowGrassToSnowCut");

        if (ShowGrassToSnowCut.Equals(1))
            LoadSnowStageSelect();
    }

    public void LoadSnowStageSelect()
    {
        SceneManager.LoadScene("StageSelect_Snow");

        PlayerPrefs.SetInt("ShowGrassToSnowCut", 1);
    }
}
