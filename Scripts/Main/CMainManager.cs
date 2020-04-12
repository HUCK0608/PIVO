using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CMainManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _dontDestroyManagerList = null;

    private void Awake()
    {
        DontDestroyOnLoad(_dontDestroyManagerList);
    }

    private void Start()
    {
        SceneManager.LoadScene("GrassStage_Stage1");
    }
}
