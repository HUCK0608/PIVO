using UnityEngine;

public class SoundPlayerOnAwake_BGM : MonoBehaviour
{
    [SerializeField]
    private SoundData_BGM _soundData = null;

    private AudioSource _audioSource = null;

    private void Start()
    {
        Play();
    }

    private void Play()
    {
        EBGMType bgmType = _soundData.BGMType;
        bool useLoop = _soundData.UseLoop;
        float delay = _soundData.Delay;

        if (delay.Equals(0f))
            _audioSource = SoundManager.Instance.PlayBGM(bgmType, useLoop);
        else
            _audioSource = SoundManager.Instance.PlayBGM(bgmType, useLoop, delay);
    }

    public void Stop()
    {
        SoundManager.Instance.Stop(_audioSource);
    }
}
