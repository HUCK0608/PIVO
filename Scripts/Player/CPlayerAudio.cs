using UnityEngine;

public class CPlayerAudio : MonoBehaviour
{
    /// <summary>오디오 소스</summary>
    private AudioSource _audioSource = null;

    /// <summary>이동 오디오 클립모음</summary>
    [SerializeField]
    private AudioClip[] _moveAudioClips = null;

    private void Awake()
    {
        _audioSource = GetComponentInParent<AudioSource>();
    }

    /// <summary>이동 오디오 재생</summary>
    public void PlayMoveAudio()
    {
        int randomValue = Random.Range(1, 101);

        if (randomValue <= 25)
            _audioSource.clip = _moveAudioClips[0];
        else if (randomValue <= 50)
            _audioSource.clip = _moveAudioClips[1];
        else if (randomValue <= 75)
            _audioSource.clip = _moveAudioClips[2];
        else
            _audioSource.clip = _moveAudioClips[3];

        _audioSource.Play();
    }
}
