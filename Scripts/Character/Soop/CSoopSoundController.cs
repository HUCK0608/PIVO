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
        _putEndSoundRandomPlayer.Play();
    }

    public void PlayMoveSound()
    {
        _moveSoundRandomPlayer.Play();
    }
}
