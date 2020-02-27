using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerOnAwake_SFX : MonoBehaviour
{
    [SerializeField]
    private SoundData_SFX _soundData = null;

    private AudioSource _audioSource = null;

    private void Start()
    {
        Play();
    }

    private void Play()
    {
        ESFXType sfxType = _soundData.SFXType;
        bool useLoop = _soundData.UseLoop;
        float delay = _soundData.Delay;

        if (delay.Equals(0f))
            _audioSource = SoundManager.Instance.PlaySFX(sfxType, useLoop);
        else
            _audioSource = SoundManager.Instance.PlaySFX(sfxType, useLoop, delay);
    }

    public void Stop()
    {
        SoundManager.Instance.Stop(_audioSource);
    }
}
