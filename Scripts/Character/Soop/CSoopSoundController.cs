using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSoopSoundController : MonoBehaviour
{
    [SerializeField]
    private SoundRandomPlayer_SFX _putEndSoundRandomPlayer = null;

    [SerializeField]
    private SoundRandomPlayer_SFX _moveSoundRandomPlayer = null;

    public void PlayPutEndSound()
    {
        if (null != _putEndSoundRandomPlayer)
            _putEndSoundRandomPlayer.Play();
    }

    public void PlayMoveSound()
    {
        if (null != _moveSoundRandomPlayer)
            _moveSoundRandomPlayer.Play();
    }
}
