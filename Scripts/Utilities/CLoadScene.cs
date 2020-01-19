using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CLoadScene : MonoBehaviour
{
    [SerializeField]
    private string _sceneName = string.Empty;
    [SerializeField]
    private float _delayTime = 0f;

    private float _currentTime = 0f;

    private bool _isLoading = false;

    void Update()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _delayTime)
            LoadScene();
    }

    private void LoadScene()
    {
        if (_isLoading)
            return;

        SceneManager.LoadScene(_sceneName);
        _isLoading = true;
    }
}
